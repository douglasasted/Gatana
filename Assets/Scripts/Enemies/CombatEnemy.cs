using System.Collections;
using UnityEngine;

public class CombatEnemy : BaseEnemy
{
    [Header("Combat Attributes")]
    [SerializeField] float attackRange;

    [Space]
    [Header("Movement Attributes")]
    [SerializeField] float speed;

    [Space]
    // Attributes for checking if enemy is grounded
    [SerializeField] float     groundDistance;
    [SerializeField] Vector2   groundedSize;
    [SerializeField] LayerMask groundedLayer;

    [Space]
    [Header("Dash Attributes")]
    [SerializeField] int   maxDashs;
    [SerializeField] float dashAngularDrag;
    [SerializeField] float dashTime;
    [SerializeField] float dashForce;
    [SerializeField, Range(0, 1)] float dashCut;

    [Space]
    [Header("Combat Sounds")]
    [SerializeField] AudioSource blockSound;

    // Hiding variables
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool rightAttack;

    // Local Variables
    int dashs;

    Vector2 dashVelocity;

    // Dependencies
    Animator anim;
    EnemyKatana katana;


    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    public override void Start()
    {
        base.Start();


        // Getting dependencies
        anim = GetComponent<Animator>();
        katana = GetComponentInChildren<EnemyKatana>();


        // Setting up start values
        dashs = maxDashs;
    }


    protected override void Main()
    {
        // Function only reaches here if player is on interaction range
        // and enemy is also not dead
        
        // Enemy should also not be dashing right now 
        if (isDashing)
        {
            rb.velocity = dashVelocity;
            

            // Direction in which the enemy is looking while dashing
            mainVisual.flipX = !rightAttack;

            return;
        }
        else if (katana.isAnticipating)
            return;


        base.Main();


        // Local variables
        int playerDirection = 0;


        // If enemy is grounded reset the dashes
        if (IsGrounded())
            dashs = maxDashs;


        // Is player in attack range?
        if (Vector2.Distance(player.transform.position, transform.position) <= attackRange)
        {
            // Player is in attack range

            // Sinalize player that attack is incoming
            // Attack player afterwards
            StartCoroutine(katana.AnticipateAttack((Vector2) transform.position - (Vector2) player.transform.position));
        }
        else
        {
            // Player is not in attack range

            // Move towards the player (to get in attack range)
            // Getting player's direction (the direction in which the enemy is going to move)
            playerDirection = player.transform.position.x > transform.position.x ? 1 : -1;
        }


        // Moving enemy in intended direction
        rb.velocity = new Vector2(playerDirection * speed, rb.velocity.y);
    }


    public override bool Hit()
    {
        // Enemy blocks attack if it's currently anticipating
        if (katana.isAnticipating)
        {
            // Block the hit sound
            blockSound.Play();


            // Don't continue the script
            return false;
        }


        base.Hit();
        

        // Stop katana from continuing attack after enemy has been killed
        katana.Reset();
        katana.katanaVisual.enabled = false;


        // Return false if enemy is dead, so that the katana doesn't
        // activate
        if (isDead)
            return false;

        return true;
    }


    public override void Reset()
    {
        base.Reset();


        // Reset katana if enemy is reseted
        katana.katanaVisual.enabled = true;


        // Stop dashing from continuing if it was happening
        isDashing = false;
        rb.angularDrag = 0;
        StopAllCoroutines();
        
        
        // Also stop katana from continuing
        katana.Reset();
    }


    #region Movement

    // Dash in the direction choosen
    // Mainly used by the katana script
    public IEnumerator Dash(Vector2 _direction) 
    {
        // Dash resource

        // If still has some dashs to use,
        // then enemy can dash
        if (dashs <= 0)
            yield break;
        // Updating variable
        dashs -= 1;


        // Shows for the rest of the script that the dashing is happening
        isDashing = true;


        // Movement

        // Angular drag
        rb.angularDrag = dashAngularDrag;
        dashVelocity = -_direction.normalized * dashForce;


        // Time that the dash will be occuring in game
        yield return new WaitForSeconds(dashTime);


        // Resetting variables / Stopping dash

        // The dash also works without this part, 
        // but without this the vertical movement is bigger than the horizontal
        // (Just cutting the dash velocity seems better than just stoping the movement entirely)
        rb.velocity *= dashCut;
        // Getting angular rigidbody variable back
        rb.angularDrag = 0;
        // Shows for the rest of the script that the dashing has been over
        isDashing = false;
    }

    #endregion

    #region Collision

    // Check if player is colliding with the ground
    bool IsGrounded() 
    {
        Vector2 _groundedPosition = transform.position + new Vector3(0, -groundDistance, 0);

        return Physics2D.OverlapBox(_groundedPosition, groundedSize, 0f, groundedLayer);
    }

    #endregion

    #region Debug

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Gizmos for attack range

        // Changing the color of the gizmos
        Gizmos.color = Color.red;
        // Drawing the attack circle
        Gizmos.DrawWireSphere(transform.position, attackRange);


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
