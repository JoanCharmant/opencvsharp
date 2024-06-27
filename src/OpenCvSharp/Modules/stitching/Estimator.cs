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
        if (pairwiseMatches is null)
          throw new ArgumentNullException(nameof(pairwiseMatches));
    
    using var featuresVec = new VectorOfImageFeatures(features);
    using var pairwiseMatchesVec = new VectorOfMatchesInfo(pairwiseMatches);
    using var camerasVec = new VectorOfCameraParams(cameras);

    NativeMethods.HandleException(
          NativeMethods.stitching_Estimator_apply(
              ptr,
              featuresVec.CvPtr,
              pairwiseMatchesVec.CvPtr,
              camerasVec.CvPtr,
              out var ret));

      ClearAndAddRange(cameras, camerasVec.ToArray());
      
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
