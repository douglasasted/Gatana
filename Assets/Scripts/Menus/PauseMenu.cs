using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] SpriteRenderer cursor;
    [SerializeField] SpriteRenderer altCursor;

    [Space]

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject baseMenu;
    [SerializeField] GameObject settingsMenu;

    [Space]

    [SerializeField] AudioSource pressSound;

    // Local variables
    bool isPaused;
    bool locked;

    // Dependencies
    InputManager inputManager;
    PlayerMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        // Getting dependencies
        inputManager = InputManager.Instance;
        playerMovement = PlayerManager.Instance.player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.GetEscape())
            Pause();
    }

    public void Pause() 
    {
        if (locked)
            return;

            
        // Sound
        pressSound.Play();

        // Utility

        isPaused = !isPaused;

        cursor.enabled = !isPaused;
        altCursor.enabled = isPaused;

        Time.timeScale = isPaused ? 0 : 1;
        playerMovement.isPaused = isPaused;

        // Visual of the pause menu
        pauseMenu.SetActive(isPaused);
    }

    public void Settings() 
    {
        if (locked)
            return;


        // Sound
        pressSound.Play();

        // Utility
        baseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Back() 
    {
        if (locked)
            return;


        // Sound
        pressSound.Play();

        // Utility
        baseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void Exit() 
    {
        if (locked)
            return;


        // Sound
        pressSound.Play();

        // Utility
        locked = true;
        Time.timeScale = 1;
        CameraTransition.Instance.TransitionScene("Menu");
    }
}
