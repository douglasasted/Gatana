using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomDoor : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] string nextScene;

    // Local Variables
    RoomController room;
    bool playerClose;

    // Dependencies
    InputManager inputManager;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    void Start()
    {
        // Getting dependencies
        inputManager = InputManager.Instance;


        // Getting initial variables
        room = transform.parent.GetComponent<RoomController>();
    }


    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        // If player is close and he wants to interact
        if (playerClose && inputManager.GetInteract() && room.CheckCompletion())
            SceneManager.LoadScene(nextScene);
    }


    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If player is close to the door, then mark player as close
        if (other.name == "Player")
            playerClose = true;
    }


    // Sent when another object leaves a trigger collider attached to
    // this object (2D physics only).
    void OnTriggerExit2D(Collider2D other)
    {
        // If player is close to the door, then mark player as close
        if (other.name == "Player")
            playerClose = true;
    }
}
