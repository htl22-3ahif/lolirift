using fun.Core;
using Environment = fun.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using lolirift.Controllers.External;
using lolirift.Controllers;

namespace lolirift
{
    public sealed class NetworkAccepterElement : Element
    {
        private TcpListener listener;
        private ExInitializationController init;
        private DataStore data;

        public int Port;

        public NetworkAccepterElement(Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClient), null);

            data = new DataStore()
            {
                Environment = Environment
            };

            init = new ExInitializationController(data);
        }

        private void AcceptTcpClient(IAsyncResult res)
        {
            var tcp = listener.EndAcceptTcpClient(res);
            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClient), null);

            data.Tcp = tcp;
            Init(tcp);
            data.Tcp = null;
        }

        private void Init(TcpClient tcp)
        {
            var net = tcp.GetStream();
            var buffer = new byte[4096];
            int length = 0;
            do
            {
                length = net.Read(buffer, 0, buffer.Length);
            } while (length == 0);

            var json = string.Empty;
            var openBracketCount = buffer.Count(b => b == '{');
            var closedBracketCount = buffer.Count(b => b == '}');
            json += Encoding.UTF8.GetString(buffer, 0, length);

            while (openBracketCount != closedBracketCount)
            {
                length = net.Read(buffer, 0, buffer.Length);
                json += Encoding.UTF8.GetString(buffer, 0, length);
            }

            var j = JsonConvert.DeserializeObject(json) as JObject;

            if (init.IsExecutable(j))
                init.Execute(j);
            else
                throw new Exception();
        }
    }
}
