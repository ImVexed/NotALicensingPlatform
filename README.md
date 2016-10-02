# Not a Licensing Platform (spoiler alert: it actually is)
Just a liteweight licensing system using [NotLiteCode](https://github.com/Icemantheditor/NotLiteCode)

## Features
 * All perks of NLC, automatic Encryption, Handshakes, Compression
 * CMS with subscription based keys
 * Basic GUI + Ability to Kick and Ban connected clients (Right click)
 * Async (ﾉ◕ヮ◕)ﾉ✧･ﾟ*
 
## The Flow
 * Connect to server
 * Handshake, start encrypting
 * Request the actual program (NaLP - Login)
 * Assembly.Load the actual program and display it to the end user
 * Actual program calls login (or register) function on server
 * Actual program then attempts to call a secure function that will change depending on if the client sucessfully logged in
 
## Planned Features:
 - All features currently satisfied...

## Notes:
 - If you want to hide the console of (NotALicensingPlatform), simple go into project settings and change the Output Type to Windows Application instead of Console Application.
 - NotALicensingPlatform and NaLP - Login NEED to be the same .NET Version, or any Assembly you're attempting to load for that matter.
 - I tried to leave everything fairly basic here, my goal wasn't to go the whole 9 yards, as everyone has a different implementation, thats why the client system is basic XML, as it'll be easy to re-implement whatever method you preffer, this same thinking was applied to the Keys (which most likely will be automatically generated), BannedIPs, BannedHWIDs, and other things of this nature.
 
