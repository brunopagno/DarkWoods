using System;
using System.Collections.Generic;

namespace Lunaria
{
    public class MessageService : IMessageService
    {
        private Dictionary<Type, List<MessageHandler>> _handlers = new Dictionary<Type, List<MessageHandler>>();
        private List<Delegate> pendingRemovals = new List<Delegate>();
		private bool isRaisingMessage;

        public void AddHandler<T>(Action<T> handler)
        {
            List<MessageHandler> handlers;
            if (!_handlers.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<MessageHandler>();
                _handlers[typeof(T)] = handlers;
            }
            if (handlers.Find(h => h.Action == handler) == null)
            {
				handlers.Add(new MessageHandler(handler.Target, handler));
            }
        }

        public void RemoveHandler<T>(Action<T> handler)
        {
            List<MessageHandler> delegates = null;
			if (_handlers.TryGetValue(typeof(T), out delegates))
			{
				MessageHandler existingHandler = delegates.Find(h => h.Action == handler);
				if (existingHandler != null)
				{
					if (isRaisingMessage)
                    {
						pendingRemovals.Add(handler);
                    }
					else
                    {
						delegates.Remove(existingHandler);
                    }
				}
			}
        }

        public void SendMessage(object message)
        {
            try
			{
				List<MessageHandler> handlers = null;
				if (_handlers.TryGetValue(message.GetType(), out handlers))
				{
					isRaisingMessage = true;
					try
					{
						foreach (MessageHandler handler in handlers)
						{
							handler.Action.DynamicInvoke(message);
						}
					}
					finally
					{
						isRaisingMessage = false;
					}
					foreach (Delegate action in pendingRemovals)
					{
						MessageHandler existingHandler = handlers.Find(h => h.Action == action);
						if (existingHandler != null)
                        {
							handlers.Remove(existingHandler);
                        }
					}
					pendingRemovals.Clear();
				}
			}
			catch(Exception ex)
			{
				UnityEngine.Debug.LogError("Exception while sending message " + message + ": " + ex);
			}
        }

        public void Clear()
        {
            _handlers.Clear();
        }
    }

    internal class MessageHandler
    {
        public object Target { get; set; }
        public Delegate Action { get; set; }

        public MessageHandler(object target, Delegate action)
        {
            Target = target;
            Action = action;
        }
    }
}
