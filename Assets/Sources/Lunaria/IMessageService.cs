using System;

namespace Lunaria
{
    public interface IMessageService
    {
        void AddHandler<T>(Action<T> handler);
        void RemoveHandler<T>(Action<T> handler);
        void SendMessage(object message);
        void Clear();
    }
}
