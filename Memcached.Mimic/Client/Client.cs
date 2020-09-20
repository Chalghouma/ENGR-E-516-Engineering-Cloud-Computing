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
        public Client(string ipAddress, int portNumber)
        {
            _ipAddress = ipAddress;
            _portNumber = portNumber;

            TcpClient server = new TcpClient(_ipAddress, _portNumber);
            Console.WriteLine("Successfully Connected To Server");


            StateObject state = new StateObject();
            state.workSocket = server.Client;
            server.Client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(OnReceive), state);

            WaitForUserInput(server.GetStream());
        }
        private void WaitForUserInput(NetworkStream stream)
        {
            Console.WriteLine("Insert your command");
            while (true)
            {
                string input = Console.ReadLine();
                ICommand command = CommandParser.Parse(input);
                if (command != null && command is GetCommand)
                {
                    string commandStringData = command.GetStringForEncoding();
                    stream.Write(Encoding.ASCII.GetBytes(commandStringData), 0, commandStringData.Length);
                    stream.Flush();
                }

            }
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
                        Console.WriteLine($"[Server]: {content}");

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
