using OpenCvSharp.Detail;
using OpenCvSharp.Internal.Util;

namespace OpenCvSharp.Internal.Vectors;

/// <summary> 
/// </summary>
public class VectorOfCameraParams : DisposableCvObject, IStdVector<CameraParams>
{
  /// <summary>
  /// Constructor
  /// </summary>
  public VectorOfCameraParams()
  {
    ptr = NativeMethods.vector_CameraParams_new1();
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="size"></param>
  public VectorOfCameraParams(nuint size)
  {
    if (size < 0)
      throw new ArgumentOutOfRangeException(nameof(size));
    ptr = NativeMethods.vector_CameraParams_new2(size);
  }

  /// <summary>
  /// Releases unmanaged resources
  /// </summary>
  protected override void DisposeUnmanaged()
  {
    NativeMethods.vector_CameraParams_delete(ptr);
    base.DisposeUnmanaged();
  }

  /// <summary>
  /// vector.size()
  /// </summary>
  public int Size
  {
    get
    {
      var res = NativeMethods.vector_CameraParams_getSize(ptr);
      GC.KeepAlive(this);
      return (int)res;
    }
  }

  /// <summary>
  /// &amp;vector[0]
  /// </summary>
  public IntPtr ElemPtr
  {
    get
    {
      var res = NativeMethods.vector_CameraParams_getPointer(ptr);
      GC.KeepAlive(this);
      return res;
    }
  }

  /// <summary>
  /// Converts std::vector to managed array
  /// </summary>
  /// <returns></returns>
  public CameraParams[] ToArray()
  {
    var size = Size;
    if (size == 0)
      return Array.Empty<CameraParams>();

    Mat[]? rs = null;
    Mat[]? ts = null;
    try
    {
      var nativeResult = new WCameraParams[size];
      rs = new Mat[size];
      ts = new Mat[size];
      for (int i = 0; i < size; i++)
      {
        rs[i] = new Mat();
        ts[i] = new Mat();
        nativeResult[i].R = rs[i].CvPtr;
        nativeResult[i].T = ts[i].CvPtr;
      }

      NativeMethods.vector_CameraParams_getElements(ptr, nativeResult);

      var result = new CameraParams[size];
      for (int i = 0; i < size; i++)
      {
        result[i] = new CameraParams(
            focal: nativeResult[i].Focal,
            aspect: nativeResult[i].Aspect,
            ppx: nativeResult[i].PpX,
            ppy: nativeResult[i].PpY,
            r: rs[i],
            t: ts[i]);
      }

      // ElemPtr is IntPtr to memory held by this object, so make sure we are not disposed until finished with copy.
      GC.KeepAlive(this);
      return result;
    }
    catch
    {
      if (rs is not null)
      {
        foreach (var mat in rs)
        {
          mat.Dispose();
        }
      }

      if (ts is not null)
      {
        foreach (var mat in ts)
        {
          mat.Dispose();
        }
      }

      throw;
    }
  }
}

