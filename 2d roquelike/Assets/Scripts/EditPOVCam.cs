using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPOVCam : MonoBehaviour // изменение поля зрения у камеры
{
    Camera mainCamera;
    public float zoomAMT = 159f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.fieldOfView = zoomAMT;
    }

    public void sliderZoom(float zoom)
    {
        zoomAMT = zoom;
        //Camera.main.orthographic = false;
    }
}
