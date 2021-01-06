# UniFi API in C#

[![licence badge]][licence]
[![issues badge]][issues]
[![prwelcome badge]][prwelcome]

[licence badge]:https://img.shields.io/badge/license-Apache2-blue.svg
[issues badge]:https://img.shields.io/github/issues/koenzomers/UniFiApi.svg
[prwelcome badge]:https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square

[licence]:https://github.com/koenzomers/UniFiApi/blob/master/LICENSE.md
[issues]:https://github.com/koenzomers/UniFiApi/issues
[prwelcome]:http://makeapullrequest.com

API in C# which can be used to read data from an on premises UniFi Controller installation. Includes Unit Tests and a sample ConsoleApp and Windows Forms application to test the API. All assemblies are signed and compiled against .NET Standard 2.0, .NET Framework 4.5.2, .NET Framework 4.7.2 and .NET Core 2.0. This library in its current state functions mainly as a starting point / sample of how to communicate with the UniFi service. Fork it and extend it with the functionality you need. Or if you're unable to add this yourself, submit an issue on GitHub describing what you need and I'll have a look at it when I get a chance.

It is sufficient to use an account with the "Read Only" role in UniFi unless you want to modify things like using BlockClient, UnblockClient, AuthorizeGuest or UnauthorizeGuest.

## Addons in this Fork

Version 1.1.8.0 - January 6, 2021

- Added ability to get a list of wireless networks 
- Added ability to get a list of configured networks 

## Usage

```C#
// Create a new Api instance to connect with the UniFi Controller
var uniFiApi = new KoenZomers.Tools.UniFi.Api.Api(new Uri("https://192.168.0.1:8443"));

// Disable SSL validation as UniFi uses a self signed certificate by default
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

// Authorize a certain guest client to access the UniFi network
await uniFiApi.AuthorizeGuest("a0:23:f3:14:c2:fa");

// Revoke the authorization for a certain guest client to access the UniFi network
await uniFiApi.UnauthorizeGuest("a0:23:f3:14:c2:fa");
```

## NuGet

Also available as NuGet Package: [KoenZomers.UniFi.Api](https://www.nuget.org/packages/KoenZomers.UniFi.Api/)

## Version History

Version 1.1.8.0 - November 16, 2020

- Added `RenameClient` option to change the friendly name of a client as proposed through [issue 16](https://github.com/KoenZomers/UniFiApi/issues/16)

Version 1.1.7.0 - July 23, 2019

- Removed method GetAccessPoints which was deprecated. Use GetDevices instead.
- Marked AccessPoint class as obsolete. Use Device class instead as any UniFi device could be returned so it suits the purpose better.
- Added a few more properties to the Device.cs class. There's still a ton more properties being returned by UniFi devices. Feel free to fork/add/PR them.

Version 1.1.6.0 - July 21, 2019

- Added some more property documentation to Client
- Fixed the following properties in a Client to represent correct values and switched them from using a TimeSpan to a DateTime:
  - LatestAssociationTime
  - LatestAssociationTimeRaw
  - FirstSeen
  - FirstSeenRaw
  - LastSeen
  - LastSeenRaw
  - LastSeenByUap
  - LastSeenByUapRaw
  - UptimeByUap
  - UptimeByUapRaw
  - AssociatedTime
  - AssociatedTimeRaw
  - IdleTime
  - IdleTimeRaw
  - Uptime
  - UptimeRaw
- Getting ToString on a Client instance will now not only return just the FriendlyName but if no Friendly Name has been set, it will return the hostname
- The property TotalConnectedTime in ClientSession has been renamed to SessionStartedAtRaw as it didn't actually indicate the total connected time but the amount of seconds since January 1, 1970 when the client started the session. Added a property SessionStartedAt to get the DateTime version of it.
- Added property SessionEndedAt to ClientSession which calculates the SessionStart + SessionDuration. UniFi does not provide information on if a session is still active, so if the outcome of SessionEndedAt is close to the current date and time, you can consider the session to still be active.

Version 1.1.5.0 - June 30, 2019

- If credentials are wrong, it will nicely handle it now by returning a false instead of throwing a WebException

Version 1.1.4.0 - May 8, 2019

- Various performance and memory optimizations
- Fixed issue where reconnecting wouldn't work or closing the application would cause a blocking thread [issue 10](https://github.com/KoenZomers/UniFiApi/issues/10)
- Added option to sample Windows Application to request all clients known to UniFi

Version 1.1.3.0 - May 6, 2019

- Fix in the dispose of the API to avoid it getting stuck on disposal of the API instance
- Added Windows Forms sample application to the solution which utilizes the UniFi API and allows for blocking and unblocking of a client and authorizing and revoking authorization of a guest
- Added support for multi targeting frameworks which supports .NET Standard 2.0, .NET Framework 4.5.2, .NET Framework 4.7.2 and .NET Core 2.0
- Downgraded Newtonsoft JSON version requirement to be 11.0.1 and higher for .NET Core and .NET Standard and 8.0.1 for .NET Framework to allow for a bit greater compatibility

Version 1.1.2.0 - April 4, 2019

- Added option to retrieve the historical connection information of a specific client via GetClientHistory(clientMacAddress)

Version 1.1.1.0 - April 3, 2019

- Added option to retrieve the sites registered with UniFi via GetSites()

Version 1.1.0.0 - January 15, 2019

- Renamed Library to Api and based it on .NET Standard instead of the .NET Framework so you can use it cross platform now
- Small bugfix in the ConsoleApp only working with the default UniFi site
- Updated .gitignore to exclude any App.config file in the solution. Copy over the App.sample.config to App.config yourself and fill it with the proper settings.
- Included documentation XML file so the inline code comments will be visible in your own project referencing this assembly
- Added AuthorizeGuest and UnauthorizeGuest guest methods to allow for someone connecting to a guest network to become authorized to access the network or revoke its access. The Unit Test AuthorizeUnAuthorizeGuestTestMethod shows how it can be used.

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
