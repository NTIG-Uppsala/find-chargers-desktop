using System.Runtime.InteropServices;
using System;


class mainClass
{
const string version = "0.001.a";
    static async Task Main(string[] args)
    {
        // Display the number of command line arguments.
        if(args.Length > 0){
            checkArgs(args);
        }
        httpHandler httpHandler;
        await httpHandler.MakeGetRequest("http://find-chargers.azurewebsites.net/get-charger");

    }
    public static void checkArgs(string[] args_in){
        string[] validArguments = {"--info","--version"};

        if(args_in.Length > 1) {
            Console.WriteLine("Only one argument is supported. Valid arduments are: {0}", string.Join(", ", validArguments));
            return;
        }

        if(args_in[0] == "--info"){
            Console.WriteLine("This desktop application is developed by NTI Uppsala. Its purpose is to det information from a api and display it for the user.");
            return;
        }

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

class httpHandler {
// HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
    static readonly HttpClient client = new HttpClient();

    public static async Task MakeGetRequest(string url)
    {
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try	
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);


            Console.WriteLine(responseBody);
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");	
            Console.WriteLine("Message :{0} ",e.Message);
        }
    }

}

