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

        [Option("lat", Required = false, HelpText = "latitude to send to api")]
        public float? latitude { get; set; }

        [Option("long", Required = false, HelpText = "longitude to send to api")]
        public float? longitude { get; set; }

        [Option('r', "range", Required = false, HelpText = "What range from cordinates to return charger (in meters)")]
        public int? range { get; set; }

        [Option('v', "version", Required = false, HelpText = "Gets current version")]
        public bool version { get; set; }
    }
    const string programVer = "0.300.a";
    static async Task Main(string[] args)
    {
        // If args provided check args 
        if(args.Length > 0){
            await checkArgs(args);
        }
        else{
            // Gets all chargers if nothing is specified
            await HttpHandler.MakeGetRequest("http://find-chargers.azurewebsites.net/get-charger");
        }

    }
    // Args handling logic
    public static async Task checkArgs(string[] args_in){
        try{
            var parser = new Parser((with) => {with.AutoVersion = false; with.HelpWriter = Console.Out; with.AutoHelp = true;});
            var parserResult = parser.ParseArguments<Options>(args_in);

            parserResult.WithParsed<Options>(o =>
                {
                    if(o.version == true){

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                        Console.WriteLine("{0} running on Linux!",  programVer);
                        }

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            Console.WriteLine("{0} running on Windows",  programVer);
                        }

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            Console.WriteLine("{0} running on Darwin!",  programVer);
                        }

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                        {
                            Console.WriteLine("{0} running on freeBSD!",  programVer);
                        }

                        Console.WriteLine("OS version {0}", Environment.OSVersion.Version);
                        return; // Dont continue
                    }

                    // Kan vara snyggt att göra detta till en switch case
                    if (o.email != null && o.latitude == null && o.longitude == null && o.range == null)
                    {
                        string urlFormatted = string.Format($"http://find-chargers.azurewebsites.net/get-charger-by-email/{o.email}");
                        var Task = HttpHandler.MakeGetRequest(urlFormatted);
                        Task.Wait(); // Wait for task to be done
                    }
                    else if(o.latitude != null && o.longitude != null && o.range != null && o.email == null) {
                        string urlFormatted = string.Format($"http://find-chargers.azurewebsites.net/get-chargers-in-range/{o.latitude}/{o.longitude}/{o.range}");
                        Console.WriteLine(urlFormatted);
                        var Task = HttpHandler.MakeGetRequest(urlFormatted);
                        Task.Wait();
                    }
                    else if((o.latitude == null || o.longitude == null || o.range == null) && o.email == null) {
                        Console.WriteLine("To get a charger by cordinates and range you must include the following: --lat {number} --long {number} --range {number}");
                    }
                    else if((o.latitude != null || o.longitude != null || o.range != null) && o.email != null) {
                        Console.WriteLine("When using --email you must only include --email in tne command.");
                    }


                });
             } catch (Exception e){

                Console.WriteLine(e.Message);
        }   
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
            Console.WriteLine(responseBody);
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

