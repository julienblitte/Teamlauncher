using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teamlauncher
{
    class WebServer
    {
        private TcpListener server;
        private string database;
        private Thread thread;

        public const string realm = "Teamlauncher";
        public const string username = "teamlauncher";
        public const string uri = "/";

        private byte[] localOpaque;
        private string password;

        private bool debug;

        public WebServer(IPAddress ip, int port, string file)
        {
            RegistryConfig reg;

            server = new TcpListener(ip, port);
            database = file;
            thread = null;

            debug = false;

            using (reg = new RegistryConfig())
            {
                localOpaque = reg.readBinary(RegistryConfig.REGISTRY_KEY_WEB_LOCALOPAQUE);
                if (localOpaque.Length == 0)
                {
                    using (var rijndael = System.Security.Cryptography.Rijndael.Create())
                    {
                        rijndael.GenerateKey();
                        localOpaque = rijndael.Key;
                    }
                    reg.writeBinary(RegistryConfig.REGISTRY_KEY_WEB_LOCALOPAQUE, localOpaque);
                }
            }
        }

        public void setDebug(bool debugMode)
        {
            debug = debugMode;
        }

        public void start()
        {
            Trace.WriteLineIf(debug, "WebServer.start()");
            if (thread == null)
            {
                thread = new Thread(listen);
                thread.Start();
            }
        }

        public void stop()
        {
            Trace.WriteLineIf(debug, "WebServer.stop()");
            if (thread != null)
            {
                server.Stop();
                thread.Abort();
                thread = null;
            }
        }

        public void setPassword(string serverPassword)
        {
            password = serverPassword;
        }

        protected void listen()
        {
            server.Start();

            try
            { 
                // Enter the listening loop.
                while (true)
                {
                    TcpClient client;
                    NetworkStream stream;
                    Dictionary<string, string> headerRequest;
                    Dictionary<string, string> headerResponse;

                    client = server.AcceptTcpClient();
                    stream = client.GetStream();

                    headerRequest = readHeader(stream);
                    headerResponse = new Dictionary<string, string>();

                    if ((headerRequest != null) && (headerRequest["GET"] == uri))
                    {
                        bool authenticated;

                        authenticated = false;
                        if (headerRequest.ContainsKey("Authorization"))
                        {
                            if (verifyAuthorization((string)headerRequest["Authorization"]))
                            {
                                authenticated = true;
                            }
                        }

                        if (authenticated)
                        {
                            headerResponse["Content-Type"] = "text/xml";
                            reply(stream, 200, headerResponse, true, database);
                            //replyContent(stream, database);
                        }
                        else
                        {
                            byte[] nonceBuffer, opaque;

                            using (var rijndael = Rijndael.Create())
                            {
                                rijndael.GenerateKey();
                                nonceBuffer = rijndael.Key;

                                opaque = new byte[nonceBuffer.Length + localOpaque.Length];
                                Buffer.BlockCopy(nonceBuffer, 0, opaque, 0, nonceBuffer.Length);
                                Buffer.BlockCopy(localOpaque, 0, opaque, nonceBuffer.Length, localOpaque.Length);
                            }

                            headerResponse["WWW-Authenticate"] = String.Format("Digest realm=\"{0}\", nonce=\"{1}\", opaque=\"{2}\"",
                                    realm, Convert.ToBase64String(nonceBuffer), md5(opaque));
                            reply(stream, 401, headerResponse, false, null);
                            //replyUnauthorized(stream);
                        }
                    }
                    else
                    {
                         reply(stream, 404, headerResponse, false, null);
                        //replyNotFound(stream);
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

        public string md5(byte[] data)
        {
            byte[] hash;

            using (MD5 md5 = MD5.Create())
            {
               hash = md5.ComputeHash(data);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public string md5(string input)
        {
            return md5(Encoding.ASCII.GetBytes(input));
        }

        protected Dictionary<string, string> readHeader(NetworkStream stream)
        {
            string request;
            Dictionary<string, string> header;
            string key, value;
            bool value_mode;
            byte[] inBuffer;

            int i, len, eoh, line;


            inBuffer = new byte[2048];
            len = stream.Read(inBuffer, 0, inBuffer.Length);
            if (len == 0)
            {
                return null;
            }

            // Translate data bytes to a ASCII string.
            request = Encoding.UTF8.GetString(inBuffer, 0, len);
            header = new Dictionary<string, string>();

            i = 0;
            line = 0;
            key = "";
            value = "";
            value_mode = false;
            eoh = 0;
            while ((i < len) && (eoh < 4))
            {
                switch (request[i])
                {
                    case ':':
                        eoh = 0;
                        if (value_mode)
                        {
                            value += request[i];
                        }
                        else
                        {
                            value_mode = true;
                        }
                    break;
                    case '\r':
                    case '\n':
                        eoh++;
                        if (key != "")
                        {
                            if (line == 0)
                            {
                                string[] request_line = key.Split(' ');
                                header.Add(request_line[0], request_line[1]);
                                Trace.WriteLine(String.Format("<Request> [{0}, {1}]", request_line[0], request_line[1]));
                            }
                            else
                            {
                                header.Add(key.Trim(new char[] {' ', '"' }), value.Trim(new char[] { ' ', '"' }));
                                Trace.WriteLine(String.Format("<RequestHeader> {0}: {1}", key.Trim(new char[] { ' ', '"' }), value.Trim(new char[] { ' ', '"' })));
                            }
                            line++;
                        }
                        key = "";
                        value = "";
                        value_mode = false;
                    break;
                    default:
                        eoh = 0;
                        if (!value_mode)
                        {
                            key += request[i];
                        }
                        else
                        {
                            value += request[i];
                        }
                    break;
                }
                i++;
            }

            return header;
        }

        protected void reply(NetworkStream outStream, int responseCode, Dictionary<string,string> header, bool externalFile, string content)
        {
            StringBuilder httpHeader;
            byte[] outHeaderBuffer;
            FileVersionInfo version;
            byte[] outBuffer = new byte[0]; ;

            Dictionary<int, string> responseStatus;
            responseStatus = new Dictionary<int, string>();
            responseStatus[200] = "OK";
            responseStatus[400] = "Bad Request";
            responseStatus[401] = "Unauthorized";
            responseStatus[402] = "Payment Required";
            responseStatus[403] = "Forbidden";
            responseStatus[404] = "Not Found";
            responseStatus[405] = "Method Not Allowed";
            responseStatus[406] = "Not Acceptable";

            if (externalFile)
            {
                if (content == null || !File.Exists(content))
                {
                    externalFile = false;
                    content = null;
                    responseCode = 404;
                }
            }

            if (content == null)
            {
                switch (responseCode)
                {
                    case 400:
                    case 401:
                    case 402:
                    case 403:
                    case 404:
                    case 405:
                    case 406:
                        content = String.Format("<!DOCTYPE html>\r\n" +
                            "<html><head><meta charset=\"UTF-8\"/><title>Error {0}</title></head>" +
                            "<body><p>{0} {1}.</h1></body></html>", responseCode, responseStatus[responseCode]);
                        header["Content-Type"] = "text/html";
                        break;
                    default:
                        content = "";
                        break;
                }
            }

            version = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            header["Server"] = "Teamlauncher/" + version.FileMajorPart + "." + version.FileMinorPart;
            header["Date"] = DateTime.UtcNow.ToString("r");
            if (!header.ContainsKey("Content-Type"))
            {
                header["Content-Type"] = (externalFile ? "application/octet-stream" : "text/html");
            }

            if (content != null)
            {
                if (externalFile)
                {
                    header["Content-Length"] = (new FileInfo(content)).Length.ToString();
                    header["Last-modified"] = File.GetLastWriteTime(content).ToString("r");
                }
                else
                {
                    outBuffer = Encoding.UTF8.GetBytes(content);
                    header["Content-Length"] = outBuffer.Length.ToString();
                }
            }
            else
            {
                header["Content-Length"] = "0";
            }


            httpHeader = new StringBuilder();
            httpHeader.AppendFormat("HTTP/1.0 {0} {1}\r\n", responseCode, responseStatus[responseCode]);
            Trace.WriteLineIf(debug, String.Format("<Response> {0} {1}", responseCode, responseStatus[responseCode]));
            foreach (var h in header)
            {
                httpHeader.Append(h.Key);
                httpHeader.Append(": ");
                httpHeader.Append(h.Value);
                httpHeader.Append("\r\n");
                Trace.WriteLineIf(debug, String.Format("<ResponseHeader> {0}: {1}", h.Key, h.Value));
            }
            httpHeader.Append("\r\n");

            outHeaderBuffer = Encoding.UTF8.GetBytes(httpHeader.ToString());
            outStream.Write(outHeaderBuffer, 0, outHeaderBuffer.Length);

            if (content != null)
            {
                if (externalFile)
                {
                    using (FileStream inStream = File.Open(content, FileMode.Open))
                    {
                        int i = 0;
                        outBuffer = new byte[2048];
                        while ((i = inStream.Read(outBuffer, 0, outBuffer.Length)) != 0)
                        {
                            outStream.Write(outBuffer, 0, i);
                        }
                    }
                }
                else
                {
                    outStream.Write(outBuffer, 0, outBuffer.Length);
                }
            }
            Trace.WriteLineIf(debug, "---");

        }

        protected bool verifyAuthorization(string auth)
        {
            Dictionary<string,string> values;
            string[] values_raw;
            string HA1, HA2, expected;
            
            values = new Dictionary<string, string>();

            auth = auth.Substring(7); // remove "Digest "
            values_raw = auth.Split(',');

            foreach(var value_line in values_raw)
            {
                string key, value;

                int equal = value_line.IndexOf('=');
                if (equal >= 0)
                {
                    key = value_line.Substring(0, equal).Trim(new char[] { ' ', '"' });
                    value = value_line.Substring(equal + 1).Trim(new char[] { ' ', '"' });

                    values.Add(key, value);
                    Trace.WriteLineIf(debug, String.Format("<RequestAuth> {0}={1}", key, value));
                }
            }

            if (!values.ContainsKey("realm") || !values.ContainsKey("uri") || !values.ContainsKey("nonce")
                || !values.ContainsKey("opaque") || !values.ContainsKey("response"))
            {
                return false;
            }

            // TODO: UNSAFE !!!
            // TODO: CHECK NONCE

            if (values["realm"] == realm && values["uri"] == uri)
            {
                HA1 = md5(String.Format("{0}:{1}:{2}", username, realm, password));
                Trace.WriteLineIf(debug, String.Format("<RequestCheck> {0}={1}", "A1", String.Format("{0}:{1}:{2}", username, realm, password)));
                Trace.WriteLineIf(debug, String.Format("<RequestCheck> {0}={1}", "HA1", HA1));

                HA2 = md5(String.Format("{0}:{1}", "GET", uri));
                Trace.WriteLineIf(debug, String.Format("<RequestCheck> {0}={1}", "A2", String.Format("{0}:{1}", "GET", uri)));
                Trace.WriteLineIf(debug, String.Format("<RequestCheck> {0}={1}", "HA2", HA2));

                expected = md5(String.Format("{0}:{1}:{2}", HA1, values["nonce"], HA2));
                Trace.WriteLineIf(debug, String.Format("<RequestCheck> {0}={1}", "Response pre-Hash", String.Format("{0}:{1}:{2}", HA1, values["nonce"], HA2)));
                Trace.WriteLineIf(debug, String.Format("<RequestCheck> {0}={1}", "Response", expected));
                Trace.WriteLineIf(debug, String.Format("<RequestCheck> {0}={1}", "ProvidedResponse", values["response"]));

                return (expected == values["response"]);
            }

            return false;
        }

    }
}
