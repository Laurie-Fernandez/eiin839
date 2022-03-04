// See https://aka.ms/new-console-template for more information

public class Program
{
    public async static Task Main()
    {
        HttpClient client = new HttpClient();
        String url = "http://localhost:8080/exercice3/multiply?param1=2&param2=12";
        var response = client.GetAsync(url);
        String responseBody = await response.Result.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }
}
