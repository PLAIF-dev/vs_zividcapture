using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZividCapture.App.Events;
using ZividCapture.Cameras;

namespace ZividCapture.App.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(ref _isEnable, value);
        }

        string? _workingMessage;
        public string? WorkingMessage
        {
            get => _workingMessage;
            set => SetProperty(ref _workingMessage, value);
        }
        public DelegateCommand CaptureCommand { get; set; }
        IEventAggregator _eventAggregator;

        AppSettingViewModel? appSetting;
        public MainWindowViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            IsEnable = true;

            eventAggregator.GetEvent<WorkingRequestEvent>().Subscribe((workInfo) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    IsEnable = !workInfo.IsWorking;
                    WorkingMessage = workInfo.WorkingMessage;
                });
            });

            eventAggregator.GetEvent<AppSettingChangedEvent>().Subscribe((appSetting) =>
            {
                this.appSetting = appSetting;
                CaptureCommand?.RaiseCanExecuteChanged();
            });

            CaptureCommand = new(CaptureAsync, CanCapture);
        }
        private bool CanCapture()
        {
            if (appSetting == null)
                return false;

            return appSetting.CaptureSettingFileInfo != null &&
                    appSetting.SaveDirectory != null &&
                        appSetting.Camera != null;
        }
        private async void CaptureAsync()
        {
            try
            {
                _eventAggregator.GetEvent<WorkingRequestEvent>().Publish(new(true, "Capturing ..."));

                if (appSetting == null)
                    throw new Exception();

                FrameSavedPaths paths = null;
                await Task.Run(() =>
                {
                    ICaptureSetting? captureSetting = null;
                    if (appSetting.Camera.Manufacturor == Manufacturor.Zivid)
                        captureSetting = ICaptureSetting.GetZividSetting(appSetting.CaptureSettingFileInfo.FullName);
                    if (captureSetting == null)
                        throw new Exception();

                    if (!appSetting.Camera.HasCaptureSetting)
                        appSetting.Camera.SetCaptureSetting(captureSetting);

                    var frame = appSetting.Camera.Capture();
                    var now = DateTime.Now.ToFileTime().ToString();
                    var baseDir = appSetting.SaveDirectory.FullName;
                    _eventAggregator.GetEvent<WorkingRequestEvent>().Publish(new(true, "Saving ..."));

                    paths = frame.SaveAll(baseDir, now);

                });
                if (paths == null)
                    throw new Exception();

                _eventAggregator.GetEvent<FrameSavedEvent>().Publish(paths);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _eventAggregator.GetEvent<WorkingRequestEvent>().Publish(new(false, "Capturing ..."));
            }
        }
    }
}
