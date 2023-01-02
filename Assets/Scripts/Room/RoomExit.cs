using UnityEngine;
using UnityEngine.Events;


public class RoomExit : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] UnityEvent exitEvent;
    [SerializeField] RoomController nextRoom;

    [SerializeField] bool desactivateAttack;


    // Local Variables

    // Initial variables
    RoomController currentRoom;
    PlayerKatana katana;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    void Start()
    {
        // Getting initial variables
        currentRoom = GetComponentInParent<RoomController>();
        katana = PlayerManager.Instance.player.GetComponentInChildren<PlayerKatana>();
    }

    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If this is the player
        if (other.name == "Player")
        {
            // Then go to the next room
            
            // Exiting the current room 
            // If the result of the function is not true, 
            // then the player can't leave the room yet
            if (!currentRoom.Exit(nextRoom))
                return;


            // Event that should activate when player exits the room
            exitEvent.Invoke();


            // Entering in the new room
            nextRoom.Enter();
        }    
    }

    // Callback to draw gizmos that are pickable and always drawn.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
    }
}
