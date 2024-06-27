using System;
using OpenCvSharp.Internal;

namespace OpenCvSharp.Detail;

/// <summary>
/// Homography based rotation estimator.
/// </summary>
public class HomographyBasedEstimator : Estimator
{
  /// <summary>
  /// Constructs a homography-based rotation estimator.
  /// </summary>
  /// <param name="isFocalsEstimated"></param>
  public HomographyBasedEstimator(bool isFocalsEstimated = false)
    : base(Create(isFocalsEstimated))
  {
  }

  private static IntPtr Create(
        bool isFocalsEstimated)
  {
    NativeMethods.HandleException(
        NativeMethods.stitching_HomographyBasedEstimator_new(
            isFocalsEstimated,
            out var ptr));
    return ptr;
  }

  /// <summary>
  /// releases unmanaged resources
  /// </summary>
  protected override void DisposeUnmanaged()
  {
    if (ptr != IntPtr.Zero)
    {
      NativeMethods.HandleException(
          NativeMethods.stitching_HomographyBasedEstimator_delete(ptr));
      ptr = IntPtr.Zero;
    }
  }
}
