using UnityEngine;

// An script for holding the information of what it's the player
public class PlayerManager : MonoBehaviour
{
    #region Singleton
    
    static PlayerManager _instance;

    public static PlayerManager Instance 
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

    public GameObject player;
}
