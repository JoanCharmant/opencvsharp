#pragma once

// ReSharper disable IdentifierTypo
// ReSharper disable CppInconsistentNaming
// ReSharper disable CppNonInlineFunctionDefinitionInHeaderFile

#include "include_opencv.h"

// Estimator

CVAPI(ExceptionStatus) stitching_Estimator_apply(
  cv::detail::Estimator* obj,
  const std::vector<cv::detail::ImageFeatures>* features,
  const std::vector<cv::detail::MatchesInfo>* pairwise_matches,
  std::vector<cv::detail::CameraParams>* cameras,
  bool* out_ret)
{
    BEGIN_WRAP
    *out_ret = (*obj)(*features, *pairwise_matches, *cameras);
    END_WRAP
}


// HomographyBasedEstimator

CVAPI(ExceptionStatus) stitching_HomographyBasedEstimator_new(
  bool is_focals_estimated,
  cv::detail::HomographyBasedEstimator** returnValue)
{
  BEGIN_WRAP
    * returnValue = new cv::detail::HomographyBasedEstimator(
      is_focals_estimated);
  END_WRAP
}

CVAPI(ExceptionStatus) stitching_HomographyBasedEstimator_delete(cv::detail::HomographyBasedEstimator* obj)
{
  BEGIN_WRAP
    delete obj;
  END_WRAP
}
