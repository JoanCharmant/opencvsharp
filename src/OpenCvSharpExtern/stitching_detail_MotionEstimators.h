#pragma once

// ReSharper disable IdentifierTypo
// ReSharper disable CppInconsistentNaming
// ReSharper disable CppNonInlineFunctionDefinitionInHeaderFile

#include "include_opencv.h"

// Estimator

CVAPI(ExceptionStatus) stitching_Estimator_apply(
    cv::detail::Estimator* obj,
    detail_ImageFeatures* features, int features_size,
    detail_MatchesInfo* pairwise_matches, int pairwise_matches_size,
    detail_CameraParams* cameras, int cameras_size,
    bool* out_ret)
{
    BEGIN_WRAP
    
    // Convert from detail_ImageFeatures* to std::vector<cv::detail::ImageFeatures>
    std::vector<cv::detail::ImageFeatures> featuresVec(features_size);
    for (int i = 0; i < features_size; i++)
    {
        cv::detail::ImageFeatures featuresCpp{
            features[i].img_idx,
            cpp(features[i].img_size),
            *features[i].keypoints,
            cv::UMat() };
        features[i].descriptors->copyTo(featuresCpp.descriptors);
        featuresVec[i] = featuresCpp;
    }

    // Convert from detail_MatchesInfo* to std::vector<cv::detail::MatchesInfo>
    std::vector<cv::detail::MatchesInfo> matchesVec(pairwise_matches_size);
    for (int i = 0; i < pairwise_matches_size; i++)
    {
        cv::detail::MatchesInfo matchesCpp;
        matchesCpp.src_img_idx = pairwise_matches[i].src_img_idx;
        matchesCpp.dst_img_idx = pairwise_matches[i].dst_img_idx;
        matchesCpp.matches = *pairwise_matches[i].matches;
        matchesCpp.inliers_mask = *pairwise_matches[i].inliers_mask;
        matchesCpp.num_inliers = pairwise_matches[i].num_inliers;
        matchesCpp.H = cv::Mat();
        pairwise_matches[i].H->copyTo(matchesCpp.H);
        matchesCpp.confidence = pairwise_matches[i].confidence;

        matchesVec[i] = matchesCpp;
    }

    // Convert from detail_CameraParams* to std::vector<cv::detail::CameraParams>
    std::vector<cv::detail::CameraParams> camerasVec(cameras_size);
    for (int i = 0; i < cameras_size; i++)
    {
        cv::detail::CameraParams camerasCpp;
        camerasCpp.focal = cameras[i].focal;
        camerasCpp.aspect = cameras[i].aspect;
        camerasCpp.ppx = cameras[i].ppx;
        camerasCpp.ppy = cameras[i].ppy;
        camerasCpp.R = cv::Mat();
        cameras[i].R->copyTo(camerasCpp.R);
        camerasCpp.t = cv::Mat();
        cameras[i].t->copyTo(camerasCpp.t);

        camerasVec[i] = camerasCpp;
    }

    *out_ret = (*obj)(featuresVec, matchesVec, camerasVec);

    // Copy results back.
    // into detail_CameraParams? 





    END_WRAP
}


// HomographyBasedEstimator

CVAPI(ExceptionStatus) stitching_HomographyBasedEstimator_new(
    bool is_focals_estimated,
    cv::detail::HomographyBasedEstimator** returnValue)
{
    BEGIN_WRAP
    * returnValue = new cv::detail::HomographyBasedEstimator(is_focals_estimated);
    END_WRAP
}

CVAPI(ExceptionStatus) stitching_HomographyBasedEstimator_delete(cv::detail::HomographyBasedEstimator* obj)
{
    BEGIN_WRAP
    delete obj;
    END_WRAP
}
