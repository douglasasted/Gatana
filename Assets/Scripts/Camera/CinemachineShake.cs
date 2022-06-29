using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    // Local Variables

    // Depedencies
    CinemachineVirtualCamera cinemachineCamera;


    // Start is called before the first frame update
    void Start()
    {
        // Getting dependencies
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator ShakeCamera (float intensity, float time) 
    {
        // Getting noise of the camera shake
        CinemachineBasicMultiChannelPerlin _channelPerlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        // Setting the intensity on camera shake
        _channelPerlin.m_AmplitudeGain = intensity;


        yield return new WaitForSeconds(time);
        
        
        // Returning intensity back to normal
        _channelPerlin.m_AmplitudeGain = 0;
    }
}
