using UnityEngine;

public class EntranceDoor : MonoBehaviour
{
    // Local Variables
    GameObject player;


    // Awake is called when the script instance is being loaded.
    void Start()
    {
        // Getting initial variables
        player = PlayerManager.Instance.player;


        // If this door exists than the player should not be visible at the start
        player.SetActive(false);
    }

    // This shows from the animation
    public void ShowPlayer() 
    {
        // Activate the first room
        RoomManager.Instance.currentRoom.Enter();


        // Set player active
        PlayerManager.Instance.player.SetActive(true);
    }
}