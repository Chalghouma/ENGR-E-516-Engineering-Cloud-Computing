using Memcached.Mimic.Commands;
using Memcached.Mimic.FileHandler;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Memcached.Mimic.Server
{
    public class Server
    {
        private Thread _listenerGeneratorThread;
        private int _portNumber;
        private string _ipAddress;
        private ICommandExecuter _commandExecuter;
        public IFileHandler FileHandler { get; private set; }
        public Server(string ipAddress, int portNumber)
        {
            _ipAddress = ipAddress;
            _portNumber = portNumber;
            FileHandler = new TextFileHandler();
            _commandExecuter = new LocalFileCommandExecuter(FileHandler);
        }
        public void Start()
        {
            _listenerGeneratorThread = new Thread(new ThreadStart(ListenForClients));
            _listenerGeneratorThread.Start();
        }
        public void ListenForClients()
        {
            IPAddress localAddr = IPAddress.Parse(_ipAddress);

            TcpListener tcpServer = new TcpListener(localAddr, _portNumber);
            tcpServer.Start();

            Console.WriteLine($"Server started on port: {_portNumber}");

            while (true)
            {
                Thread clientConnectionThread = new Thread(new ParameterizedThreadStart(OnNewClientAccepted));
                TcpClient tcpClient = tcpServer.AcceptTcpClient();
                clientConnectionThread.Start(new ClientConnectionSetup
                {
                    TcpClient = tcpClient,
                    OnCommandRequested = this._commandExecuter.ExecuteCommand
                });

            }
        }
        private void OnNewClientAccepted(Object param)
        {
            Console.WriteLine("New Incoming Connection");
            ClientConnectionSetup clientConnectionSetup = param as ClientConnectionSetup;
            new ClientConnection(clientConnectionSetup.TcpClient, clientConnectionSetup.OnCommandRequested);
        }
    }
    public class ClientConnectionSetup
    {
        public TcpClient TcpClient { get; set; }
        public Func<ICommand, Task<ExecutionResult>> OnCommandRequested { get; set; }
    }
}
