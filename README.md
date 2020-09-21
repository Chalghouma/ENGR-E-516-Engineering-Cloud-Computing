#Installation
The code needs to be pulled, whether from this sent .rar <br/>
or <br/>
from this github link: https://github.com/Chalghouma/ENGR-E-516-Engineering-Cloud-Computing
<p>
- You can run `git clone https://github.com/Chalghouma/ENGR-E-516-Engineering-Cloud-Computing.git` 
- then search for the latest commit's date that is prior to the deadline
- git checkout <commitId>
</p>

.NET Core 3.1 needs to be installed on the machine. This link for instance references to Ubuntu: https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu

#Generating and running the executables
To generate & run the Server
1. Go to <solutiondir>/Server/
2. Run: `dotnet build --configuration release`
3. cd bin/release/netcoreapp3.1/
4. Run ./Server


To generate & run the Client
1. Go to <solutiondir>/Client/
2. Run: `dotnet build --configuration release`
3. cd bin/release/netcoreapp3.1/
4. Run ./Client

#Architecture
The solution is organized into 4 different projects
1. Memcached.Mimic that holds the core logic for Sockets, how the Client & Server interact,
the parser, the commands, the command executer, the file handler, the benchmarker, etc....
2. Memcached.Mimic.Tests holds the unit tests to make sure the code is consistent and every new line 
does not break the code overall. 
The unit tests cover essentially: 
-	The command parsing
-	The file handling
-	The command execution (through sockets) that itself invokes the file handler
3. The Server project that basically parses & runs the Server class from Memcached.Mimic
4. The Client project that basically parses & runs the Client class from Memcached.Mimic. Includes further details, like the possibility of running multiple clients that set random keys & values with random lengths

