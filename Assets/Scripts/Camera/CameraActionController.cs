using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraActionController : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin perlin;
    [SerializeField] float intensity, time;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update() { }
    public void StartShaking()
    {
        CancelInvoke(nameof(StopShaking));
        perlin.m_AmplitudeGain = intensity;
        Invoke(nameof(StopShaking), time);
    }
    void StopShaking()
    {
        perlin.m_AmplitudeGain = 0f;
    }
}
