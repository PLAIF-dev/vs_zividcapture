using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZividCapture.Cameras.Cameras.Zivid;

namespace ZividCapture.Cameras
{
    public interface ICameraMatrix
    {
        double FX { get; }
        double FY { get; }
        double CX { get; }
        double CY { get; }
    }
    public interface IFrameSize
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }
    }
    public interface IFrame : IDisposable
    {
        public DateTime FrameCpturedTime { get; }
        public ICameraMatrix GetCameraMatrix();
        public Mat GetNormalMap();
        public Mat GetRGB();
        public Mat GetDepthMap();
        public void SavePly(string plyPath);
        public FrameSavedPaths SaveAll(string directoryPath, string id);
    }
    public interface IRGBImage
    {
        IFrameSize Size { get; }
    }

    public interface INormalMap
    {
        IFrameSize Size { get; }
    }
    public interface IDepthMap
    {
        IFrameSize Size { get; }
    }
    public enum Manufacturor
    {
        Zivid
    }
    public interface ICameraInfo
    {
        public Manufacturor Manufacturor { get; }
        public string SerialNumber { get; }
        public string ProductName { get; }
    }
    public class CameraInfo : ICameraInfo
    {
        public Manufacturor Manufacturor { get; internal set; }

        public string SerialNumber { get; internal set; }

        public string ProductName { get; internal set; }
    }
    public interface ICaptureSetting
    {
        public Manufacturor Manufacturor { get; }
        public static ICaptureSetting GetZividSetting(string settingFile)
        {
            return new ZividCaptureSetting(settingFile);
        }
    }
    public interface ICamera : ICameraInfo
    {
        public bool IsConnected { get; }
        public bool HasCaptureSetting { get; }
        event EventHandler ConnectingEvent;
        event EventHandler ConnectedEvent;
        event EventHandler DisconnectingEvent;
        event EventHandler DisconnectedEvent;
        event EventHandler ConnectionErrorEvent;

        public void Connect();
        public void SetCaptureSetting(ICaptureSetting setting);
        public void Disconnect();
        public IFrame Capture();
        public ICameraMatrix GetCameraMatrix();
        public static IEnumerable<ICamera> EnumeratorCameras(IEnumerable<ICamera> fakes = null)
        {
            var manufacturors = Enum.GetValues<Manufacturor>().ToList();
            foreach (var manufacturor in manufacturors)
            {
                if (manufacturor == Manufacturor.Zivid)
                    foreach (var cam in ZividInstance.EnumerateCameras())
                        yield return cam;
            }
        }
    }
}
