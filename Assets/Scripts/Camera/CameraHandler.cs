using UnityEngine;


public class CameraHandler : MonoBehaviour
{
    [SerializeField] int cameraCut = 4;

    // Update is called once per frame
    void Update()
    {
        Vector3 _handlerPosition = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetCursorPosition()) / cameraCut;
        
        _handlerPosition.z = 0;

        transform.position = _handlerPosition; 
    }
}
