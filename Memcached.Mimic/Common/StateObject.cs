﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Memcached.Mimic.Common
{
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024*100;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }
}
