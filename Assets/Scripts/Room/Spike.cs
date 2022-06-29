using UnityEngine;

public class Spike : MonoBehaviour
{

    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player collides with the spike
        // then reset the room
        if (other.name == "Player")
            // Restart the room
            CameraTransition.Instance.TransitionReset();
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
