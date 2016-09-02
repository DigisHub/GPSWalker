using GMap.NET;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_walker
{
    public static class Extender
    {

        public static GeoCoordinate ToGeoCoordinate(this PointLatLng point)
        {
            return new GeoCoordinate(point.Lat, point.Lng);
        }

        public static PointLatLng Lerp(PointLatLng firstPoint, PointLatLng secondPoint, double by)
        {
            double lat = Lerp(firstPoint.Lat, secondPoint.Lat, by);
            double lng = Lerp(firstPoint.Lng, secondPoint.Lng, by);
            return new PointLatLng(lat, lng);
        }

        public static double Lerp(double firstFloat, double secondFloat, double by)
        {
            by = by > 1 ? 1 : by;
            return firstFloat * (1 - by) + secondFloat * by;
        }
    }
}
