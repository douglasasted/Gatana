using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{
    [Header("Visual")]
    [SerializeField] SpriteRenderer mainVisual;
    [SerializeField] SpriteRenderer deadVisual;
    [SerializeField] SpriteRenderer bloodSplashVisual;
    [SerializeField] GameObject bloodEffect;

    [Header("Sounds")]
    public AudioClip hitClip;
    
    // Hidden variables
    [HideInInspector] public bool isDead;

    // Local Variables

    // Dependencies
    GameObject player;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    void Start()
    {
        // Getting dependencies
        player = PlayerManager.Instance.player;
    }


    // Audio clip that player needs to play when enemy gets hitten    
    public AudioClip HitClip 
    { 
        get
        {
            return hitClip;
        }     
    }


    // What happens when the enemy gets hitten?
    public void Hit()
    {
        // Enemy is now dead
        isDead = true;


        // Blood Effect

        // Instantiate visual effect
        GameObject _bloodEffect = Instantiate(bloodEffect);
        // Changing blood effect position to current enemy position
        _bloodEffect.transform.position = transform.position;
        // In which direction is the player?
        int playerDirection = transform.position.x > player.transform.position.x ? 1 : -1;
        // Changing the direction the blood should be going
        _bloodEffect.transform.localScale = new Vector3(playerDirection, 1, 1);


        // Kill enemy
        
        // Desactivating the main look
        mainVisual.enabled = false;
        // The enemy should now be a body
        deadVisual.enabled = true;
        // Showing blood Splash on wall
        bloodSplashVisual.enabled = true;
        bloodSplashVisual.flipX = playerDirection < 0 ? true : false;
        // Should not have any collision anymore
        GetComponent<BoxCollider2D>().enabled = false;
    }


    public void Reset() 
    {
        // Enemy is not dead anymore
        isDead = false;


        // Back to default variables

        // Main visual should be enabled
        mainVisual.enabled = true;
        // The enemy should not be dead
        deadVisual.enabled = false;
        // Removing blood splash from wall
        bloodSplashVisual.enabled = false;
        // Collision should be enabled
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
