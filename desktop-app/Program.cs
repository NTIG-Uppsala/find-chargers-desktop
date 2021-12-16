using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json.Linq;


class mainClass
{
const string version = "0.200.a";
    static async Task Main(string[] args)
    {
        // If args provided check args 
        if(args.Length > 0){
            checkArgs(args);
        }
        await HttpHandler.MakeGetRequest("http://find-chargers.azurewebsites.net/get-charger");

    }
    // Args handling logic
    public static void checkArgs(string[] args_in){
        string[] validArguments = {"--info","--version"};

        // More then one argument
        if(args_in.Length > 1) {
            Console.WriteLine("Only one argument is supported. Valid arduments are: {0}", string.Join(", ", validArguments));
            return;
        }

        // Argument is --info
        if(args_in[0] == "--info"){
            Console.WriteLine("This desktop application is developed by NTI Uppsala. Its purpose is to det information from a api and display it for the user.");
            return;
        }
        
        // Argument is --version
        if(args_in[0] == "--version"){

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
            Console.WriteLine("{0} running on Linux!",  version);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("{0} running on Windows",  version);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Console.WriteLine("{0} running on MacOS!",  version);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                Console.WriteLine("{0} running on freeBSD!",  version);
            }
            
            Console.WriteLine("OS version {0}", Environment.OSVersion.Version);
            return;
        }

        Console.WriteLine("{0} is not a valid argument. Valid arduments are: {1}", args_in[0] ,string.Join(", ", validArguments));

    }
}

class HttpHandler {
    static readonly HttpClient client = new HttpClient();

    public static async Task MakeGetRequest(string url)
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
                }
                
            }

        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");	
            Console.WriteLine("Message :{0} ",e.Message);
        }
    }

}

