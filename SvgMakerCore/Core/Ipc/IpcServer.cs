using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace SvgMakerCore.Core
{
    public class IpcServer<TServerContext> where TServerContext : MarshalByRefObject
    {
        public TServerContext ServerContext { get; private set; }

        private bool _isStarted;

        public void Start(string uid, string port, string serverAddress)
        {
            if (_isStarted)
                return;

            _isStarted = true;

            var channelProperties = new Dictionary<string, string> { { "portName", $"{port}{uid}" }, { "name", $"ipc{uid}" } };

            var channel = new IpcChannel(channelProperties, null,
                new BinaryServerFormatterSinkProvider
                {
                    TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full,
                });

            ChannelServices.RegisterChannel(channel, true);

            ServerContext = Activator.CreateInstance(typeof(TServerContext)) as TServerContext;

            RemotingServices.Marshal(ServerContext, $"{serverAddress}{uid}", typeof(TServerContext));
        }
    }
}
