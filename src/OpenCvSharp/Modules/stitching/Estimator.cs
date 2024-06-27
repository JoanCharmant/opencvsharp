using OpenCvSharp.Internal;
using OpenCvSharp.Internal.Util;
using OpenCvSharp.Internal.Vectors;

namespace OpenCvSharp.Detail;

/// <summary>
/// Rotation estimator base class.
///
/// It takes features of all images, pairwise matches between all images and estimates rotations of all cameras.
/// 
/// The coordinate system origin is implementation-dependent, but you can always normalize the
/// rotations in respect to the first camera, for instance.
/// </summary>
public abstract class Estimator : DisposableCvObject
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ptr"></param>
    protected Estimator(IntPtr ptr)
        : base(ptr)
    {
    }

    /// <summary>
    /// Estimates camera parameters.
    /// </summary>
    /// <param name="features">Features of images</param>
    /// <param name="pairwiseMatches">Pairwise matches of images</param>
    /// <param name="cameras">Estimated camera parameters</param>
    /// <returns>True in case of success, false otherwise</returns>
    public virtual bool Apply(
        IEnumerable<ImageFeatures> features, 
        IEnumerable<MatchesInfo> pairwiseMatches, 
        IList<CameraParams> cameras)
    {
        ThrowIfDisposed();

        if (features is null)
            throw new ArgumentNullException(nameof(features));
        var featuresArray = features.CastOrToArray();
        if (featuresArray.Length == 0)
          throw new ArgumentException("Empty features array", nameof(features));

        if (pairwiseMatches is null)
          throw new ArgumentNullException(nameof(pairwiseMatches));
        var pairwiseMatchesArray = pairwiseMatches.CastOrToArray();
        if (pairwiseMatchesArray.Length == 0)
          throw new ArgumentException("Empty matches array", nameof(pairwiseMatches));

        var wImageFeatures = new WImageFeatures[featuresArray.Length];
        var wPairwiseMatches = new WMatchesInfo[pairwiseMatchesArray.Length];
        using var camerasVec = new VectorOfCameraParams(cameras);
        
        NativeMethods.HandleException(
            NativeMethods.stitching_Estimator_apply(
                ptr,
                wImageFeatures, wImageFeatures.Length,
                wPairwiseMatches, wPairwiseMatches.Length,
                camerasVec.CvPtr,
                out var ret));

        ClearAndAddRange(cameras, camerasVec.ToArray());
        
        GC.KeepAlive(this);

        return ret;
    }

    private static void ClearAndAddRange<T>(ICollection<T> list, IEnumerable<T> values)
    {
      list.Clear();
      foreach (var t in values)
      {
        list.Add(t);
      }
    }
}
