using Newtonsoft.Json;
using PathScheduler.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PathScheduler.Helpers
{
    public static class DistanceMatrixProvider
    {
        public static DistanceMatrixResponse GetDistanceMatrix(List<MapPoint> points)
        {
            List<GeoData> geoPoints = GetGeoDataFromMapPointsList(points);
            string geoPointsString = CreateGeoPointsString(geoPoints);
            string requestUrl = CreateDistanceMatrixRequestUrl(geoPointsString, "driving", CredentialsProvider.ApiKey);
            WebRequest matrixWebRequest = WebRequest.Create(requestUrl);
            Stream matrixDataStream = matrixWebRequest.GetResponse().GetResponseStream();
            var dataReader = new StreamReader(matrixDataStream);
            string matrixResponseString = dataReader.ReadToEnd();
            return JsonConvert.DeserializeObject<DistanceMatrixResponse>(matrixResponseString);
        }

        private static List<GeoData> GetGeoDataFromMapPointsList(List<MapPoint> points)
        {
            List<GeoData> pointsGeoData = new List<GeoData>();
            foreach (var point in points)
            {
                pointsGeoData.Add(new GeoData(point.CoordX, point.CoordY));
            }
            return pointsGeoData;
        }

        private static string CreateGeoPointsString(List<GeoData> geoPoints)
        {
            NumberFormatInfo format = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
            List<string> geoPointsStrings = new List<string>();
            foreach (var geoPoint in geoPoints)
            {
                geoPointsStrings.Add(geoPoint.latitude.ToString(format) + "," + geoPoint.longitude.ToString(format));
            }
            return string.Join(";", geoPointsStrings);
        }

        private static string CreateDistanceMatrixRequestUrl(string geoPointsString, string travelMode, string apiKey)
        {
            return "https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix"
                    + "?origins="
                    + geoPointsString
                    + "&destinations="
                    + geoPointsString
                    + "&travelMode="
                    + travelMode
                    + "&key="
                    + apiKey;
        }
    }
}
