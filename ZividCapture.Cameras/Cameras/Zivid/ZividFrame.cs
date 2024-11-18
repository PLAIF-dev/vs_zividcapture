using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zivid.NET;
using Zivid.NET.Experimental.Calibration;

namespace ZividCapture.Cameras.Cameras.Zivid
{
    internal class ZividFrame : IFrame
    {
        public DateTime FrameCpturedTime { get; init; }

        Frame _frame;
        int w;
        int h;
        public ZividFrame(Frame frame)
        {
            FrameCpturedTime = DateTime.Now;
            _frame = frame;
            w = (int)_frame.PointCloud.Width;
            h = (int)_frame.PointCloud.Height;
        }

        public Mat GetNormalMap()
        {
            var normals = _frame.PointCloud.CopyNormalsXYZ();
            using (var ImageNormals = new Mat((int)h, (int)w, MatType.CV_8UC3))
            {
                Parallel.For(0, w, (col) =>
                {
                    Parallel.For(0, h, (row) =>
                    {
                        if (Double.IsNaN(normals[row, col, 0]))
                            ImageNormals.Set<Vec3b>(row, col, new(0, 0, 0));

                        var valX = 255.0 * ((-normals[row, col, 0] + 1.0) / 2.0);
                        var valY = 255.0 * ((-normals[row, col, 1] + 1.0) / 2.0);
                        var valZ = 255.0 * ((-normals[row, col, 2] + 1.0) / 2.0);

                        ImageNormals.Set<Vec3b>(row, col, new((byte)valZ, (byte)valY, (byte)valX));
                    });
                });

                return ImageNormals.Clone();
            }
        }
        public Mat GetDepthMap()
        {
            var depth = _frame.PointCloud.CopyPointsZ();
            var depthList = depth.Cast<float>().ToList();
            var depthMin = depthList.Where(n => !float.IsNaN(n)).Min();
            var depthMax = depthList.Where(n => !float.IsNaN(n)).Max();
            using (var ImageDepth = new Mat((int)h, (int)w, MatType.CV_8UC1))
            {
                Parallel.For(0, w, (col) =>
                {
                    Parallel.For(0, h, (row) =>
                    {
                        if (float.IsNaN(depth[row, col]))
                            ImageDepth.Set<byte>(row, col, 0);
                        else
                        {
                            var val = 255.0 * ((depth[row, col] - depthMin) / (depthMax - depthMin));
                            ImageDepth.Set<byte>(row, col, (byte)val);
                        }
                    });
                });
                return ImageDepth.Clone();
            }
        }
        public Mat GetRGB()
        {
            var sRGB = _frame.PointCloud.CopyImageSRGB();

            using (var mat = Mat.FromPixelData((int)sRGB.Height, (int)sRGB.Width, MatType.CV_8UC4, sRGB.DataPtr))
            {
                using (var image2d = mat.CvtColor(ColorConversionCodes.BGR2RGB))
                {
                    return image2d.Clone();
                }
            }
        }

        public void SavePly(string plyPath)
        {
            _frame.Save(plyPath);
        }

        public void Dispose()
        {
            _frame.Dispose();
        }

        public FrameSavedPaths SaveAll(string directoryPath, string id)
        {
            var paths = new FrameSavedPaths(directoryPath, id);

            var rgb = GetRGB();
            var normals = GetNormalMap();
            var depth = GetDepthMap();

            SavePly(paths.PlyPath);
            Cv2.ImWrite(paths.RGBImgPath, rgb);
            Cv2.ImWrite(paths.NormalsImgPath, normals);
            Cv2.ImWrite(paths.DepthImgPath, depth);

            return paths;
        }

        public ICameraMatrix GetCameraMatrix()
        {
            var matrix = Calibrator.EstimateIntrinsics(_frame);
            return new ZividCameraMatrix(matrix.CameraMatrix.FX, matrix.CameraMatrix.FY, matrix.CameraMatrix.CX, matrix.CameraMatrix.CY);
        }
    }
}
