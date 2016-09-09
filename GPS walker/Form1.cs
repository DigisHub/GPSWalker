using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using System.Diagnostics;
using System.IO;
using System.Device.Location;
using System.Threading;

namespace GPS_walker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        GMapOverlay markerOverlay;
        GMarkerGoogle currentPositionMarker;
        GMarkerGoogle destinationMarker;
        MapRoute route;
        GMapRoute routeMarker;
        bool gettingCurrentPosition, isRunning;
        PointLatLng currentLatLng, destinationLatLng, stepLatLng;
        int speed, sleepMin, sleepMax, jitter;

        Process fakeGPS, PokeGet;
        double speedms;
        TimeSpan timeToDestination;
        Stopwatch stopwatch;
        Random r;
        List<RoutePoints> routePoints;
        double totalDistance;
        int routeStep;

        List<SavedRoute> savedRoute;
        bool onRoute = false;
        int savedRouteIndex;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["stepSleepMinMilliseconds"], out sleepMin);
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["stepSleepMaxMilliseconds"], out sleepMax);


            map.Position = PointLatLng.Empty;
            map.MapProvider = GMapProviders.GoogleMap;
            map.Manager.Mode = GMap.NET.AccessMode.ServerAndCache;

            markerOverlay = new GMapOverlay();
            currentPositionMarker = new GMarkerGoogle(PointLatLng.Empty, GMarkerGoogleType.green);
            destinationMarker = new GMarkerGoogle(PointLatLng.Empty, GMarkerGoogleType.red);
            markerOverlay.Markers.Add(destinationMarker);
            markerOverlay.Markers.Add(currentPositionMarker);
            map.Overlays.Add(markerOverlay);
            r = new Random();
            stopwatch = new Stopwatch();
            savedRoute = new List<SavedRoute>();

            SaveValues s = SaveValues.ReadFromBinaryFile<SaveValues>("settings.ini");
            currentLatLng = s.Position;
            txtIP.Text = s.IP;
            chkRoads.Checked = s.UseRoads;
            chkGo.Checked = s.GoAutomatically;
            chkFollow.Checked = s.FollowPostion;
            speed = s.Speed;
            txtSpeed.Text = speed.ToString();
            jitter = s.Jitter;
            txtJitter.Text = jitter.ToString();
            if (s.Zoom > 0)
            {
                map.Zoom = s.Zoom;
            }

            map.Position = currentLatLng;
            SetCurrentPosition();

            LoadRoutes();
        }

        private void LoadRoutes()
        {            
            string[] routes = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
            foreach (string route in routes)
            {
                cmbRoutes.Items.Add(Path.GetFileName(route));
            }
        }

        private void btnSetPosition_Click(object sender, EventArgs e)
        {
            gettingCurrentPosition = true;
            btnSetPosition.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
        }

        private void SetCurrentPosition()
        {
            setText(txtLat, currentLatLng.Lat.ToString());
            setText(txtLong, currentLatLng.Lng.ToString());
            setText(txtETA, (timeToDestination - stopwatch.Elapsed).ToString(@"hh\:mm\:ss"));
            currentPositionMarker.Position = currentLatLng;

            if (chkFollow.Checked)
            {
                Action actMap = () => map.Position = currentLatLng;
                map.Invoke(actMap);
            }
        }

        private void SetDestination()
        {                    
            destinationMarker.Position = destinationLatLng;
            setText(txtdestLat, destinationLatLng.Lat.ToString());
            setText(txtDestLong, destinationLatLng.Lng.ToString());
            markerOverlay.Routes.Clear();
            routePoints = new List<RoutePoints>();
            if (chkRoads.Checked)
            {
                route = GMapProviders.OpenStreetMap.GetRoute(currentLatLng, destinationLatLng, false, false, 15);
                if (route==null)
                {
                    MessageBox.Show("No route found");
                    return;
                }
                routeMarker = new GMapRoute(route.Points, "My route");
                routePoints.Add(new RoutePoints(currentLatLng));
                foreach (var item in route.Points)
                {
                    routePoints.Add(new RoutePoints(item));
                }
                routePoints.Add(new RoutePoints(destinationLatLng));
                CalculateCumulativeDistance();
                totalDistance = routePoints.Last().CumulativeDistance;
                markerOverlay.Routes.Add(routeMarker);
            }
            else
            {
                totalDistance = currentLatLng.ToGeoCoordinate().GetDistanceTo(destinationLatLng.ToGeoCoordinate());
                routePoints.Add(new RoutePoints(currentLatLng));
                routePoints.Add(new RoutePoints(destinationLatLng, totalDistance));
            }
            if (speed > 0)
            {
                speedms = (double)speed * 1000 / 60 / 60; // km/h > m/h > m/m > m/s
                timeToDestination = TimeSpan.FromSeconds(totalDistance / speedms);
                setText(txtETA, timeToDestination.ToString(@"hh\:mm\:ss"));
            }
            Action r = () => this.Refresh();
            this.Invoke(r);
            if (chkGo.Checked)
            {
                GoToDestination();
            }
        }

        private void CalculateCumulativeDistance()
        {
            for (int i = 1; i < routePoints.Count; i++)
            {
                routePoints[i].CumulativeDistance = routePoints[i - 1].CumulativeDistance + routePoints[i - 1].Point.ToGeoCoordinate().GetDistanceTo(routePoints[i].Point.ToGeoCoordinate());
            }
        }

        private void GoToDestination()
        {            
            if (speed == 0)
            {
                adb(destinationLatLng);
                currentLatLng = destinationLatLng;
                SetCurrentPosition();
            }
            else
            {
                
                routeStep = 0;
                speedms = (double)speed * 1000 / 60 / 60; // km/h > m/h > m/m > m/s
                timeToDestination = TimeSpan.FromSeconds(totalDistance / speedms);
                stopwatch = Stopwatch.StartNew();
                if (!isRunning)
                {
                    isRunning = true;
                    Task.Delay(r.Next(sleepMin, sleepMax)).ContinueWith(t => MoveStep());
                }                
            }
        }

        private void MoveStep()
        {
            if (!stopwatch.IsRunning)
            {
                isRunning = false;
                if (onRoute)
                {
                    EndRoutePoint();
                }
                return;
            }

            if (stopwatch.ElapsedMilliseconds > timeToDestination.TotalMilliseconds)
            {
                stepLatLng = destinationLatLng;
                stopwatch.Stop();
            }
            else
            {
                double distanceTraveled = speedms * stopwatch.ElapsedMilliseconds / 1000;
                for (int i = routeStep; i < routePoints.Count; i++)
                {
                    if (routePoints[i].CumulativeDistance > distanceTraveled)
                    {
                        routeStep = i;
                        break;
                    }
                }
                stepLatLng = Extender.Lerp(routePoints[routeStep - 1].Point, routePoints[routeStep].Point, distanceTraveled / routePoints[routeStep].CumulativeDistance);
            }
            
            adb(stepLatLng, true);
        }

        private void FakeGPS_Exited(object sender, EventArgs e)
        {
            currentLatLng = stepLatLng;
            SetCurrentPosition(); 
            Task.Delay(r.Next(sleepMin, sleepMax)).ContinueWith(t => MoveStep());
        }

        private void map_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var ltlg = map.FromLocalToLatLng(e.X, e.Y);

            if (gettingCurrentPosition)
            {
                currentLatLng = ltlg;

                gettingCurrentPosition = false;
                btnSetPosition.BackColor = Color.FromKnownColor(KnownColor.Control);
                SetCurrentPosition();
            }
            else
            {
                destinationLatLng = ltlg;
                SetDestination();
            }
        }

        private void btnAdbConnect_Click(object sender, EventArgs e)
        {
            adb(string.Format(" connect {0}:5555", txtIP.Text), false);
        }

        private void txtPosition_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPosition.Text) && ParseCoords(txtPosition.Text, ref currentLatLng))
            {
                SetCurrentPosition();
            }
            txtPosition.Text = string.Empty;
        }

        private void txtDestination_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDestination.Text) && ParseCoords(txtDestination.Text, ref destinationLatLng))
            {
                SetDestination();
            }
            txtDestination.Text = string.Empty;
        }

        private bool ParseCoords(string text, ref PointLatLng point)
        {
            //13.705755, -89.2516574
            string[] coords = text.Split(',');
            double tmpLat, tmpLng;
            if (double.TryParse(coords[0].Trim(), out tmpLat)
                && double.TryParse(coords[1].Trim(), out tmpLng))
            {
                point.Lat = tmpLat;
                point.Lng = tmpLng;
                return true;
            }
            MessageBox.Show("Error parsing coordinates");
            return false;
        }

        private void txtSpeed_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSpeed.Text, out speed))
            {
                txtSpeed.Text = speed.ToString();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            txtETA.Text = string.Empty;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save current status
            SaveValues s = new SaveValues
            {
                IP = txtIP.Text,
                Position = currentLatLng,
                FollowPostion = chkFollow.Checked,
                GoAutomatically = chkGo.Checked,
                UseRoads = chkRoads.Checked,
                Speed = speed,
                Zoom = map.Zoom,
                Jitter = jitter
            };
            SaveValues.WriteToBinaryFile<SaveValues>("settings.ini", s, false);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            SetDestination();
            GoToDestination();
        }

        private void cmbRoutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstSavedRoute.Items.Clear();
            savedRoute.Clear();

            string[] routeFile = File.ReadAllLines(cmbRoutes.SelectedItem.ToString());
            //Name, Lat, Lng, Pokestop, UseRoads, Speed, Jitter
            for (int i = 1; i < routeFile.Length; i++)
            {
                var line = routeFile[i].Split(',');

                try
                {
                    PointLatLng p = PointLatLng.Empty;

                    ParseCoords(string.Format("{0},{1}", line[1], line[2]), ref p);
                    SavedRoute sr = new SavedRoute
                    {
                        Name = line[0],
                        Location = p,
                        PokeStop = bool.Parse(line[3]),
                        UseRoads = bool.Parse(line[4]),
                        Speed = int.Parse(line[5]),
                        Jitter = int.Parse(line[6])
                    };
                    savedRoute.Add(sr);
                }
                catch
                {
                }
            }

            savedRoute.ForEach(a => lstSavedRoute.Items.Add(a.ToString()));

        }

        private void btnRoute_Click(object sender, EventArgs e)
        {
            if (savedRoute.Count == 0)
            {
                MessageBox.Show("No Route selected");
                return;
            }

            onRoute = true;
            if (lstSavedRoute.SelectedIndex == -1)
            {
                lstSavedRoute.SelectedIndex = 0;
                savedRouteIndex = 0;
            }
            StartRoute();
        }

        private void StartRoute()
        {
            //load values from route into current destination
            destinationLatLng = savedRoute[savedRouteIndex].Location;
            speed = savedRoute[savedRouteIndex].Speed;
            setText(txtSpeed, speed.ToString());
            jitter = savedRoute[savedRouteIndex].Jitter;
            setText(txtJitter, jitter.ToString());
            if (chkRoads.InvokeRequired)
            {
                Action a = () => chkRoads.Checked = savedRoute[savedRouteIndex].UseRoads;
                chkRoads.Invoke(a);
            }
            else
            {
                chkRoads.Checked = savedRoute[savedRouteIndex].UseRoads;
            }            
            SetDestination();
            GoToDestination();
        }

        private void EndRoutePoint()
        {
            if (savedRoute[savedRouteIndex].PokeStop)
            {
                //Get Pokestop
                setText(txtAdbResult, string.Format("[{0}]: Getting pokestop {1}", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), savedRoute[savedRouteIndex].Name));
                if (this.InvokeRequired)
                {
                    Action a = () => this.Refresh();
                    this.Invoke(a);
                }

                PokeGet = new Process();
                ProcessStartInfo psi = new ProcessStartInfo(System.Configuration.ConfigurationManager.AppSettings["pokeget"], string.Format(" {0} {1}", savedRoute[savedRouteIndex].Location.Lat, savedRoute[savedRouteIndex].Location.Lng))
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WorkingDirectory = Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings["pokeget"])
                };
                PokeGet.StartInfo = psi;

                PokeGet.EnableRaisingEvents = true;
                PokeGet.Exited += PokeGet_Exited;

                PokeGet.Start();
                var output = fakeGPS.StandardOutput.ReadToEndAsync();
                PokeGet.WaitForExit();
                Action actResult = () => txtAdbResult.AppendText(string.Format("[{0}]: {1}", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), output.Result));
                txtAdbResult.Invoke(actResult);
            }
            else
            {
                NextRouteStep();
            }
        }

        private void PokeGet_Exited(object sender, EventArgs e)
        {
            NextRouteStep();
        }

        private void NextRouteStep()
        {
            if (savedRouteIndex == savedRoute.Count - 1)
            {
                savedRouteIndex = 0;
            }
            else
            {
                savedRouteIndex++;
            }

            if (lstSavedRoute.InvokeRequired)
            {
                Action a = () => lstSavedRoute.SelectedIndex = savedRouteIndex;
                lstSavedRoute.Invoke(a);
            }
            else
            {
                lstSavedRoute.SelectedIndex = savedRouteIndex;
            }

            StartRoute();
        }

        private void lstSavedRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            savedRouteIndex = lstSavedRoute.SelectedIndex;
        }

        private void btnStopRoute_Click(object sender, EventArgs e)
        {
            onRoute = false;
            lstSavedRoute.SelectedIndex = -1;
            savedRouteIndex = 0;
            btnStop_Click(sender, e);
        }

        private void txtJitter_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtJitter.Text, out jitter))
            {
                txtJitter.Text = jitter.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLat.Text = string.Empty;
            txtLong.Text = string.Empty;
        }

        private void adb(string command, bool withEvent)
        {
            setText(txtAdbResult, string.Format("[{0}]: {1}{2}{3}", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), "adb.exe", command, "\r\n"));
            if (this.InvokeRequired)
            {
                Action a = () => this.Refresh();
                this.Invoke(a);
            }

            fakeGPS = new Process();
            ProcessStartInfo psi = new ProcessStartInfo(System.Configuration.ConfigurationManager.AppSettings["adb"], command)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            fakeGPS.StartInfo = psi;
            if (withEvent)
            {
                fakeGPS.EnableRaisingEvents = true;
                fakeGPS.Exited += FakeGPS_Exited;
            }
            fakeGPS.Start();
            var output = fakeGPS.StandardOutput.ReadToEndAsync();
            fakeGPS.WaitForExit();
            Action actResult = () => txtAdbResult.AppendText(string.Format("[{0}]: {1}", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), output.Result));
            txtAdbResult.Invoke(actResult);
            //setText(txtAdbResult, string.Format("{3}[{0}]: {1}{2}", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), output.Result, "\r\n", txtAdbResult.Text));
        }

        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            setText(txtAdbResult, string.Format("{3}[{0}]: {1}{2}", DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), outLine.Data, "\r\n", txtAdbResult.Text));
        }

        private void adb(PointLatLng coords, bool withEvent = false)
        {
            //0.00001 decimal degrees ~= 1.11m
            double offset = r.NextDouble() * jitter * 0.00001  / 1.11 * (r.NextDouble() > 0.5 ? -1 : 1);
            coords.Lat += offset;
            coords.Lng += offset;

            adb(string.Format(" shell am startservice -a com.incorporateapps.fakegps.ENGAGE --ef lat {0} --ef lng {1}", coords.Lat, coords.Lng), withEvent);
        }

        private void setText(TextBox txt, string text)
        {
            if (txt.InvokeRequired)
            {
                Action act = () => txt.Text = text;
                txt.Invoke(act);
            }
            else
            {
                txt.Text = text;
            }
        }
    }
}
