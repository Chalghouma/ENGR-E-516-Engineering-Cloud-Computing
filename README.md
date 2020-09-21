# Installation

The code needs to be pulled, whether from this sent .rar <br/>
or <br/>
from this github link: https://github.com/Chalghouma/ENGR-E-516-Engineering-Cloud-Computing
<p>
- You can run `git clone https://github.com/Chalghouma/ENGR-E-516-Engineering-Cloud-Computing.git` 
- then search for the latest commit's date that is prior to the deadline
- git checkout <commitId>
</p>

.NET Core 3.1 needs to be installed on the machine. This link for instance references to Ubuntu: https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu

# Generating and running the executables
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

# Architecture
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


# Concurrency
The server listens for incoming clients. <br/>
For each new TcpClient being accepted, a new Thread is created to handle the commands sent by the corresponding client. <br/>
The consistency relies when writing/reading on the file. For that, our handler creates an empty object as a field, and uses it
as a lock in order to access the shared file. <br/>
As soon as the operation is finished (whether it be a get, set or delete) (which is basically a Read, Write, Write), we release it.
1000 active threads/clients were active, maintaining a connection with the Server.
The Client app allows to create an N amount of automated threads in order to benchmark on your end.

# Performance
Execution time depends mainly on the size of the accumulated storage file.
For the requests, the commands' key and value lengths can range from 1 to 2^15bytes.
The execution time of a command ranges from 0.017s and goes up to 0.8s (can go either further if the file becomes huge)

#Implemented commands
On the client side, you can run "man" to know the params of the currently implement requests.
For now, we have:
- get <keyName>
- set <keyName> <bytesLength>. Which then asks for another prompt where we insert <key_value>
- delete <keyName> (Which returns DELETED only if the key exists)

# Scenario to check
1. get key1 => Should return VALUE key1 0
2. set key1 3 => abc => Should return STORED
3. get key1 => VALUE key1 3 abc
4. Repeat the set/get. You will always get the value of the last set that has been done. (Since the locking/unlocking done by the FileHandler is in FIFO)
5. delete key1 => DELETED
6. get key1 => Should return VALUE key1 0


# Future Improvements
Enhance the performance of the storage. (Rather than Going through the lines and splitting them by " " to get the values) use another approach.
Implement more customizable fields & commands to be used, especially the stats ones


