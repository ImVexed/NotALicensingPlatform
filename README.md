# Not a Licensing Platform (spoiler alert: it actually is)
Just a liteweight licensing system using [NotLiteCode](https://github.com/Icemantheditor/NotLiteCode)
 
## The Flow
 * Connect to server
 * Handshake, start encrypting
 * Request the actual program (NaLP - Login)
 * Assembly.Load the actual program and display it to the end user
 * Actual program calls login function on server
 * Actualy program then attempts to call a secure function that will change depending on if the client sucessfully logged in
 
## Planned Features:
 - All features currently satisfied...

## Notes:
 - If you want to hide the console of (NotALicensingPlatform), simple go into project settings and change the Output Type to Windows Application instead of Console Application.
 - NotALicensingPlatform and NaLP - Login NEED to be the same .NET Version, or any Assembly you're attempting to load for that matter.
 
