using UnityEngine;
using Cinemachine;

public class RoomController : MonoBehaviour
{    
    [Header("Attributes")]
    [SerializeField] bool firstRoom;
    [SerializeField] bool noTimerRoom;
    public bool lockedCamera;
    [SerializeField] float completeTime; // The time the player has to complete the room

    [Header("Dependencies")]
    [SerializeField] Transform spawnpoint;
    [SerializeField] Transform enemiesParents;

    [Header("Editing")]
    public Vector2 topRightPoint;
    public Vector2 bottomLeftPoint;

    // Hiddem Variables
    [HideInInspector] public CinemachineVirtualCamera roomCamera;

    // Local Variables
    bool currentRoom;
    bool completed;
    
    // Initial Variables
    Transform player;
    BaseEnemy[] enemies;
    RoomExit[] roomExits;

    // Dependencies
    RoomManager roomManager;
    TimeManager timeManager;
    InputManager inputManager;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    void Start()
    {
        // Getting dependencies
        roomManager = RoomManager.Instance;
        timeManager = TimeManager.Instance;
        inputManager = InputManager.Instance;
        roomCamera = GetComponentInChildren<CinemachineVirtualCamera>();


        // Getting initial variables
        player = PlayerManager.Instance.player.transform;
        enemies = enemiesParents.GetComponentsInChildren<BaseEnemy>();
        roomExits = GetComponentsInChildren<RoomExit>();


        // The camera of this room should not be focus when starts scene
        roomCamera.Priority = 0;

        // The camera should be following the player
        // Unless the camera should not move
        if (!lockedCamera)
            roomCamera.m_Follow = player;


        // If this is the first room
        if (firstRoom)
        {
            if (player.gameObject.activeSelf)
            {
                // This room needs to activated himself
                Enter();

                // Put player at the first location
                player.transform.position = spawnpoint.position;
            }
            else
                // The room will be activated from the door
                roomManager.currentRoom = this;
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        // If player wants to restart the room
        // then restart the room
        if (inputManager.GetRestartPressed() && currentRoom && timeManager.timerRunning && !CameraTransition.Instance.transitioning)
            CameraTransition.Instance.TransitionReset();
    }


    public void Enter()
    {
        // When player enters the room, this is the current room
        currentRoom = true;


        // Enabling current room

        // Enabling all exits
        foreach (RoomExit roomExit in roomExits)
            roomExit.GetComponent<BoxCollider2D>().enabled = true;
        // Change camera priority for this camera to become the focus
        roomCamera.Priority = 10;


        // Update Room Manager

        // Telling to room manager this is the current room
        roomManager.currentRoom = this;


        // Should this room have a timer? 
        // (has been completed and should this room have a timer)
        if (!completed && !noTimerRoom)
        {        
            // Starting the timer with the room complete time
            timeManager.StartTimer(completeTime);
        
        
            return;
        }


        // Finish the timer, if it has not been completed
        timeManager.FinishTimer();
    }


    public bool Exit(RoomController nextRoom) 
    {   
        bool _completed = CheckCompletion();

        // If he's exiting to the previous room
        // then the level doesn't need to be completed
        if (nextRoom != roomManager.previousRoom && !_completed)
        {
            // Put everything from this room back in the original place
            CameraTransition.Instance.TransitionReset();

            // Return room has failed
            return false;
        }
        // If room has not been completed yet
        // but player returned then we still need to reset the enemies
        else if (!_completed)
            // Getting enemies back
            foreach (BaseEnemy enemy in enemies)
                enemy.gameObject.SetActive(true);


        // Marking room as completed

        // If the script reached this point than
        // the player finished the level and is going towards the next one
        // This level has been completed
        completed = _completed;


        // The room the player is exiting is now the previous room
        roomManager.previousRoom = this;


        // Disabling this room

        // When player leaves the room, this is not the current room anymore
        currentRoom = false;
        // Disabling all exits
        foreach (RoomExit roomExit in roomExits)
            roomExit.GetComponent<BoxCollider2D>().enabled = false;
        // Removing the priority from this camera for the new room to become the focus
        roomCamera.Priority = 0;


        return true;
    }

    
    public bool CheckCompletion()
    {
        // Has the player finished the level

        // Checking if all enemies had been killed
        foreach (BaseEnemy enemy in enemies)
            if (!enemy.isDead)
                // Has not completed the level yet, an enemy is alive
                return false;
        

        // If function reached this, then return true
        return true;
    }


    // For restarting the entire room, without reseting the scene
    public void Reset() 
    {
        // Getting player back

        // Player has not yet killed all the enemies
        // Return him to the room checkpoint
        player.transform.position = spawnpoint.position;
        // Reset variables that could cause an visual error
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponentInChildren<Animator>().Play("Idle");
        // Player should be able to move again, and not be dieing
        player.GetComponent<PlayerMovement>().cantMove = false;


        // The enemies and time should only get back, if player
        // still have not completed the level
        if (!completed)
        {
            // Getting enemies back
            foreach (BaseEnemy enemy in enemies)
                enemy.Reset();
        
            
            // Should be a timer in this room?
            if (!noTimerRoom)
                // Restarting time
                timeManager.StartTimer(completeTime);
        }
    }


    #region Editor Function's
    
    public void ResetRoomCameraBoundaries ()
    {
        // Temporary variables
        PolygonCollider2D _boundary = GetComponent<PolygonCollider2D>();
        Vector2[] _boundariesPoints = 
        {
            new Vector2(8.89f, 5), 
            new Vector2(-8.89f, 5), 
            new Vector2(-8.89f, -5), 
            new Vector2(8.89f, -5)
        }; // The default position for each point (the same as the size of the camera)


        // Actually setting the points
        _boundary.points = _boundariesPoints;
        
        topRightPoint = new Vector2(8.89f, 5);
        bottomLeftPoint = new Vector2(-8.89f, -5);
    }
    
    #endregion
}