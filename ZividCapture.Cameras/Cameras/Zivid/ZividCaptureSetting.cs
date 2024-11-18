using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zivid.NET;

namespace ZividCapture.Cameras.Cameras.Zivid
{
    internal class ZividCaptureSetting : ICaptureSetting
    {
        public Manufacturor Manufacturor => Manufacturor.Zivid;

        internal string _settingFile;
        internal ZividCaptureSetting(string settingFile)
        {
            _settingFile = settingFile;
        }

        internal Settings GetSettings()
        {
            return new Settings(_settingFile);
        }
    }
}
