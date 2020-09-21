using Memcached.Mimic.Commands;
using Memcached.Mimic.Common;
using Memcached.Mimic.Parser;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Memcached.Mimic.Client
{
    public class Client
    {
        private string _ipAddress;
        private int _portNumber;
        private NetworkStream _networkStream;
        public Client(string ipAddress, int portNumber, bool waitForUserInput = true)
        {
            _ipAddress = ipAddress;
            _portNumber = portNumber;

            TcpClient server = new TcpClient(_ipAddress, _portNumber);
            Console.WriteLine($"Successfully Connected To Server on port {_portNumber}");


            StateObject state = new StateObject();
            state.workSocket = server.Client;
            server.Client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(OnReceive), state);

            _networkStream = server.GetStream();
            if (waitForUserInput)
                WaitForUserInput();
        }
        private void WaitForUserInput()
        {
            ShowMan();
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "man")
                    ShowMan();
                else
                {
                    ICommand command = CommandParser.ParseFromUserInput(input, true);
                    if (command == null) Console.WriteLine("Couldn't parse your request");
                    else
                    {
                        SendCommand(command);
                    }
                }
            }
        }

        private void ShowMan()
        {
            Console.WriteLine("************************");
            Console.WriteLine("Available Commands :");
            Console.WriteLine(" - get <keyName>");
            Console.WriteLine(" - set <keyName> <keySize (Required as it's parsed but doesn't really bring value)>");
            Console.WriteLine("     - <value of key from previous set command>");
            Console.WriteLine(" - delete <keyName>");
            Console.WriteLine("Write man to show this manual");
        }

        public void SendCommand(ICommand command)
        {
            string commandStringData = command.GetStringForEncoding();
            _networkStream.Write(Encoding.ASCII.GetBytes(commandStringData), 0, commandStringData.Length);
            _networkStream.Flush();
        }

        public void OnReceive(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead;

            if (handler.Connected)
            {
                try
                {
                    bytesRead = handler.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        // There  might be more data, so store the data received so far.
                        state.sb.Remove(0, state.sb.Length);
                        state.sb.Append(Encoding.ASCII.GetString(
                                         state.buffer, 0, bytesRead));

                        content = state.sb.ToString();
                        Console.Write($"{content}");

                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(OnReceive), state);

                    }
                }

                catch (SocketException socketException)
                {
                    //WSAECONNRESET, the other side closed impolitely
                    if (socketException.ErrorCode == 10054 || ((socketException.ErrorCode != 10004) && (socketException.ErrorCode != 10053)))
                    {
                        handler.Close();
                    }
                }

                catch (Exception exception)
                {
                    Console.WriteLine($"ExceptionMessage: {exception.Message}\nStackTrace:{exception.StackTrace}");
                }
            }
        }

    }
}
