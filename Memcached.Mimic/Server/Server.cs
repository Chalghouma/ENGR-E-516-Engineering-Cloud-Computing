using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Memcached.Mimic.Server
{
    public class Server
    {
        private Thread _listenerGeneratorThread;
        private int _portNumber;
        private string _ipAddress;
        
        public Server(string ipAddress, int portNumber)
        {
            _ipAddress = ipAddress;
            _portNumber = portNumber;
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

            while (true)
            {
                Thread clientConnectionThread = new Thread(new ParameterizedThreadStart(OnNewClientAccepted));
                TcpClient tcpClient = tcpServer.AcceptTcpClient();
                clientConnectionThread.Start(tcpClient);

            }
        }
        private void OnNewClientAccepted(Object newTcpClient)
        {
            new ClientConnection(newTcpClient as TcpClient);
        }
    }
}
