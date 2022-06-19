using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Local Variables

    // Depedencies
    RoomController room;

    // Start is called before the first frame update
    void Start()
    {
        room = GetComponentInParent<RoomController>();
    }

    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player collides with the spike
        // then reset the room
        if (other.name == "Player")
            room.Reset();
    }
}
