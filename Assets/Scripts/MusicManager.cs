using UnityEngine;


public class MusicManager : MonoBehaviour
{
    #region Singleton & DDOL
    
    static MusicManager _instance;

    public static MusicManager Instance 
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

        // Getting initial variables
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    #endregion


    [SerializeField] AudioClip[] musics;
    [SerializeField, Range(0, 1)] float volume;

    // Local Variables
    int currentMusic = -1;

    // Initial Variables
    AudioSource audioSource;
    Animator anim;


    // Initial variables are in Awake inside singleton

    void LateUpdate() 
    {
        audioSource.volume *= volume;
    }

    // Prepare to change the music, the animation will trigger the actual change
    public void PrepareChangeMusic(int _music) 
    {
        // Don't continue if it's the same music
        if (currentMusic == _music)
            return;

        // What music should be played
        currentMusic = _music;

        anim.Play("Change Music", -1, 0f);
    }

    // The actual change in the music from the animation
    public void ChangeMusic()
    {
        audioSource.Stop();
        
        // If current music = -1, then there should be no music
        if (currentMusic == -1)
            return;

        // Changing music
        audioSource.clip = musics[currentMusic];
        audioSource.Play();
    }
}
