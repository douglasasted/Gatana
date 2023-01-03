using UnityEngine;


// SettingsManager and SettingsController are different
// SettingsManager controls the actual changes on the settings from the player and saves them
// SettingsController is where the player changes the settings that the player wants
public class SettingsManager : MonoBehaviour
{
    #region Singleton & DDOL
    
    static SettingsManager _instance;

    public static SettingsManager Instance 
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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
