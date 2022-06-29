using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement Attributes")]
    [SerializeField] float speed;
    [SerializeField] float airSpeed;

    [Space]


    [Header("Jump Attributes")]
    [SerializeField] float              jumpForce;
    [SerializeField, Range(0, 1)] float jumpCut;

    [Space]

    [SerializeField] float wallCoyoteTime;
    [SerializeField] float wallJumpForce;
    [SerializeField] float wallJumpCooldown;
    [SerializeField] float wallJumpLerp;
    
    [Space]

    [SerializeField] float bufferTime;
    [SerializeField] float coyoteTime;

    [Space]

    // Attributes for checking if player is grounded
    [SerializeField] float     groundDistance;
    [SerializeField] Vector2   groundedSize;
    [SerializeField] LayerMask groundedLayer;

    [Space]

    // Attributes for checking if player is grounded
    [SerializeField] float     onWallDistance;
    [SerializeField] Vector2   onWallSize;

    [Space]

    [SerializeField] float apexThreshold = 0.25f;
    [SerializeField] float apexBonus;
    [SerializeField] float apexAntiGravity;

    [Space]

    [SerializeField] float fallVelocityLimit;

    [Header("Dash Attributes")]
    [SerializeField] int maxDashs;
    [SerializeField] float dashAngularDrag;
    [SerializeField] float dashTime;
    [SerializeField] float dashForce;
    [SerializeField, Range(0, 1)] float dashCut;

    [Space]

    [Header("Visual")]
    [SerializeField] SpriteRenderer mainVisual;
    [SerializeField] ParticleSystem dustEffect;

    [Space]

    [Header("Sound")]
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource climbSound;
    
    [Space]

    [SerializeField] AudioSource stepSound;
    [SerializeField] AudioClip[] stepClips;
    [SerializeField] float stepSoundCooldown;

    // Hidden variables
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool cantMove;

    [HideInInspector] public float gravityScale;

    // Local variables
    float currentBufferTime;
    float currentCoyoteTime;
    float currentWallCoyoteTime;
    float currentWallJumpCooldown;
    float stepSoundCurrentCooldown;

    bool wallJumped;
    bool climbingInput = false;

    int dashs;

    // Dependencies
    Animator anim;
    Rigidbody2D rb;
    InputManager inputManager;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // Getting dependencies
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        inputManager = InputManager.Instance;

        // Setting up initial variables
        gravityScale = rb.gravityScale;
    }


    // Update is called once per frame
    void Update()
    {        
        // If player is currently dashing
        // than he should not be in control until it ends
        if (isDashing)
            return;


        // If player should not move, remove his velocity and return
        if (cantMove)
        {
            // The player should not be dieing currently
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                // No other animation should be playing while the player can't move
                anim.Play("Idle");


            rb.velocity *= new Vector2(0.2f, 1);
            return;
        }


        // Getting inputs
        float _horizontalInput = inputManager.GetMovement();
        if (inputManager.GetClimbPressed())
            climbingInput = true;
        else if (inputManager.GetClimbReleased())
            climbingInput = false;


        // Jump Buffer

        // Updating the variable
        currentBufferTime -= Time.deltaTime;
        // If player pressed the jump button then the jump 
        // should be queued for some time
        if (inputManager.GetJumpPressed())
            currentBufferTime = bufferTime;


        // Coyote Time

        // Updating the variable
        currentCoyoteTime -= Time.deltaTime;
        // If player leaves groundes he can still jumps for a few frames
        if (IsGrounded())
        {
            // Player was not grounded before, then
            // play dust effect for the landing
            if (currentCoyoteTime <= 0)
                // Dust effect
                dustEffect.Play();


            // Setting the grounded variables
            currentCoyoteTime = coyoteTime;
        

            // Reset variables
            wallJumped = false;
            dashs = maxDashs;
        }


        // Wall Jump

        // Updating variables
        currentWallCoyoteTime -= Time.deltaTime;
        // If player wants to jump and was the wall: then jump
        if (currentBufferTime > 0 && currentWallCoyoteTime > 0)
            // Wall Jump
            Jump(-IsOnWall());


        // Wall Slide

        // Updating variables
        currentWallJumpCooldown -= Time.deltaTime;
        // If is on the wall and not on the ground then wall slide
        if (IsOnWall() != 0 && !IsGrounded() && currentWallJumpCooldown <= 0)
        {
            // Animation

            // Is on wall and not grounded
            anim.Play("Wall");
            // In which direction should the player be facing?
            mainVisual.flipX = IsOnWall() > 0 ? true : false;


            // The player should fall more slowly when on the wall
            rb.velocity = new Vector2(0, -0.5f);


            // Stop climbing if player wants to exit
            if (IsOnWall() != _horizontalInput)
                currentWallJumpCooldown = wallJumpCooldown;

            
            // Player is on wall,
            currentWallCoyoteTime = wallCoyoteTime;

            return;
        }


        // Player Jump Height Control

        // The player cannot wall jump to perfom this
        if (!wallJumped)
        {
            // If the player is going up, and he released the jump button
            // then cut the velocity of the jump
            // this gives more control to the player for when he wants to make the jump
            if (rb.velocity.y > 0 && inputManager.GetJumpReleased())
                rb.velocity *= new Vector2(1, jumpCut);
            // If player released button before the jump started then cancel the jump
            else if (currentBufferTime > 0 && inputManager.GetJumpReleased())
                currentBufferTime = 0;
        }

    
        // Apex Modifier
    
        // How close is the player to the apex of the jump?
        // If is not grounded apex should not even be considerered
        float _apexPoint = IsGrounded() ? 0 : Mathf.InverseLerp(apexThreshold, 0, Mathf.Abs(rb.velocity.y));
        // Bonus to horizontal movement, makes player able to land 
        // on the location they want more easily
        float _apexBonus = _horizontalInput * apexBonus * _apexPoint;
        // Give a little bit more time for the player at the apex of the jump
        // which also should help him decide where he should land better
        rb.gravityScale = gravityScale - Mathf.Lerp(0, apexAntiGravity, _apexPoint);
        

        // If player wants to jump and is on the ground: then jump
        if (currentBufferTime > 0 && currentCoyoteTime > 0)
            Jump();

        
        // Speed if not grounded is different
        float _speed = IsGrounded() ? speed : airSpeed;
        // Giving the velocity the player should be going to the rigidbody 2D
        float _horizontalVelocity = _horizontalInput * _speed + _apexBonus;
        // If player just wall jumped, then they should not have total control
        if (wallJumped && currentWallJumpCooldown >= 0)
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, _horizontalVelocity, wallJumpLerp * Time.deltaTime), rb.velocity.y);
        // Player normal velocity
        else
            rb.velocity = new Vector2(_horizontalVelocity, rb.velocity.y);


        // Step sound

        // If player is moving play the sound of player's steps
        if (_horizontalInput != 0 && stepSoundCurrentCooldown <= 0 && IsGrounded())
        {
            // Get a random step
            stepSound.clip = stepClips[Random.Range(0, 2)];
            // Play the random step
            stepSound.Play();


            // Wait some time before the next step sound
            stepSoundCurrentCooldown = stepSoundCooldown;
        }
        // Update timer variable
        stepSoundCurrentCooldown -= Time.deltaTime;


        // Limiting the fall speed
        // If there's no limit to the fall velocity, than the player has no control to where they lands
        if (rb.velocity.y < -fallVelocityLimit)
            rb.velocity = new Vector2(rb.velocity.x, -fallVelocityLimit);


        // Animations

        // Is the player on wall?
        if (IsOnWall() != 0)
        {
            // If player is not grounded there's a different animation
            if (!IsGrounded())
                anim.Play("Wall");
            else
                anim.Play("Wall Grounded");
            // In which direction should the player be facing?
            mainVisual.flipX = IsOnWall() > 0 ? true : false;
        }
        // If player is not on the ground
        else if (!IsGrounded())
        {
            // If player is going up
            if (rb.velocity.y > 0)
                anim.Play("Jump");
            // If player is going down
            else
                anim.Play("Fall");
        }
        // Is the player trying to move?
        else if (_horizontalInput != 0)
        {
            // In which direction is the character facing?
            int _direction = mainVisual.flipX ? -1 : 1;
            // In which direction is the character going?
            // give the animation from that
            if (_direction == _horizontalInput)
                // Direction is the same, then walk forward
                anim.Play("Walk");
            else
                // Direction is different, then walk backwards
                anim.Play("Walk Backwards");
        }
        // If nothing else, the player is not moving
        else
            anim.Play("Idle");
    }


    void Jump(float direction = 0) 
    {
        // Local variable for getting the current velocity + the jump force
        Vector2 _velocity = new Vector2(rb.velocity.x, jumpForce);


        // Resetting variables
        currentBufferTime = 0;
        currentCoyoteTime = 0;
        currentWallCoyoteTime = 0;


        // Dust effect
        dustEffect.Play();


        // Sound
        jumpSound.Play();


        // Wall Jump

        // If the direction is not 0, then this is a wall jump
        if (direction != 0)
        {
            // Setting variables
            wallJumped = true;
        
            
            // Changing the x velocity to the wall jump
            _velocity.x = wallJumpForce * direction;
        }
        // Even if this is not a wall jump 
        // it should still have an cooldown for entering in a wall jump
        currentWallJumpCooldown = wallJumpCooldown;


        // Giving the velocity the player should be going to the rigidbody 2D
        rb.velocity = _velocity;
    }


    // Dash in the direction choosen
    // Mainly used by the katana script
    public IEnumerator Dash(Vector2 _direction) 
    {
        // Dash resource

        // If still has some dashs to use,
        // then player can dash
        if (dashs <= 0)
            yield break;
        // Updating variable
        dashs -= 1;


        // Shows for the rest of the script that the dashing is happening
        isDashing = true;


        // Movement

        // Angular drag
        rb.angularDrag = dashAngularDrag;
        rb.velocity = -_direction.normalized * dashForce;


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


    // Reset dashes to max dashs
    public void ResetDash() 
    {
        dashs = maxDashs;
    } 


    #region Collision

    // Check if player is colliding with the ground
    bool IsGrounded() 
    {
        Vector2 _groundedPosition = transform.position + new Vector3(0, -groundDistance, 0);

        return Physics2D.OverlapBox(_groundedPosition, groundedSize, 0f, groundedLayer);
    }


    // Check if player is colliding with the wall
    public int IsOnWall() 
    {
        // Getting the position that the collision of the wall should be
        Vector2 _leftOnWallPosition = transform.position + new Vector3(-onWallDistance, 0, 0);
        Vector2 _rightOnWallPosition = transform.position + new Vector3(onWallDistance, 0, 0);

        // Getting the collision for both sides of the player
        bool _leftWall = Physics2D.OverlapBox(_leftOnWallPosition, onWallSize, 0f, groundedLayer);
        bool _rightWall = Physics2D.OverlapBox(_rightOnWallPosition, onWallSize, 0f, groundedLayer);

        // The variables that holds in which side the wall is
        int _side = 0;

        // Setting the variable to the correct side
        _side += _rightWall ? 1 : 0;
        _side += _leftWall ? -1 : 0;

        // Checking the collision
        return _side;
    }

    #endregion


    #region Debug

    // Callback to draw gizmos that are pickable and always drawn.
    void OnDrawGizmos()
    {
        // Gizmos for on ground collision

        // Changing the color of the gizmos
        Gizmos.color = Color.blue;
        // Getting the position of the grounded collision
        Vector2 _groundedPosition = transform.position + new Vector3(0, -groundDistance, 0);
        // Drawing the grounded cube
        Gizmos.DrawWireCube(_groundedPosition, groundedSize);    
    
        
        // Gizmos for on wall collision

        // Changing the color of the gizmos
        Gizmos.color = Color.red;
        // Getting the position for the left and right wall collision
        Vector2 _leftOnWallPosition = transform.position + new Vector3(-onWallDistance, 0, 0);
        Vector2 _rightOnWallPosition = transform.position + new Vector3(onWallDistance, 0, 0);
        // Drawing both on wall cube
        Gizmos.DrawWireCube(_leftOnWallPosition, onWallSize);   
        Gizmos.DrawWireCube(_rightOnWallPosition, onWallSize);   
    }
    
    #endregion
}   