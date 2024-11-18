using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZividCapture.App.Events
{
    public class WorkingInfo
    {
        public bool IsWorking { get; init; }
        public string WorkingMessage { get; init; }
        public WorkingInfo(bool isWorking, string workMessage)
        {
            IsWorking = isWorking;
            WorkingMessage = workMessage;
        }

    }
    public class WorkingRequestEvent : PubSubEvent<WorkingInfo>
    {
    }
}
