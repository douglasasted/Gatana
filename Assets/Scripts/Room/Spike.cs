using UnityEngine;

public class Spike : MonoBehaviour
{
    // Local Variables

    // Depedencies
    RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
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
}
