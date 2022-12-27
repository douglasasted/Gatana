using UnityEngine;

public class Spike : MonoBehaviour
{

    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player collides with the spike
        if (other.gameObject.layer == 12)
            // then reset the room
            CameraTransition.Instance.TransitionReset();
        // If the enemy collides with the spike
        else if (other.gameObject.layer == 10)
            // then kill the enemy
            other.GetComponent<BaseEnemy>().Hit();
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
