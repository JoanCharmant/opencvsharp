using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace OpenCvSharp.Detail;

/// <summary>
/// Describes camera parameters.
/// </summary>
public sealed class CameraParams : IDisposable
{
    /// <summary>
    /// Focal length
    /// </summary>
    public double Focal { get; }

    /// <summary>
    /// Aspect ratio
    /// </summary>
    public double Aspect { get; }

    /// <summary>
    /// Principal point X
    /// </summary>
    public double PpX { get; }

    /// <summary>
    /// Principal point Y
    /// </summary>
    public double PpY { get; }

    /// <summary>
    /// Rotation
    /// </summary>
    public Mat R { get; }

    /// <summary>
    /// Translation
    /// </summary>
    public Mat T { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="focal"></param>
    /// <param name="aspect"></param>
    /// <param name="ppx"></param>
    /// <param name="ppy"></param>
    /// <param name="r"></param>
    /// <param name="t"></param>
    public CameraParams(
        double focal,
        double aspect,
        double ppx,
        double ppy,
        Mat r, 
        Mat t)
    {
        Focal = focal;
        Aspect = aspect;
        PpX = ppx;
        PpY = ppy;
        R = r;
        T = t;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="other"></param>
    public CameraParams(CameraParams other)
    {
        if (other is null)
            throw new ArgumentNullException(nameof(other));
        Focal = other.Focal;
        Aspect = other.Aspect;
        PpX = other.PpX;
        PpY = other.PpY;
        R = other.R;
        T = other.T;
    }
    
    // Allocate a new Mat on every call.
    // public Mat K()
    // {
            // Mat_<double> k = Mat::eye(3, 3, CV_64F);
            // k(0,0) = focal; k(0,2) = ppx;
            // k(1,1) = focal * aspect; k(1,2) = ppy;
            // return Mat(k);
    // }

    /// <summary>
    /// Dispose R and T
    /// </summary>
    public void Dispose()
    {
        R.Dispose();
        T.Dispose();
    }
}


#pragma warning disable 1591
[StructLayout(LayoutKind.Sequential)]
[SuppressMessage("Design", "CA1051: Do not declare visible instance fields")]
[SuppressMessage("Microsoft.Design", "CA1815: Override equals and operator equals on value types")]
public struct WCameraParams
{
  public double Focal;
  public double Aspect;
  public double PpX;
  public double PpY;
  public IntPtr R;
  public IntPtr T;
}
#pragma warning restore 1591

