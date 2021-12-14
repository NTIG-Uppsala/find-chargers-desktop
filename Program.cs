using System.Runtime.InteropServices;

Console.WriteLine($"Hello {System.Environment.GetEnvironmentVariable("USER")}");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
 {
  Console.WriteLine("We're on Linux!");
 }

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
 {
  Console.WriteLine("We're on Windows!");
 }

 if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
 {
  Console.WriteLine("We're on Mac!");
 }

  if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
 {
  Console.WriteLine("We're on freeBSD!");
 }

Console.WriteLine("Version {0}", Environment.OSVersion.Version);