using UnityEngine;

public class BaseEnemy : MonoBehaviour, IHittable
{
    [Header("General Attributes")]
    [SerializeField] float interactRange;
    [SerializeField] RoomController room;

    [Header("Visual")]
    [SerializeField] protected SpriteRenderer mainVisual;
    [SerializeField] SpriteRenderer deadVisual;
    [SerializeField] SpriteRenderer bloodSplashVisual;
    [SerializeField] GameObject bloodEffect;

    [Header("Sounds")]
    [SerializeField] AudioSource deathSound;
    
    // Hidden variables
    [HideInInspector] public bool isDead;
    // Dependencies
    [HideInInspector] public GameObject player;

    // Local Variables
    Vector2 startPosition;

    // Dependencies
    protected Rigidbody2D rb;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    public virtual void Start()
    {
        // Getting dependencies
        player = PlayerManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();


        // Setting up start values
        startPosition = transform.position;
    }


    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        // Start the main loop of the enemy
        // Player needs to be close and enemy needs to not be dead
        if (!isDead && PlayerOnRange()) 
            Main();
        else
            rb.velocity *= new Vector2(0, 1);
    }


    protected virtual void Main() 
    {
        // Enemy needs to face the player direction

        // The enemy is being flipped when the player x is greater than he's x
        // meaning they are to the the right of the player
        mainVisual.flipX = player.transform.position.x > transform.position.x ? false : true;
    }


    // What happens when the enemy gets hitten?
    public virtual bool Hit()
    {
        // If enemy is already dead, it should not be able to die again
        if (isDead)
            return false;


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
        bloodSplashVisual.transform.parent = null;
        // Should not have any collision anymore
        GetComponent<BoxCollider2D>().enabled = false;
        // If enemy is in a room and all other enemy are dead, then signal to player
        if (room != null && room.CheckCompletion())
        {
            if (room.endArrow != null) room.endArrow.SetActive(true);
            room.completionSound.Play();
        }
        // Sound
        deathSound.Play();


        // Finishing hit
        return true;
    }


    public virtual void Reset() 
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
        // Reset enemy back to start position and reset velocity
        transform.position = startPosition;
        rb.velocity = Vector2.zero;
        // Collision should be enabled
        GetComponent<BoxCollider2D>().enabled = true;
    }

    #region Utility

    public bool PlayerOnRange() 
    {
        return Vector2.Distance(player.transform.position, transform.position) <= interactRange;
    }

    #endregion


    #region Debug

    // Callback to draw gizmos that are pickable and always drawn.
    protected virtual void OnDrawGizmos()
    {
        // Gizmos for interact range

        // Changing the color of the gizmos
        Gizmos.color = Color.blue;
        // Drawing the attack circle
        Gizmos.DrawWireSphere(transform.position, interactRange);    
    }

    #endregion
}
