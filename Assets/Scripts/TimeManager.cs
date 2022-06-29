using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    
    static TimeManager _instance;

    public static TimeManager Instance 
    {
        get 
        {
            return _instance;
        }
    }

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    #endregion

    [Header("Visual")]
    [SerializeField] GameObject timeBar;
    [SerializeField] RectTransform timeSlider;

    // Hidden variables
    [HideInInspector] public float maxTime;

    [HideInInspector] public bool timerRunning;

    // Local variables
    float currentTime;

    // Getting dependencies
    RoomManager roomManager;


    // Start is called before the first frame update
    void Start()
    {
        // When the scene starts there should no bar up
        timeBar.SetActive(false);
    

        // Getting dependencies
        roomManager = RoomManager.Instance;
    }


    // Update is called once per frame
    void Update()
    {
        // If the time is running, updated it
        if (timerRunning)
        {
            // Updating the timer float
            currentTime -= Time.deltaTime;


            // Visual
            // Show the time visually on the screen via the time bar

            // Getting the position the time bar should be this frame
            // in relation to the currentTime x maxTime
            float _timeBarPosition = -timeBar.GetComponent<RectTransform>().sizeDelta.x * (1 - (currentTime / maxTime));
            // putting the position
            timeSlider.offsetMax = new Vector2(_timeBarPosition, 0);


            // If times is over
            if (currentTime < 0)
                // Restart the room
                CameraTransition.Instance.TransitionReset();
        }
    }


    public void StartTimer(float _maxTime) 
    {
        // Timer is running
        timerRunning = true;


        // When the scene starts, active timer
        timeBar.SetActive(true);


        // Updating the time to match the current room time
        maxTime = _maxTime;
        currentTime = _maxTime;
    }

    
    public void FinishTimer() 
    {
        // Timer is not running anymore
        timerRunning = false;


        // Showing time bar on screen
        timeBar.SetActive(false);
    }
}