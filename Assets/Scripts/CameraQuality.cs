using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraQuality : MonoBehaviour
{
    public int targetFrameRate = 90;
    public float resolutionScale = 0.5f;

    void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
        Screen.SetResolution(
            (int)(Display.main.systemWidth * resolutionScale),
            (int)(Display.main.systemHeight * resolutionScale),true);
    }
}