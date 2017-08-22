using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace apitesting.Models
{
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public static Location GetLocation(string search)
        {
            var client = new RestClient("https://maps.googleapis.com/maps/api/geocode");
            var request = new RestRequest("json?address=" + search + "&key=" + EnvironmentVariables.GeoCodeApi);
    
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            Console.WriteLine(jsonResponse["results"][0]);
            var geometryList = JsonConvert.DeserializeObject<Location>(jsonResponse["results"][0]["geometry"]["location"].ToString());
            Console.WriteLine(geometryList);
            //foreach (var geometry in geometryList)
            //{
            //    Console.WriteLine("lat: {0}", geometry.location.lat);
            //    Console.WriteLine("lng: {0}", geometry.location.lng);
            //}
            return geometryList;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
