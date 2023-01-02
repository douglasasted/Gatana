using UnityEngine;
using UnityEngine.SceneManagement;


public class StatsManager : MonoBehaviour
{
    #region Singleton & DDOL
    
    static StatsManager _instance;

    public static StatsManager Instance 
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

        DontDestroyOnLoad(gameObject);

        _instance = this;
    }

    #endregion
    
    public int maxTokens;

    [Space]
    public int deaths;
    public int collectedTokens;
    public float runTime;

    [Space]
    public int leastDeaths = -1;
    public float bestRunTime = -1;
    public float bestTokens = -1;


    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update() 
    {
        // Getting run time of this run
        string _activeScene = SceneManager.GetActiveScene().name;

        // Only should be saving the time if it's on a run and not on a menu
        if (_activeScene != "Menu" && _activeScene != "Ending")
            runTime += Time.deltaTime;
    }


    public void SaveRun() 
    {
        // Saving deaths, if it's less then current
        if (leastDeaths == -1 || deaths < leastDeaths)
            leastDeaths = deaths;

        // Saving current time, if it's less then current
        if (bestRunTime == -1 || runTime < bestRunTime)
            bestRunTime = runTime;

        // Saving tokens, if it's more then current
        if (bestTokens == -1 || collectedTokens > bestTokens)
            bestTokens = collectedTokens;

        deaths = 0;
        runTime = 0;
        collectedTokens = 0;
    }


    public string ConvertToMinutesSeconds(float time)
    {
        float _minutes = Mathf.FloorToInt(time / 60);
        float _seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", _minutes, _seconds);
    }
}
