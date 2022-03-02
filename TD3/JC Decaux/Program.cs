using Microsoft.Build.Tasks;
using Nest;
using System;
using System.Text.Json;

namespace TD3 // Note: actual namespace depends on the project name.
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

    internal class JCDecaux
    {
        static readonly HttpClient client = new HttpClient();

        static async void exercice1()
        {
            string api_key = "fd4e518c5cf2c25e0ae2b34e82b286365c05487b";
            HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v1/stations?apiKey={api_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<Root> jsonData = JsonSerializer.Deserialize<List<Root>>(responseBody);

            foreach (Root element in jsonData)
            {
                Console.WriteLine(element.contract_name);

            }
        }

        static async void exercice2(String contract)
        {
            string api_key = "fd4e518c5cf2c25e0ae2b34e82b286365c05487b";
            // On précise ici le nom du contrat dans la requête GET avec l'args[0] de l'appel
            HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v1/stations?contract={contract}&apiKey={api_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<Root> jsonData = JsonSerializer.Deserialize<List<Root>>(responseBody);

            foreach (Root element in jsonData)
            {
                Console.WriteLine(element.name);
            }
        }

        static async void exercice3(String station)
        {
            string api_key = "fd4e518c5cf2c25e0ae2b34e82b286365c05487b";
            HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v1/stations?name={station}&apiKey={api_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<Root> jsonData = JsonSerializer.Deserialize<List<Root>>(responseBody);


            List<Root> stationData = null;
            foreach (Root element in jsonData)
            {
                if (element.name == station) { stationData.Add(element); }
            }

            foreach (Root element in stationData)
            {
                Console.WriteLine(element.name);
            }
        }

        /*static async void exercice4(String contract, double lat, double lng) 
        {
            GeoCoordinate userCurrentPosition = new GeoCoordinate(lat, lng);
            string api_key = "fd4e518c5cf2c25e0ae2b34e82b286365c05487b";
            HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v1/stations?contract={contract}&apiKey={api_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<Root> jsonData = JsonSerializer.Deserialize<List<Root>>(responseBody);
            Root nearestStation = null;
            double distanceMin = 100000;

            foreach (Root element in jsonData)
            {
                // Je n'ai aucune idée de pourquoi la méthode GetDistanceTo n'est pas reconnue et donc pas utilisable
                GeoCoordinate posCurrentStation = new GeoCoordinate(element.position.lat, element.position.lng);
                if (posCurrentStation.GetDistanceTo(userCurrentPosition) < distanceMin)
                {
                    distanceMin = posCurrentStation.GetDistanceTo(userCurrentPosition);
                    nearestStation = element;
                }
            }
            if (nearestStation != null) {
                Console.WriteLine($"The nearest station to the position ({lat},{lng}) is {nearestStation.name} at ({nearestStation.position.lat},{nearestStation.position.lng}) situated {(distanceMin)}m from your current position.");
            }
            else {Console.WriteLine($"No station were found near your current position ({lat},{lng})");}                   
        }*/

        static async Task Main(string[] args)
        {
            /*
            * QUESTION 1 : Créez un programme qui ne prend pas d'argument et renvoie la liste des contrats disponibles de JCDecaux.
            */
            exercice1();

            /*
             * QUESTION 2 : Créez un programme qui, étant donné un contrat comme paramètre, renvoie la liste des stations correspondantes.
             */
            exercice2("toulouse");

            /*
             * QUESTION 3 : Créez un programme qui, étant donné un contrat comme paramètre, renvoie la liste des stations correspondantes.
             */
            //exercice3("00055 - SAINT-SERNIN - G. ARNOULT");

            /*
            * QUESTION 4 : Créez un programme qui prend en argument un contrat et une position GPS, et trouve la station la plus proche.
            */
            //exercice4("toulouse",43.6089519605732, 1.4410035987314);
        }
    }
}
