using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Profiler.Common.Models;

namespace NServiceBus.Profiler.Core
{
    public interface IQueueManager
    {
        IList<Queue> GetQueues(string machineName = null);
        IList<MessageInfo> GetMessages(Queue queue);
        MessageBody GetMessageBody(Queue queue, string messageId);
        Queue CreatePrivateQueue(Queue queue, bool transactional = true);

        void SendMessage(Queue destination, object message);
        void DeleteQueue(Queue queue);
        void MoveMessage(Queue source, Queue destination, string messageId);
        void DeleteMessage(Queue queue, MessageInfo message);
        void Purge(Queue queue);
        int GetMessageCount(Queue queue);
    }

    public interface IQueueManagerAsync : IQueueManager
    {
        new Task<IList<MessageInfo>> GetMessages(Queue queue);
    }
}