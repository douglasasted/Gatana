using UnityEngine;


public class TriggerTransition : MonoBehaviour
{
    [SerializeField] CameraTransition cameraTransition;


    public void TransitionScene(string scene)
    {
        cameraTransition.TransitionScene(scene);
    }
}
