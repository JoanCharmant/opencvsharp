#pragma once

// ReSharper disable IdentifierTypo
// ReSharper disable CppInconsistentNaming
// ReSharper disable CppNonInlineFunctionDefinitionInHeaderFile

#include "include_opencv.h"

// Estimator


CVAPI(ExceptionStatus) stitching_Estimator_apply(
  cv::detail::Estimator* obj,
  detail_ImageFeatures* features, int featuresSize,
  detail_MatchesInfo* pairWiseMatches, int pairWiseMatchesSize,

  std::vector<cv::details::CameraParams>* cameras,
  bool* out_ret)
{
  BEGIN_WRAP
    std::vector<cv::detail::ImageFeatures> featuresVec(featuresSize);
    for (int i = 0; i < featuresSize; i++)
    {
      cv::detail::ImageFeatures featuresCpp{
          features[i].img_idx,
          cpp(features[i].img_size),
          *features[i].keypoints,
          cv::UMat() };
      features[i].descriptors->copyTo(featuresCpp.descriptors);
      featuresVec.push_back(featuresCpp);
    }

    // same for matches info.

    // std::vector<cv::detail::MatchesInfo> pairwise_matches;






    *out_ret = result.confidence;
    END_WRAP
}



  cv::detail::ImageFeatures features2Cpp{
      features2->img_idx,
      cpp(features2->img_size),
      *features2->keypoints,
      cv::UMat()
  };
  features1->descriptors->copyTo(features1Cpp.descriptors);
  features2->descriptors->copyTo(features2Cpp.descriptors);

  cv::detail::MatchesInfo result;
  (*obj)(features1Cpp, features2Cpp, result);

  *out_src_img_idx = result.src_img_idx;
  *out_dst_img_idx = result.dst_img_idx;
  std::copy(result.matches.begin(), result.matches.end(), std::back_inserter(*out_matches));
  std::copy(result.inliers_mask.begin(), result.inliers_mask.end(), std::back_inserter(*out_inliers_mask));
  *out_num_inliers = result.num_inliers;
  result.H.copyTo(*out_H);
  *out_confidence = result.confidence;
  END_WRAP
}
