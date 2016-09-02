using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_walker
{
    class RoutePoints
    {
        public PointLatLng Point { get; set; }
        public double CumulativeDistance { get; set; }

        public RoutePoints()
        { }

        public RoutePoints(PointLatLng point)
        {
            Point = point;
        }
        public RoutePoints(PointLatLng point, double distance)
        {
            Point = point;
            CumulativeDistance = distance;
        }
    }
}
