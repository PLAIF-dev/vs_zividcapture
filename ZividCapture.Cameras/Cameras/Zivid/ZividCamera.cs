using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zivid.NET;
using Zivid.NET.Experimental.Calibration;
namespace ZividCapture.Cameras.Cameras.Zivid
{
    internal class ZividCamera : ICamera
    {
        public bool IsConnected
        {
            get
            {
                if (_cam == null)
                    return false;

                return _cam.State.Connected;
            }
        }
        public string SerialNumber { get; private set; }
        public Manufacturor Manufacturor => Manufacturor.Zivid;

        public string ProductName { get; private set; }

        public bool HasCaptureSetting => _settings != null;

        Application _zivid;
        Camera? _cam;
        Settings? _settings;
        public event EventHandler ConnectingEvent;
        public event EventHandler ConnectedEvent;
        public event EventHandler DisconnectingEvent;
        public event EventHandler DisconnectedEvent;
        public event EventHandler ConnectionErrorEvent;

        internal ZividCamera(ICameraInfo camInfo)
        {
            _zivid = ZividInstance.Instance;
            SerialNumber = camInfo.SerialNumber;
            ProductName = camInfo.ProductName;
        }

        public void Connect()
        {
            if (_cam != null)
                throw new Exception();

            ConnectingEvent?.Invoke(this, new());
            _cam = _zivid.ConnectCamera(SerialNumber);

            if (!_cam.State.Connected)
                throw new Exception();

            ConnectedEvent?.Invoke(this, new());

        }

        public void Disconnect()
        {
            if (_cam == null)
                throw new Exception();

            DisconnectingEvent?.Invoke(this, new());
            _settings = null;
            _cam.Disconnect();
            _cam.Dispose();
            _cam = null;

            DisconnectedEvent?.Invoke(this, new());
        }

        public IFrame Capture()
        {
            if (!HasCaptureSetting)
                throw new Exception();

            var frame = _cam?.Capture(_settings);

            if (frame == null)
                throw new Exception();
            GC.SuppressFinalize(frame);
            return new ZividFrame(frame);
        }

        private Settings GetSettings(ICaptureSetting? setting)
        {
            if (setting == null)
            {
                return new Settings
                {
                    Acquisitions = { new Settings.Acquisition { } }
                };
            }
            else
            {
                if (setting is ZividCaptureSetting zividCaptureSetting)
                    return zividCaptureSetting.GetSettings();
                else
                    throw new Exception();
            }
        }

        public ICameraMatrix GetCameraMatrix()
        {
            if (_cam == null)
                throw new Exception();

            var intrinsics = Calibrator.Intrinsics(_cam);

            return new ZividCameraMatrix(intrinsics.CameraMatrix.FX, intrinsics.CameraMatrix.FY, intrinsics.CameraMatrix.CX, intrinsics.CameraMatrix.CY);
        }

        public void SetCaptureSetting(ICaptureSetting setting)
        {
            _settings = GetSettings(setting);
            if (_settings == null)
                throw new Exception();
        }
    }
}
