using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zivid.NET;

namespace ZividCapture.Cameras.Cameras.Zivid
{
    internal static class ZividInstance
    {
        private static readonly Lazy<Application> _instance = new Lazy<Application>(() => new Application());
        public static Application Instance { get { return _instance.Value; } }

        public static IEnumerable<ICamera> EnumerateCameras()
        {
            var zivid = Instance;

            foreach (var cam in zivid.Cameras)
            {
                if (!cam.State.Available)
                    continue;

                var camInfo = new CameraInfo()
                {
                    SerialNumber = cam.Info.SerialNumber,
                    Manufacturor = Manufacturor.Zivid,
                    ProductName = cam.Info.ModelName
                };
                yield return new ZividCamera(camInfo);
            }
        }
    }
}
