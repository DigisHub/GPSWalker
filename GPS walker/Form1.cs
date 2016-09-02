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
        bool gettingCurrentPosition;
        PointLatLng currentLatLng, destinationLatLng, stepLatLng, originLatLng;
        int speed, sleepMin, sleepMax;

        Process fakeGPS;
        double speedms;
        TimeSpan timeToDestination;
        Stopwatch stopwatch;
        Random r;
        List<RoutePoints> routePoints;
        double totalDistance;
        int routeStep;

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

            SaveValues s = SaveValues.ReadFromBinaryFile<SaveValues>("settings.ini");
            currentLatLng = s.Position;
            txtIP.Text = s.IP;
            chkRoads.Checked = s.UseRoads;
            chkGo.Checked = s.GoAutomatically;
            chkFollow.Checked = s.FollowPostion;
            speed = s.Speed;
            txtSpeed.Text = speed.ToString();
            if (s.Zoom > 0)
            {
                map.Zoom = s.Zoom;
            }

            map.Position = currentLatLng;
            SetCurrentPosition();
        }


        private void btnSetPosition_Click(object sender, EventArgs e)
        {
            gettingCurrentPosition = true;
            btnSetPosition.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
        }

        private void SetCurrentPosition()
        {
            if (txtLat.InvokeRequired)
            {
                Action actLat = () => txtLat.Text = currentLatLng.Lat.ToString();
                txtLat.Invoke(actLat);

                Action actLong = () => txtLong.Text = currentLatLng.Lng.ToString();
                txtLong.Invoke(actLong);

                currentPositionMarker.Position = currentLatLng;
                if (chkFollow.Checked)
                {
                    Action actMap = () => map.Position = currentLatLng;
                    map.Invoke(actMap);
                }             
                
                Action actETA = () => txtETA.Text = (timeToDestination - stopwatch.Elapsed).ToString(@"hh\:mm\:ss");
                txtETA.Invoke(actETA);
            }
            else
            {
                txtLat.Text = currentLatLng.Lat.ToString();
                txtLong.Text = currentLatLng.Lng.ToString();
                currentPositionMarker.Position = currentLatLng;
                if (chkFollow.Checked)
                {
                    map.Position = currentLatLng;
                }
            }            
        }

        private void SetDestination()
        {
            destinationMarker.Position = destinationLatLng;
            txtdestLat.Text = destinationLatLng.Lat.ToString();
            txtDestLong.Text = destinationLatLng.Lng.ToString();
            markerOverlay.Routes.Clear();
            this.Refresh();
            routePoints = new List<RoutePoints>();
            if (chkRoads.Checked)
            {
                route = GMapProviders.OpenStreetMap.GetRoute(currentLatLng, destinationLatLng, false, false, 15);
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
                Process p;
                p = Process.Start(System.Configuration.ConfigurationManager.AppSettings["adb"], string.Format(" shell am startservice -a com.incorporateapps.fakegps.ENGAGE --ef lat {0} --ef lng {1}", destinationLatLng.Lat, destinationLatLng.Lng));
                p.WaitForExit();
                currentLatLng = destinationLatLng;
                SetCurrentPosition();
            }
            else
            {
                routeStep = 0;
                speedms = (double)speed * 1000 / 60 / 60; // km/h > m/h > m/m > m/s
                timeToDestination = TimeSpan.FromSeconds(totalDistance / speedms);
                originLatLng = currentLatLng;
                stopwatch = Stopwatch.StartNew();
                MoveStep();
            }
        }

        private void MoveStep()
        {
            if (stopwatch.ElapsedMilliseconds > timeToDestination.TotalMilliseconds)
            {
                stepLatLng = destinationLatLng;
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

            fakeGPS = Process.Start(System.Configuration.ConfigurationManager.AppSettings["adb"], string.Format(" shell am startservice -a com.incorporateapps.fakegps.ENGAGE --ef lat {0} --ef lng {1}", stepLatLng.Lat, stepLatLng.Lng));
            fakeGPS.EnableRaisingEvents = true;
            fakeGPS.Exited += FakeGPS_Exited;
        }

        private void FakeGPS_Exited(object sender, EventArgs e)
        {
            currentLatLng = stepLatLng;
            SetCurrentPosition();
            if (stopwatch.IsRunning)
            {                
                System.Threading.Thread.Sleep(r.Next(sleepMin, sleepMax));
                if (stopwatch.ElapsedMilliseconds > timeToDestination.TotalMilliseconds)
                {
                    stopwatch.Stop();
                }
                MoveStep();
            }
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
            Process.Start(System.Configuration.ConfigurationManager.AppSettings["adb"], string.Format(" connect {0}:5555", txtIP.Text));
        }

        private void txtPosition_Leave(object sender, EventArgs e)
        {
            if (ParseCoords(txtPosition.Text, ref currentLatLng))
            {
                SetCurrentPosition();
            }
            txtPosition.Text = string.Empty;
        }

        private void txtDestination_Leave(object sender, EventArgs e)
        {
            if (ParseCoords(txtDestination.Text, ref destinationLatLng))
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
            fakeGPS.EnableRaisingEvents = true;
            fakeGPS.Exited -= FakeGPS_Exited;
            txtETA.Text = string.Empty;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save current status
            SaveValues s = new SaveValues { IP = txtIP.Text, Position = currentLatLng, FollowPostion = chkFollow.Checked,
                GoAutomatically = chkGo.Checked, UseRoads = chkRoads.Checked, Speed = speed, Zoom = map.Zoom };
            SaveValues.WriteToBinaryFile<SaveValues>("settings.ini", s, false);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            GoToDestination();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLat.Text = string.Empty;
            txtLong.Text = string.Empty;
        }
    }
}
