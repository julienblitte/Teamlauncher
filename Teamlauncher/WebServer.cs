using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teamlauncher
{
    class WebServer
    {
        private TcpListener server;
        private byte[] buffer;
        private string database;
        private Thread thread;

        public WebServer(IPAddress ip, int port, string file)
        {
            server = new TcpListener(ip, port);
            database = file;
            thread = null;
        }

        public void start()
        {
            if (thread == null)
            {
                thread = new Thread(listen);
                thread.Start();
            }
        }

        public void stop()
        {
            if (thread != null)
            {
                server.Stop();
                thread.Abort();
                thread = null;
            }
        }

        protected void listen()
        {
            server.Start();

            try
            { 
                buffer = new Byte[1024];

                // Enter the listening loop.
                while (true)
                {
                    TcpClient client;
                    NetworkStream stream;
                    string request;
                    int i;

                    client = server.AcceptTcpClient();

                    stream = client.GetStream();
                    request = "";
                    while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        request = Encoding.UTF8.GetString(buffer, 0, i);
                        if (endsWithTwoCRLF(request))
                        {
                            break;
                        }
                    }

                    if (request.Contains("GET "))
                    {
                        replyContent(stream, database);
                    }
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        protected bool endsWithTwoCRLF(string content)
        {
            int l;
            int i;

            l = content.Length;
            if (l < 4)
                return false;

            for (i = l - 1; i < l - 4; i--)
            {
                if ((content[i] != '\r') && (content[i] != '\n'))
                    return false;
            }
            return true;
        }

        protected void replyContent(NetworkStream outStream, string filename)
        {
            DateTime modified, now;
            string httpHeader;
            byte[] httpHeaderBuffer;

            modified = File.GetLastWriteTime(filename);
            now = DateTime.UtcNow;
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            using (FileStream inStream = File.Open(filename, FileMode.Open))
            {
                httpHeader = "HTTP/1.0 200 OK\r\n" +
                    "Date: " + now.ToString("r") + "\r\n" +
                    "Server: Teamlauncher/"+ versionInfo.FileMajorPart + "." + versionInfo.FileMinorPart + "\r\n" +
                    "Content-Type: text/xml\r\n" +
                    "Content-Length: " + inStream.Length + "\r\n" +
                    "Last-modified: " + modified.ToString("r") + "\r\n\r\n";
                httpHeaderBuffer = Encoding.UTF8.GetBytes(httpHeader);
                outStream.Write(httpHeaderBuffer, 0, httpHeaderBuffer.Length);

                int i = 0;
                while ((i = inStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    outStream.Write(buffer, 0, i);
                }
            }
        }
    }
}
