using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZividCapture.Cameras;

namespace ZividCapture.App.ViewModels
{
    public class AppSettingViewModel : BindableBase
    {
        ICamera _camera;
        [DefaultValue(null)]
        public ICamera Camera
        {
            get => _camera;
            set => SetProperty(ref _camera, value);
        }

        FileInfo _captureSettingFileInfo;
        [DefaultValue(null)]
        [DisplayName("Capture Setting File")]
        public FileInfo CaptureSettingFileInfo
        {
            get => _captureSettingFileInfo;
            set => SetProperty(ref _captureSettingFileInfo, value);
        }

        DirectoryInfo _saveDirectory;
        [DefaultValue(null)]
        [DisplayName("Save Directory")]
        public DirectoryInfo SaveDirectory
        {
            get => _saveDirectory;
            set => SetProperty(ref _saveDirectory, value);
        }
    }
}
