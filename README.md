# NotALicensingPlatform (NaLP)
A sample C# licensing server & client implementation using [NotLiteCode](https://github.com/ImVexed/NotLiteCode) RPC and Docker.

## Features
 - Automatic SSL
 - Key based subscription model
 - SQLite support (with Mongo & friends soon to come)
 - Docker support
 
 ## Try
 
 ```bash
 git clone https://github.com/ImVexed/NotALicensingPlatform
 cd NotALicensingPlatform/Server/
 docker build -t nalp .
 docker run -d -p 1337:1337 --name nalp nalp
 docker exec nalp nalp --genkey "6 months"
 ```
 
 Start the Client and use the key outputted from the `docker exec` to register an account.
 
 Most server logic can be found [here.](https://github.com/ImVexed/NotALicensingPlatform/blob/master/Server/NLC/SharedClass.cs)
