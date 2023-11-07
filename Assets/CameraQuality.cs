using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraQuality : MonoBehaviour
{
    public float targetFrameRate = 90;
    public float resolutionScale = 0.5f;

    void Awake()
    {
        Application.targetFrameRate = 999;
        Screen.SetResolution(
            (int)(Screen.width * resolutionScale),
            (int)(Screen.height * resolutionScale),
            true
        );
    }
}