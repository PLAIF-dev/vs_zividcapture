using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZividCapture.Cameras;

namespace ZividCapture.App.Dialogs.ViewModels
{
    public class CameraViewModel : BindableBase
    {
        ICamera _camera;
        public ICamera Camera
        {
            get => _camera;
            set => SetProperty(ref _camera, value);
        }
        public CameraViewModel(ICamera camera)
        {
            Camera = camera;
        }
    }
    public class SelectCameraViewModel : BindableBase, IDialogAware
    {
        string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public DialogCloseListener RequestClose { get; set; }

        public DelegateCommand SearchCamerasCommand { get; set; }
        public DelegateCommand OKCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }


        ObservableCollection<CameraViewModel> _cameras;
        public ObservableCollection<CameraViewModel> Cameras
        {
            get => _cameras;
            set => SetProperty(ref _cameras, value);
        }

        CameraViewModel _selectedCamera;
        public CameraViewModel SelectedCamera
        {
            get => _selectedCamera;
            set
            {
                SetProperty(ref _selectedCamera, value);
                OKCommand.RaiseCanExecuteChanged();
            }
        }
        bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(ref _isEnable, value);
        }
        public SelectCameraViewModel()
        {
            Title = "Select Camera";
            SearchCamerasCommand = new(() =>
            {
                Task.Factory.StartNew(() =>
                {
                    IsEnable = false;
                    SelectedCamera = null;
                    Cameras = new(ICamera.EnumeratorCameras().ToList().Select(x => new CameraViewModel(x)));
                    IsEnable = true;
                });

            });
            OKCommand = new(() =>
            {
                var result = new DialogParameters();
                result.Add("camera", SelectedCamera.Camera);
                RequestClose.Invoke(result, ButtonResult.OK);
            },
            () =>
            {
                return SelectedCamera != null;
            });
            CancelCommand = new(() =>
            {
                RequestClose.Invoke(ButtonResult.Cancel);
            });
        }
        public bool CanCloseDialog()
        {
            return IsEnable;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            IsEnable = true;
        }
    }
}
