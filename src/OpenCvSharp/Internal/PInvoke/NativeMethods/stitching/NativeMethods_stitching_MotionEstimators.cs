using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using OpenCvSharp.Detail;

#pragma warning disable 1591
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable IDE1006 // Naming style

namespace OpenCvSharp.Internal;

static partial class NativeMethods
{
  
  // Estimator

  [Pure, DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
  public static extern ExceptionStatus stitching_Estimator_apply(
      IntPtr obj,
      WImageFeatures[] features, int featuresSize,
      WMatchesInfo[] matches, int matchesSize,
      WCameraParams[] cameras, int camerasSize,
      out bool ret);


  // HomographyBasedEstimator

  [Pure, DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
  public static extern ExceptionStatus stitching_HomographyBasedEstimator_new(
        bool isFocalsEstimated,
        out IntPtr returnValue);

  [Pure, DllImport(DllExtern, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
  public static extern ExceptionStatus stitching_HomographyBasedEstimator_delete(IntPtr obj);


}
