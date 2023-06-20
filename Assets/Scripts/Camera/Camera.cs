using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin perlin;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {

    }
    public void StartShaking(float intensity, float time)
    {
        perlin.m_AmplitudeGain = intensity;
        Invoke(nameof(StopShaking), time);
    }
    void StopShaking()
    {
        perlin.m_AmplitudeGain = 0f;
    }
}
