# UniFi API in C#

API in C# which can be used to read data from an on premises UniFi Controller installation. Includes Unit Tests and a sample ConsoleApp to test the API. All assemblies are signed and compiled against .NET 4.6.2. This library in its current state functions mainly as a starting point / sample of how to communicate with the UniFi service. Fork it and extend it with the functionality you need. Or if you're unable to add this yourself, submit an issue on GitHub describing what you need and I'll have a look at it when I get a chance.

It is sufficient to use an account with the "Read Only" role in UniFi unless you want to modify things like using BlockClient or UnblockClient.

## Usage

```C#
// Create a new Api instance to connect with the UniFi Controller
using (var uniFiApi = new KoenZomers.Tools.UniFi.Api(new Uri("https://192.168.0.1:8443")))
{
    // Disable SSL validation as UniFi uses a self signed certificate
    uniFiApi.DisableSslValidation();

    // Authenticate to UniFi
    await uniFiApi.Authenticate("admin", "password");

    // Retrieve the UniFi devices
    var devices = await uniFiApi.GetDevices();

    foreach (var device in devices)
    {
        Console.WriteLine($"  - {device.Name} (MAC {device.MacAddress})");
    }

    // Retrieve the active clients
    var activeClients = await uniFiApi.GetActiveClients();

    foreach (var activeClient in activeClients)
    {
        Console.WriteLine($"  - {activeClient.FriendlyName} (MAC {activeClient.MacAddress}, Channel {activeClient.Channel})");
    }

	// Block a certain client from accessing the UniFi network
	await uniFiApi.BlockClient("a0:23:f3:14:c2:fa");
	
	// Unblock a certain client from accessing the UniFi network
	await uniFiApi.UnblockClient("a0:23:f3:14:c2:fa");
}
```

## NuGet

Also available as NuGet Package: [KoenZomers.UniFi.Api](https://www.nuget.org/packages/KoenZomers.UniFi.Api/)

## Version History

Version 1.0.2.0 - September 26, 2018

- Added ability to provide a site ID to communicate with. This only applies if you have more than one site in UniFi. In this case use the new constructor ```public Api(Uri baseUri, string siteId)``` to provide the site identifier. Based on request from [issue 4](https://github.com/KoenZomers/UniFiApi/issues/4).

Version 1.0.1.1 - September 15, 2018

- Added method EnableTls11and12() to enable TLS 1.1 and 1.2 as valid protocols when your UniFi service is using an SSL certificate signed with one of these protocols. Thanks to Ashley Gregory for pointing this out!

Version 1.0.1.0 - September 14, 2018

- Added some inline code comments to the Clients model. Still needs more work to get all properties provided with comments.
- Added PostRequest to the HttpUtility which can be used to request a change to UniFi
- Added GetAllCookies to the HttpUtility which allows extracting the cookies from the cookie container. This is required as UniFi demands the cross site request forgery token stored in the cookie to be passed through the custom X-Csrf-Token header on every HTTP Post.
- Added the XML documentation to the NuGet package so you will see the inline code comments in your own project as well
- Added method GetDevices which returns all UniFi devices
- Marked GetAccessPoints as obsolete as it didn't only retrieve the UniFi access points, but all UniFi devices so the naming was confusing
- Added method BlockClient which allows blocking a client from getting access to the UniFi network and UnblockClient to unblock a client. This sample can nicely be used as a sample of how to modify properties in UniFi.
- Added inline code comments to the Unit Tests to explain what each of them is testing for
- Added GetAllClients method which returns all clients known to UniFi, regardless if they're currently connected

Version 1.0.0.1 - March 28, 2017

- Changed assembly name of Library, added ToString to Client

Version 1.0.0.0 - March 28, 2017

- Initial version. Allows requesting all active clients and their details and the basics about the access points.

## Feedback

Comments\suggestions\bug reports are welcome!

Koen Zomers
koen@zomers.eu
