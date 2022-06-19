using UnityEngine;

public class RoomManager : MonoBehaviour
{
    #region Singleton
    
    static RoomManager _instance;

    public static RoomManager Instance 
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


    [HideInInspector] public RoomController currentRoom;
    [HideInInspector] public RoomController previousRoom;
}
