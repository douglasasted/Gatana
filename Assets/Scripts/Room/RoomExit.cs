using UnityEngine;

public class RoomExit : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] RoomController nextRoom;


    // Local Variables
    RoomController currentRoom;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    void Start()
    {
        currentRoom = GetComponentInParent<RoomController>();
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
