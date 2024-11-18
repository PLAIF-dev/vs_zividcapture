# vs_zividcapture
Zivid 데이터 촬영 프로그램(Windows Application) 입니다<br>
[사용법](https://www.notion.so/plaif/142ba69e7a8780a083b0ed3201c97d2c) <br>
<br>
Dependencies
 - .NET 8.0 WPF
 - [Zivid SDK 2.12.0](https://downloads.zivid.com/sdk/releases/2.12.0+6afd4961-1/windows/ZividSetup_2.12.0+6afd4961-1.exe) <- 설치 필수~~!!!!!
<br>

# 데이터 형식
## Depth Map
```C#
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
```

## RGB
```C#
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
```

## Normal
```C#
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
```
