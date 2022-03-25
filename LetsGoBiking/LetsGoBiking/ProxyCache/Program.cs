// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Threading;
using System.Web;
using System.Net;
using System.Text.Json;

namespace ProxyCache
{

    class Program
    {
        public class Root
        {
            public int number { get; set; }
            public string contract_name { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public Position position { get; set; }
            public bool banking { get; set; }
            public bool bonus { get; set; }
            public int bike_stands { get; set; }
            public int available_bike_stands { get; set; }
            public int available_bikes { get; set; }
            public string status { get; set; }
            public object last_update { get; set; }
        }

        public class Position
        {
            public double lat { get; set; }
            public double lng { get; set; }
            public Position(double lat, double lng) { this.lat = lat; this.lng = lng; }
        }

        static readonly HttpClient client = new HttpClient();

        static async void Main(string[] args)
        {
            Console.WriteLine("---- Démarrage serveur ----");
            HttpListener listener = new HttpListener();
            

            ObjectCache cache = MemoryCache.Default;
            Console.WriteLine("---- Création du cache ----");

            string api_key = "fd4e518c5cf2c25e0ae2b34e82b286365c05487b";
            HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v1/stations?apiKey={api_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<Root> jsonData = JsonSerializer.Deserialize<List<Root>>(responseBody);


            //add ClassContent object obj1 in Cache 
            //without cacheItem expiration date
            var cacheItemPolicy1 = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30.0),
            };
            foreach (Root element in jsonData)
            {
                cache.Add(element.contract_name, element, cacheItemPolicy1);
            }
            cache.Add("Stations : ", jsonData, null);
            Console.WriteLine("Liste des stations enregistrée");


            //print all cache
            PrintAllCache(cache);

            Thread.Sleep(2000);
            Console.WriteLine("Attente de 2 secondes...");

            //remove cache
            //cache.Remove("Station");


            //print all cache
            PrintAllCache(cache);

            Console.ReadLine();
        }

        public static void PrintAllCache(ObjectCache cache)
        {

            DateTime dt = DateTime.Now;

            Console.WriteLine("Toutes les clés-valeurs de " + dt.Second);
            //loop through all key-value pairs and print them
            foreach (var item in cache)
            {
                Console.WriteLine("Objet cache clé-valeur: " + item.Key + "-" + item.Value);
            }
        }

        public static void UpdateCache(ObjectCache cache)
        {

            DateTime dt = DateTime.Now;

            Console.WriteLine("Toutes les clés-valeurs de " + dt.Second);
            //loop through all key-value pairs and print them
            foreach (var element in cache)
            {
            }
        }
    }
}
