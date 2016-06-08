using fun.Core;
using Environment = fun.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace lolirift
{
    public sealed class NetworkAccepterElement : Element
    {
        private TcpListener listener;

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
        }

        private void AcceptTcpClient(IAsyncResult res)
        {
            var tcp = listener.EndAcceptTcpClient(res);
            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClient), null);

            var entity = new Entity("Lolicon" + tcp.Client.RemoteEndPoint.ToString(), Environment);
            entity.AddElement<LoliconElement>();
            entity.GetElement<LoliconElement>().Tcp = tcp;

            lock(Environment)
                Environment.AddEntity(entity);
        }
    }
}
