using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        
        Camera cam = GetComponent<Camera>();//카메라 컴포넌트 가져오기

        
        Rect viewportRect = cam.rect; // 현재 카메라의 뷰포트 영역

        // 원하는 가로 세로 비율
        float screenAspectRatio = (float)Screen.width / Screen.height;
        float targetAspectRatio = 16f / 9f; // 원하는 고정 비율 설정

        // 화면 가로 세로 비율에 따라 뷰포트 영역을 조정
        if (screenAspectRatio < targetAspectRatio)
        {
            // 화면이 더 '높다'면 (세로가 더 길다면) 세로를 조절
            viewportRect.height = screenAspectRatio / targetAspectRatio;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }
        else
        {
            // 화면이 더 '넓다'면 (가로가 더 길다면) 가로를 조절
            viewportRect.width = targetAspectRatio / screenAspectRatio;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        // 조정된 뷰포트 영역을 카메라에 설정
        cam.rect = viewportRect;
    }
}
