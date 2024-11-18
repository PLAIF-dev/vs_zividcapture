using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZividCapture.Cameras;

namespace ZividCapture.App.Events
{
    public class FrameSavedEvent :PubSubEvent<FrameSavedPaths>
    {
    }
}
