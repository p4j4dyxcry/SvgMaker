using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;

namespace SvgMakerCore.Core
{
    public class IpcClient<TServerContext> where TServerContext : MarshalByRefObject
    {
        public TServerContext ServerContext { get; }

        public IpcClient(string uid, string port, string serverAddress)
        {
            // IPCチャンネルの生成
            var channelProperties = new Dictionary<string, string> { { "portName", $"dummy{uid}" }, { "name", $"ipc{uid}" } };
            var channel = new IpcChannel(channelProperties, null,
                new BinaryServerFormatterSinkProvider
                {
                    TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full,
                });

            ChannelServices.RegisterChannel(channel, true);

            // リモートオブジェクトを取得
            ServerContext = Activator.GetObject(typeof(TServerContext), MakeIpcServerUri($"{port}{uid}", $"{serverAddress}{uid}")) as TServerContext;
        }

        //URIは ipc:/"チャンネル"/"アドレス"の形になる
        public static string MakeIpcServerUri(string ipcChannel, string ipcAddress) => $"ipc://{ipcChannel}/{ipcAddress}";
    }
}
