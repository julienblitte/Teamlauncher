using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamlauncher
{
    // Delegate for passing received message back to caller
    public delegate void DelegateMessage(string Reply);

    class PipeServer
    {
        //public event Action<String> PipeMessage;
        public Action<String> PipeMessage;

        string _pipeName;

        public void Listen(string PipeName)
        {
            try
            {
                // Set to class level var so we can re-use in the async callback method
                _pipeName = PipeName;
                // Create the new async pipe 
                NamedPipeServerStream pipeServer = new NamedPipeServerStream(PipeName,
                   PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                // Wait for a connection
                pipeServer.BeginWaitForConnection
                (new AsyncCallback(WaitForConnectionCallBack), pipeServer);
            }
            catch (Exception oEX)
            {
                Debug.WriteLine("PipeServer.Listen:" + oEX.Message);
            }
        }

        private void WaitForConnectionCallBack(IAsyncResult iar)
        {
            try
            {
                // Get the pipe
                NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState;
                // End waiting for the connection
                pipeServer.EndWaitForConnection(iar);

                byte[] buffer = new byte[255];

                // Read the incoming message
                pipeServer.Read(buffer, 0, 255);

                // Convert byte buffer to string
                string stringData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(stringData + Environment.NewLine);

                // Pass message back to calling form
                PipeMessage.Invoke(stringData);

                // Kill original sever and create new wait server
                pipeServer.Close();
                pipeServer.Dispose();
/*
                pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.In,
                   1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                // Recursively wait for the connection again and again....
                pipeServer.BeginWaitForConnection(
                   new AsyncCallback(WaitForConnectionCallBack), pipeServer);
*/
            }
            catch
            {
                return;
            }
        }
    }

    class PipeClient
    {
        public void Send(string SendStr, string PipeName, int TimeOut = 1000)
        {
            try
            {
                NamedPipeClientStream pipeStream = new NamedPipeClientStream
                   (".", PipeName, PipeDirection.Out, PipeOptions.Asynchronous);

                // The connect function will indefinitely wait for the pipe to become available
                // If that is not acceptable specify a maximum waiting time (in ms)
                pipeStream.Connect(TimeOut);
                Debug.WriteLine("[Client] Pipe connection established");

                byte[] _buffer = Encoding.UTF8.GetBytes(SendStr);
                pipeStream.BeginWrite
                (_buffer, 0, _buffer.Length, new AsyncCallback(AsyncSend), pipeStream);
            }
            catch (TimeoutException oEX)
            {
                Debug.WriteLine("PipeClient.Send:"+oEX.Message);
            }
        }

        private void AsyncSend(IAsyncResult iar)
        {
            try
            {
                // Get the pipe
                NamedPipeClientStream pipeStream = (NamedPipeClientStream)iar.AsyncState;

                // End the write
                pipeStream.EndWrite(iar);
                pipeStream.Flush();
                pipeStream.Close();
                pipeStream.Dispose();
            }
            catch (Exception oEX)
            {
                Debug.WriteLine("PipeClient.AsyncSend:" + oEX.Message);
            }
        }
    }
}
