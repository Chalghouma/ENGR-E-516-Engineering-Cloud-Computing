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
