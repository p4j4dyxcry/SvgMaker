using System;

namespace SvgMakerCore.Core
{
    public class RemoteEvent<TEventArgs> : MarshalByRefObject where TEventArgs : EventArgs
    {
        private Action<TEventArgs> OnEvent { get; }

        public RemoteEvent(Action<TEventArgs> onEvent)
        {
            OnEvent = onEvent;
        }
        public void CallbackToClient(TEventArgs evt) => OnEvent?.Invoke(evt);
    }
}
