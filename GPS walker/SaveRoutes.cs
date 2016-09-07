using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_walker
{
    [Serializable]
    public class SaveRoutes
    {
        public int Name { get; set; }
        public PointLatLng Location { get; set; }
        public bool PokeStop { get; set; }
        public bool UseRoads { get; set; }
        public int Speed { get; set; }
        public int Jitter { get; set; }
    }
}
