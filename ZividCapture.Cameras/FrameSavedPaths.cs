using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZividCapture.Cameras
{
    public class FrameSavedPaths
    {
        public string PlyPath { get; init; }
        public string RGBImgPath { get; init; }
        public string NormalsImgPath { get; init; }
        public string DepthImgPath { get; init; }

        public FrameSavedPaths(string directoryPath, string id)
        {
            DepthImgPath = Path.Combine(directoryPath, $"{id}_depth.png");
            NormalsImgPath = Path.Combine(directoryPath, $"{id}_normals.png");
            PlyPath = Path.Combine(directoryPath, $"{id}.ply");
            RGBImgPath = Path.Combine(directoryPath, $"{id}_rgb.png");
        }
    }
}
