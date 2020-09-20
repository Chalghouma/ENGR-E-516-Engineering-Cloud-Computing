using Memcached.Mimic.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Memcached.Mimic.Server
{
    public class ClientConnection
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;

        public ClientConnection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _networkStream = tcpClient.GetStream();

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
                        // Complete the disconnect request.
                        String remoteIP = ((IPEndPoint)handler.RemoteEndPoint).Address.ToString();
                        String remotePort = ((IPEndPoint)handler.RemoteEndPoint).Port.ToString();

                        handler.Close();
                        handler = null;

                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"ExceptionMessage: {exception.Message}\nStackTrace:{exception.StackTrace}");

                }
            }
        }
        private void HandleClientResponse(string clientContent)
        {
            Console.WriteLine($"[Client]: {clientContent}");
            string serverResponse = $"Server has received your message: {clientContent}";
            _networkStream.Write(Encoding.ASCII.GetBytes(serverResponse), 0, serverResponse.Length);
            _networkStream.FlushAsync();
        }
    }
}
