﻿using Memcached.Mimic.Commands;
using Memcached.Mimic.Common;
using Memcached.Mimic.Parser;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Memcached.Mimic.Server
{
    public class ClientConnection
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private Func<ICommand, Task<ExecutionResult<string[]>>> _onCommandRequested;

        public ClientConnection(TcpClient tcpClient, Func<ICommand, Task<ExecutionResult<string[]>>> onCommandRequested)
        {
            _tcpClient = tcpClient;
            _networkStream = tcpClient.GetStream();
            _onCommandRequested = onCommandRequested;

            StateObject state = new StateObject();
            state.workSocket = _tcpClient.Client;

            _tcpClient.Client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(OnReceive), state);
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
                        HandleClientResponse(content);


                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(OnReceive), state);

                    }
                }

                catch (SocketException socketException)
                {
                    //WSAECONNRESET, the other side closed impolitely
                    if (socketException.ErrorCode == 10054 || ((socketException.ErrorCode != 10004) && (socketException.ErrorCode != 10053)))
                    {
                        Console.WriteLine("Client Disconnected");
                        // Complete the disconnect request.
                        String remoteIP = ((IPEndPoint)handler.RemoteEndPoint).Address.ToString();
                        String remotePort = ((IPEndPoint)handler.RemoteEndPoint).Port.ToString();

                        handler.Close();
                        handler = null;

                        Console.WriteLine("Closing Socket & killing thread");
                        Thread.CurrentThread.Interrupt();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"ExceptionMessage: {exception.Message}\nStackTrace:{exception.StackTrace}");

                }
            }
        }
        private async void HandleClientResponse(string clientContent)
        {
            ICommand receivedCommand = CommandParser.ParseFromSentData(clientContent);
            if (receivedCommand == null) Console.WriteLine("Received Command is null");
            Console.WriteLine($"[Client.Command]: {receivedCommand?.GetStringForEncoding()}");
            string serverResponse = $"Server has received your message: {clientContent}";
            ExecutionResult<string[]> executionResult = await this._onCommandRequested(receivedCommand);
            if(executionResult.ExecutionTimeInMS>=0)
            {
                string output = $"[Server]: Executed in {executionResult.ExecutionTimeInMS/1000}s\r\n";
                _networkStream.Write(Encoding.ASCII.GetBytes(output), 0, output.Length);
            }
            foreach (var result in executionResult.Result)
            {
                string output = $"[Server]: {result}\r\n";
                _networkStream.Write(Encoding.ASCII.GetBytes(output), 0, output.Length);
            }
            await _networkStream.FlushAsync();
        }
    }
}
