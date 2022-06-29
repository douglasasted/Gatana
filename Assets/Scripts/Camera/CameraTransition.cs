using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    #region Singleton

    // Variable that holds which one is the original instance
    static CameraTransition _instance;


    // Variable from which other script get this singleton
    public static CameraTransition Instance 
    {
        get 
        {
            return _instance;
        }
    }


    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Destroy the game object if the instance is not null and is not this script
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }    


        // If this object has not been destroyed yet than this is the the singleton
        _instance = this;
    }

    #endregion

    [Header("Sound")]
    [SerializeField] AudioSource deathSound;

    // Hidden variables
    [HideInInspector] public bool transitioning;

    // Local Variables
    string scene;

    // Initial Variables
    GameObject player;

    // Dependencies
    Animator anim;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    void Start()
    {
        // Getting dependencies
        anim = GetComponent<Animator>();


        // Getting initial variables
        player = PlayerManager.Instance.player;
    }


    // Start the transtion towards scene
    public void TransitionScene (string _scene)
    {
        // Play the fade out animation
        anim.Play("FadeOut");

        
        // Save the scene that will be loaded
        scene = _scene;
    }


    // Reset scene with transition
    public void TransitionReset () 
    {
        // Death sound
        deathSound.Play();

        
        // For other classes to know this script is transtioning
        transitioning = true;


        // Play the fade in death animation
        anim.Play("FadeIn Death");


        // If there is a cinemachine confiner
        if (RoomManager.Instance.currentRoom.roomCamera.GetComponent<CinemachineConfiner2D>() != null)
            // No damping, or the camera will shake too much during this part
            RoomManager.Instance.currentRoom.roomCamera.GetComponent<CinemachineConfiner2D>().m_Damping = 0;


        // Player should not be moving while death occurs
        player.GetComponent<Animator>().Play("Death");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        player.GetComponent<PlayerMovement>().cantMove = true;
    }


    // Function for when the death transtion ends
    public void TransitionFinished() 
    {
        // For other classes to know this script is not transtioning anymore
        transitioning = false;


        // If there is a cinemachine confiner
        if (RoomManager.Instance.currentRoom.roomCamera.GetComponent<CinemachineConfiner2D>() != null)
            // Resetting the damping back to normal
            RoomManager.Instance.currentRoom.roomCamera.GetComponent<CinemachineConfiner2D>().m_Damping = 0.5f;


        // Getting player back to default
        player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<PlayerMovement>().gravityScale;
    }


    // Change scene function
    public void ChangeScene() 
    {
        SceneManager.LoadScene(scene);
    }


    // Reset current room
    public void Reset ()
    {
        // Play the fade out death animation
        anim.Play("FadeOut Death");


        // Reset the current room
        RoomManager.Instance.currentRoom.Reset();
    } 
}
