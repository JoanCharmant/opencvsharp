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

        var featuresArray = features.CastOrToArray();
        if (featuresArray.Length == 0)
            throw new ArgumentException("Empty features array", nameof(features));

        var matchesArray = pairwiseMatches.CastOrToArray();
        if (matchesArray.Length == 0)
            throw new ArgumentException("Empty matches array", nameof(pairwiseMatches));

        // Depending on the stage of the process the cameras list may be empty.
        var camerasArray = cameras.CastOrToArray();
        var cameraArrayLength = camerasArray == null ? 0 : camerasArray.Length;

        var keypointVecs = new VectorOfKeyPoint?[featuresArray.Length];
        var wImageFeatures = new WImageFeatures[featuresArray.Length];
        var dmatchesVecs = new VectorOfDMatch?[matchesArray.Length];
        var inliersMaskVecs = new VectorOfByte?[matchesArray.Length];
        var wMatchesInfos = new WMatchesInfo[matchesArray.Length];
        var wCameras = new WCameraParams[cameraArrayLength];
        try
        {
            // Convert from IEnumerable<ImageFeatures> to WImageFeatures[].
            for (int i = 0; i < featuresArray.Length; i++)
            {
                if (featuresArray[i].Descriptors is null)
                    throw new ArgumentException("features contain null descriptor mat", nameof(features));

                featuresArray[i].Descriptors.ThrowIfDisposed();
                keypointVecs[i] = new VectorOfKeyPoint(featuresArray[i].Keypoints);
                wImageFeatures[i] = new WImageFeatures
                {
                    ImgIdx = featuresArray[i].ImgIdx,
                    ImgSize = featuresArray[i].ImgSize,
                    Keypoints = keypointVecs[i]!.CvPtr,
                    Descriptors = featuresArray[i].Descriptors.CvPtr,
                };
            }

            // Convert from IEnumerable<MatchesInfo> to WMatchesInfo[].
            for (int i = 0; i < matchesArray.Length; i++)
            {
                dmatchesVecs[i] = new VectorOfDMatch(matchesArray[i].Matches);
                inliersMaskVecs[i] = new VectorOfByte(matchesArray[i].InliersMask);
                wMatchesInfos[i] = new WMatchesInfo
                {
                    SrcImgIdx = matchesArray[i].SrcImgIdx,
                    DstImgIdx = matchesArray[i].DstImgIdx,
                    Matches = dmatchesVecs[i]!.CvPtr,
                    InliersMask = inliersMaskVecs[i]!.CvPtr,
                    NumInliers = matchesArray[i].NumInliers,
                    H = matchesArray[i].H.CvPtr,
                    Confidence = matchesArray[i].Confidence,
                };
            }

            // Convert from IList<CameraParams> to WCameraParams[].
            for (int i = 0; i < cameraArrayLength; i++)
            {
                wCameras[i] = new WCameraParams
                {
                    Focal = camerasArray[i].Focal,
                    Aspect = camerasArray[i].Aspect,
                    PpX = camerasArray[i].PpX,
                    PpY = camerasArray[i].PpY,
                    R = camerasArray[i].R.CvPtr,
                    T = camerasArray[i].T.CvPtr,
                };
            }

            NativeMethods.HandleException(
                NativeMethods.stitching_Estimator_apply(
                    ptr,
                    wImageFeatures, wImageFeatures.Length,
                    wMatchesInfos, wMatchesInfos.Length,
                    wCameras, wCameras.Length,
                    out var ret
                ));

            //ClearAndAddRange(cameras, camerasVec.ToArray());

            return ret;
        }
        finally
        {
            foreach (var vec in keypointVecs)
            {
                vec?.Dispose();
            }
            foreach (var vec in dmatchesVecs)
            {
                vec?.Dispose();
            }
            foreach (var vec in inliersMaskVecs)
            {
                vec?.Dispose();
            }
            GC.KeepAlive(this);
        }
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
