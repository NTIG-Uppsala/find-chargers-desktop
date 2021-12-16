using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json.Linq;
using CommandLine;



class MainClass
{

    public class Options
    {
        [Option('e', "email", Required = false, HelpText = "Email to send to api")]
        public string? email { get; set; }
    }
    const string version = "0.300.a";
    static async Task Main(string[] args)
    {

        // If args provided check args 
        if(args.Length > 0){
            await checkArgs(args);
        }
        else{
            await HttpHandler.MakeGetRequest("http://find-chargers.azurewebsites.net/get-charger");
        }

    }
    // Args handling logic
    public static async Task checkArgs(string[] args_in){

        Parser.Default.ParseArguments<Options>(args_in)
            .WithParsed<Options>(o =>
            {
                if (o.email != "")
                {
                    string urlFormatted = string.Format($"http://find-chargers.azurewebsites.net/get-charger-by-email/{o.email}");
                    var Task = HttpHandler.MakeGetRequest(urlFormatted);
                    Task.Wait(); // Wait for task to be done
                }

            });
    }
}

class HttpHandler {
    static readonly HttpClient client = new HttpClient();

    public static async Task<bool> MakeGetRequest(string url)
    {
        try	
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var dataResponse = JArray.Parse(responseBody).ToObject<List<JObject>>();

            // If objs is null dont continue
            if(dataResponse == null) throw new HttpRequestException();
            foreach(var json in dataResponse){

                string chargerType = string.Format(" AC_1: {0}, AC_2: {1}, Chademo: {2}, CCS: {3} ", json["ac_1"], json["ac_2"], json["chademo"], json["ccs"]);
                try{
                    Console.WriteLine("-|||- ID: {0} ||| ADRESS: {1} ||| {2} |||  IS_VISIBLE: {3}  ||| EMAIL: {4} |||", json["id"].ToString().PadRight(5), json["address"].ToString().PadRight(20), chargerType.PadRight(20), json["is_visible"], json["email_address"].ToString().PadRight(40));
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                } catch (Exception e) {
                    Console.WriteLine("Error formatting data: {0}", e.Message);
                    return false;
                }
                
            }

        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");	
            Console.WriteLine("Message :{0} ",e.Message);
            return false;
        }
        return true;
    }

}

