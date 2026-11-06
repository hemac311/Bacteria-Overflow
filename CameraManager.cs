using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    float zoomFactor = 3.0f;
    [SerializeField]
    float minZoomValue = 2.0f;
    [SerializeField]
    float maxZoomValue = 30.0f;

    public void ZoomIn() {
        if(Camera.main.orthographicSize - zoomFactor > minZoomValue) {
            Camera.main.orthographicSize = Camera.main.orthographicSize - zoomFactor;
        } else {
            Camera.main.orthographicSize = minZoomValue;
        }
    }

    public void ZoomOut() {
        if(Camera.main.orthographicSize + zoomFactor < maxZoomValue) {
            Camera.main.orthographicSize = Camera.main.orthographicSize + zoomFactor;
        } else {
            Camera.main.orthographicSize = maxZoomValue;
        }
    }
}
