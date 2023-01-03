using UnityEngine;

public class BaseEnemy : MonoBehaviour, IHittable
{
    [Header("General Attributes")]
    [SerializeField] float touchKnockback;
    [SerializeField] float deathKnockback;
    [SerializeField] float interactRange;
    [SerializeField] RoomController room;

    [Header("Visual")]
    [SerializeField] protected SpriteRenderer mainVisual;
    [SerializeField] SpriteRenderer deadVisual;
    [SerializeField] SpriteRenderer bloodSplashVisual;
    [SerializeField] SpriteRenderer weakenEffectVisual;
    [SerializeField] GameObject bloodEffect;

    [Header("Sounds")]
    [SerializeField] AudioSource deathSound;

    [Space]
    [Header("Movement Attributes")]
    // Attributes for checking if enemy is grounded
    [SerializeField] float     groundDistance;
    [SerializeField] Vector2   groundedSize;
    [SerializeField] LayerMask groundedLayer;

    [Space]
    [SerializeField] PhysicsMaterial2D defaultPhyshics;
    [SerializeField] PhysicsMaterial2D deadPhyshics;
    
    // Hidden variables
    [HideInInspector] public bool isDead;
    // Dependencies
    [HideInInspector] public GameObject player;

    // Local Variables
    Vector2 startPosition;
    float currentTouchKnockbackCooldown;

    // Initial variables
    Vector2 startGroundedSize;
    Vector2 startColliderSize;

    // Dependencies
    protected Animator anim;
    protected Rigidbody2D rb;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    public virtual void Start()
    {
        // Getting dependencies
        player = PlayerManager.Instance.player;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();


        // Getting initial variables
        startGroundedSize = groundedSize;
        startColliderSize = GetComponent<BoxCollider2D>().size;


        // Setting up start values
        startPosition = transform.position;
    }


    // Update is called every frame, if the MonoBehaviour is enabled.
    protected virtual void Update()
    {
        // Updating variables
        currentTouchKnockbackCooldown -= Time.deltaTime;


        // Start the main loop of the enemy
        // Player needs to not be dead
        if (!isDead) 
            Main();
        
        
        // Animation that also needs to work with the dead body
        if (!IsGrounded())
        {
            if (rb.velocity.y > 0)
                anim.Play("Jump");
            else
                anim.Play("Fall");
        }
        // If enemy is not dead other scripts should look into not grounded state 
        else if (isDead)
            anim.Play("Idle");
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
        deadVisual.flipX = !mainVisual.flipX;
        GetComponents<BoxCollider2D>()[0].size = startColliderSize * new Vector2(0.5f, 1);
        GetComponents<BoxCollider2D>()[1].size = startColliderSize * new Vector2(0.5f, 1);
        groundedSize.x = startGroundedSize.x / 2;
        rb.sharedMaterial = deadPhyshics;
        // Showing blood Splash on wall
        bloodSplashVisual.enabled = true;
        bloodSplashVisual.flipX = playerDirection < 0 ? true : false;
        bloodSplashVisual.transform.parent = null;
        // Not weaken effect anymore
        if (weakenEffectVisual != null) weakenEffectVisual.enabled = false;
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


        // Kill velocity
        rb.velocity = new Vector2((transform.position - player.transform.position).normalized.x * deathKnockback, 0);


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
        GetComponents<BoxCollider2D>()[0].size = startColliderSize;
        GetComponents<BoxCollider2D>()[1].size = startColliderSize;
        groundedSize.x = startGroundedSize.x;
        rb.sharedMaterial = defaultPhyshics;
        // Removing blood splash from wall
        bloodSplashVisual.enabled = false;
        // Show weaken effect
        if (weakenEffectVisual != null) weakenEffectVisual.enabled = true;
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

    #region Collision

    // Check if player is colliding with the ground
    protected bool IsGrounded() 
    {
        Vector2 _groundedPosition = transform.position + new Vector3(0, -groundDistance, 0);

        return Physics2D.OverlapBox(_groundedPosition, groundedSize, 0f, groundedLayer);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.name == "Player" && currentTouchKnockbackCooldown < 0)
        {
            int _deathMultiplier = isDead ? 2 : 1;
            rb.velocity +=  new Vector2((transform.position - player.transform.position).normalized.x * touchKnockback, 0) * _deathMultiplier;
        
            currentTouchKnockbackCooldown = 0.2f;
        }
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


        // Gizmos for on ground collision

        // Changing the color of the gizmos
        Gizmos.color = Color.blue;
        // Getting the position of the grounded collision
        Vector2 _groundedPosition = transform.position + new Vector3(0, -groundDistance, 0);
        // Drawing the grounded cube
        Gizmos.DrawWireCube(_groundedPosition, groundedSize);   
    }

    #endregion
}
