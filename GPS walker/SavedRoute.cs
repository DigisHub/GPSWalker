using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_walker
{
    [Serializable]
    public class SavedRoute
    {
        public string Name { get; set; }
        public PointLatLng Location { get; set; }
        public bool PokeStop { get; set; }
        public bool UseRoads { get; set; }
        public int Speed { get; set; }
        public int Jitter { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. Pokestop: {3}, Roads: {4}, Speed: {5}, jitter: {6}, Location: {1},{2}", Name, Location.Lat, Location.Lng, PokeStop, UseRoads, Speed, Jitter);
        }
    }
}
