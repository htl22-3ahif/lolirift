using fun.Core;
using lolirift.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lolirift
{
    public sealed class WebAccepterElement : Element
    {
        private HttpListener listener;
        private DataStore data;
        private InitializationController init;

        public int Port;

        public WebAccepterElement(fun.Core.Environment environment, Entity entity)
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:844/");
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(GetContext), null);

            data = new DataStore()
            {
                Environment = Environment
            };

            init = new InitializationController(data);
        }

        private void GetContext(IAsyncResult res)
        {
            var context = listener.EndGetContext(res);
            listener.BeginGetContext(new AsyncCallback(GetContext), null);

            var request = context.Request;
            var response = context.Response;

            if (context.Request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                response.OutputStream.Close();
            }
            else
            {
                data.ResponseData = new byte[] { };
                var requestData = new byte[context.Request.ContentLength64];
                request.InputStream.Read(requestData, 0, requestData.Length);
                var requestString = Encoding.UTF8.GetString(requestData);

                var j = JsonConvert.DeserializeObject(requestString) as JObject;

                if (init.IsExecutable(j))
                    init.Execute(j);
                else
                {
                    var name = request.RawUrl.Split('/')[0];
                    var loli = Environment.GetEntity(name).GetElement<LoliconElement>();
                    data.Lolicon = loli;
                    data.Controllers = loli.Controllers;
                    foreach (var c in loli.Controllers)
                        if (c.IsExecutable(j))
                            c.Execute(j);
                }

                response.StatusCode = 200;
                response.ContentEncoding = Encoding.UTF8;
                response.ContentLength64 = data.ResponseData.Length;
                response.ContentType = "application/json";
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                response.OutputStream.Write(data.ResponseData, 0, data.ResponseData.Length);

                response.OutputStream.Close();
            }

        }

        private void InitHttp(TcpClient tcp)
        {
            var net = tcp.GetStream();
            var buffer = new byte[4096];
            var length = 0;
            var request = string.Empty;

            do
            {
                length = net.Read(buffer, 0, buffer.Length);
                request += Encoding.UTF8.GetString(buffer, 0, length);
            } while (!request.Contains("POST / HTTP/1.1"));

            var response = Encoding.UTF8.GetBytes(
@"HTTP/1.1 200 OK
Date: Mon, 27 Jul 2009 12:28:53 GMT
Server: Apache/2.2.14 (Win32)
Last-Modified: Wed, 22 Jul 2009 19:15:56 GMT
Content-Length: 88
Content-Type: text/html
Connection: Closed

<html>
<body>
<h1>Hello, World!</h1>
</body>
</html>");
            net.Write(response, 0, response.Length);
        }

        private void Init(TcpClient tcp)
        {
            var net = tcp.GetStream();
            var buffer = new byte[4096];
            var length = 0;
            var openBracketCount = 0;
            var closedBracketCount = 0;
            var json = string.Empty;
            do
            {
                do
                {
                    length = net.Read(buffer, 0, buffer.Length);
                } while (length == 0);

                openBracketCount = buffer.Count(b => b == '{');
                closedBracketCount = buffer.Count(b => b == '}');
                json += Encoding.UTF8.GetString(buffer, 0, length);

                while (openBracketCount != closedBracketCount)
                {
                    length = net.Read(buffer, 0, buffer.Length);
                    json += Encoding.UTF8.GetString(buffer, 0, length);
                }
            } while (openBracketCount == 0);


            json = json.Substring(json.IndexOf('{'), json.LastIndexOf('}') + 1);

            var j = JsonConvert.DeserializeObject(json) as JObject;

            if (init.IsExecutable(j))
                init.Execute(j);
            else
                throw new Exception();
        }

    }
}
