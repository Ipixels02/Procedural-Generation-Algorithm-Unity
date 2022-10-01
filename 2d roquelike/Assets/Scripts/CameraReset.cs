using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraReset : MonoBehaviour //сменя поля зрения камеры на перспективную
{
    //public int zoomAMT = 159;
    private Camera mainCamera;

    //void Start()
    //{
    //    mainCamera = GetComponent<Camera>();
    //}

    //void Update()
    //{
    //    mainCamera.fieldOfView = zoomAMT;
    //}

    public void Reset() 
    {
        Camera.main.orthographic = false;
        DestroyBoss();
        DestroyPlayer();
    }

    private void DestroyBoss() //уничтожаем босса
    {
        foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
        {
            DestroyImmediate(boss);
        }
    }

    private void DestroyPlayer() //унчитожаем объект игрока
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            DestroyImmediate(player);
        }
    }
}
