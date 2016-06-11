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
                {
                    Console.WriteLine("Initializing new member...");
                    init.Execute(j);
                    Console.WriteLine("New member initialized!");
                }
                else
                {
                    var name = request.RawUrl.Split('/')[1];
                    var loli = Environment.GetEntity(name).GetElement<LoliconElement>();
                    data.Lolicon = loli;
                    data.Controllers = loli.Controllers;
                    foreach (var c in loli.Controllers)
                        if (c.IsExecutable(j))
                        {
                            Console.WriteLine("Parsing an command...");
                            c.Execute(j);
                            Console.WriteLine("Parsing successful!");
                        }
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
    }
}
