using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Local Variables

    // Dependencies
    InputManager inputManager;


    // Start is called before the first frame update
    void Start()
    {
        // Getting dependencies
        inputManager = InputManager.Instance;


        // Setting initial settings
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Cursor position on the world
        Vector2 _cursorPosition = Camera.main.ScreenToWorldPoint(inputManager.GetCursorPosition());


        // Change position of this object to cursor position
        transform.position = _cursorPosition;
    }
}
