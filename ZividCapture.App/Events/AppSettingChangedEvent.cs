using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZividCapture.App.ViewModels;

namespace ZividCapture.App.Events
{
    public class AppSettingChangedEvent : PubSubEvent<AppSettingViewModel>
    {
    }
}
