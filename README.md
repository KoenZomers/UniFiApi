# UniFi API in C#

API in C# which can be used to read data from an on premises UniFi Controller installation. Includes Unit Tests and a sample ConsoleApp to test the API. All assemblies are signed and compiled against .NET 4.6.2.

## Usage

```C#
// Create a new Api instance to connect with the UniFi Controller
using (var uniFiApi = new Library.Api(new Uri("https://192.168.0.1:8443")))
{
    // Disable SSL validation as UniFi uses a self signed certificate
    uniFiApi.DisableSslValidation();

    // Authenticate to UniFi
    await uniFiApi.Authenticate("admin", "password");

    // Retrieve the access points
    var accessPoints = await uniFiApi.GetAccessPoints();

    foreach (var accessPoint in accessPoints)
    {
        Console.WriteLine($"  - {accessPoint.Name} (MAC {accessPoint.MacAddress})");
    }

    // Retrieve the active clients
    var activeClients = await uniFiApi.GetActiveClients();

    foreach (var activeClient in activeClients)
    {
        Console.WriteLine($"  - {activeClient.FriendlyName} (MAC {activeClient.MacAddress}, Channel {activeClient.Channel})");
    }
}
```

## NuGet

Also available as NuGet Package: [KoenZomers.UniFi.Api](https://www.nuget.org/packages/KoenZomers.UniFi.Api/)

## Version History

Version 1.0.0.0 - March 28, 2017

Initial version. Allows requesting all active clients and their details and the basics about the access points.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
mail@koenzomers.nl