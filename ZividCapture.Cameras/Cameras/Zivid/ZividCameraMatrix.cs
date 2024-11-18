using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZividCapture.Cameras.Cameras.Zivid
{
    internal class ZividCameraMatrix : ICameraMatrix
    {
        public double FX { get; private set; }
        public double FY { get; private set; }
        public double CX { get; private set; }
        public double CY { get; private set; }

        public ZividCameraMatrix(double fx, double fy, double cx, double cy)
        {
            FX = fx;
            FY = fy;
            CX = cx;
            CY = cy;
        }

        public override string ToString()
        {
            return $"[{FX},0,{CX}]\n[0,{FY},{CY}]\n[0,0,1]";
        }
    }
}
