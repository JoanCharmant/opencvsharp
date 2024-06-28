using OpenCvSharp.Detail;
using OpenCvSharp.Internal.Util;

namespace OpenCvSharp.Internal.Vectors;

/// <summary> 
/// </summary>
public class VectorOfMatchesInfo : DisposableCvObject, IStdVector<MatchesInfo>
{
  /// <summary>
  /// Constructor
  /// </summary>
  public VectorOfMatchesInfo()
  {
    ptr = NativeMethods.vector_MatchesInfo_new1();
  }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="size"></param>
  public VectorOfMatchesInfo(nuint size)
  {
    if (size < 0)
      throw new ArgumentOutOfRangeException(nameof(size));
    ptr = NativeMethods.vector_MatchesInfo_new2(size);
  }

  /// <summary>
  /// Releases unmanaged resources
  /// </summary>
  protected override void DisposeUnmanaged()
  {
    NativeMethods.vector_MatchesInfo_delete(ptr);
    base.DisposeUnmanaged();
  }

  /// <summary>
  /// vector.size()
  /// </summary>
  public int Size
  {
    get
    {
      var res = NativeMethods.vector_MatchesInfo_getSize(ptr);
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
      var res = NativeMethods.vector_MatchesInfo_getPointer(ptr);
      GC.KeepAlive(this);
      return res;
    }
  }

  /// <summary>
  /// Converts std::vector to managed array
  /// </summary>
  /// <returns></returns>
  public MatchesInfo[] ToArray()
  {
    var size = Size;
    if (size == 0)
      return Array.Empty<MatchesInfo>();

    VectorOfDMatch[]? matchesVecs = null;
    VectorOfByte[]? inliersMaskVecs = null;
    Mat[]? homographies = null;
    try
    {
      var nativeResult = new WMatchesInfo[size];
      matchesVecs = new VectorOfDMatch[size];
      inliersMaskVecs = new VectorOfByte[size];
      homographies = new Mat[size];
      for (int i = 0; i < size; i++)
      {
        matchesVecs[i] = new VectorOfDMatch();
        inliersMaskVecs[i] = new VectorOfByte();
        homographies[i] = new Mat();
        nativeResult[i].Matches = matchesVecs[i].CvPtr;
        nativeResult[i].InliersMask = inliersMaskVecs[i].CvPtr;
        nativeResult[i].H = homographies[i].CvPtr;
      }

      NativeMethods.vector_MatchesInfo_getElements(ptr, nativeResult);

      var result = new MatchesInfo[size];
      for (int i = 0; i < size; i++)
      {
        result[i] = new MatchesInfo(
            srcImgIdx: nativeResult[i].SrcImgIdx,
            dstImgIdx: nativeResult[i].DstImgIdx,
            matches: matchesVecs[i].ToArray(),
            inliersMask: inliersMaskVecs[i].ToArray(),
            numInliers: nativeResult[i].NumInliers,
            h: homographies[i],
            confidence: nativeResult[i].Confidence);
      }

      // ElemPtr is IntPtr to memory held by this object, so make sure we are not disposed until finished with copy.
      GC.KeepAlive(this);
      return result;
    }
    catch
    {
      if (homographies is not null)
      {
        foreach (var mat in homographies)
        {
          mat.Dispose();
        }
      }

      throw;
    }
    finally
    {
#pragma warning disable CA1508 // (???) Avoid dead conditional code
      if (matchesVecs is not null)
      {
        foreach (var vec in matchesVecs)
        {
          vec.Dispose();
        }
      }

      if (inliersMaskVecs is not null)
      {
        foreach (var vec in inliersMaskVecs)
        {
          vec.Dispose();
        }
      }

#pragma warning restore CA1508
    }
  }
}

