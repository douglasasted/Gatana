using UnityEngine;

public class Spike : MonoBehaviour
{
    // Local Variables

    // Depedencies
    RoomManager roomManager;


    // Start is called before the first frame update
    void Start()
    {
        // Getting Dependencies
        roomManager = RoomManager.Instance;
    }


    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player collides with the spike
        // then reset the room
        if (other.name == "Player")
            roomManager.currentRoom.Reset();
    }


    // Callback to draw gizmos that are pickable and always drawn.
    void OnDrawGizmos()
    {
        // Color of the gizmos
        Gizmos.color = Color.red;


        // Debug box, should not show up in game
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
