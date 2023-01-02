using UnityEngine;


public class EnvironmentController : MonoBehaviour
{
    [SerializeField] int environmentMusic;


    void Start() 
    {
        // Change to menu music
        MusicManager.Instance.PrepareChangeMusic(environmentMusic);
    }
}
