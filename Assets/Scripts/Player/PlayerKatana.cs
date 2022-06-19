using UnityEngine;

public class PlayerKatana : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float angleOffset;
    [SerializeField] float attackCooldown;

    [Header("Visual")]
    [SerializeField] SpriteRenderer characterVisual;

    [Header("Sounds")]
    [SerializeField] AudioSource slashSound;
    [SerializeField] AudioSource hitSound;

    // Local Variables
    float currentAttackCooldown;

    // Dependencies
    Animator anim;
    InputManager inputManager;
    PlayerMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        // Getting dependencies
        anim = GetComponent<Animator>();
        inputManager = InputManager.Instance;
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        // Updatin attack cooldown
        currentAttackCooldown -= Time.deltaTime;


        // Only continue if dash is over
        if (playerMovement.isDashing)
            return;


        // Setting the katana in direction

        // Cursor position on the world
        Vector2 _cursorPosition = Camera.main.ScreenToWorldPoint(inputManager.GetCursorPosition());
        // Getting the angle the katana should be
        float _katanaAngle = -GetAngleToPoint(transform.position, _cursorPosition) + angleOffset;
        // Set katana in the cursor direction
        transform.rotation = Quaternion.Euler(0, 0, _katanaAngle);


        // Visual

        // Change character to face the cursor side
        characterVisual.flipX = _cursorPosition.x > transform.position.x ? false : true;


        // Does player wants to attack? then attack
        if (inputManager.GetFirePressed() && currentAttackCooldown < 0) 
            // This gives us the direction of the cursor in relation to the katana
            Attack((Vector2) transform.position - _cursorPosition);
    }


    void Attack(Vector2 _direction) 
    {
        // Sound

        // Playing the sound of the slash
        slashSound.Play();


        // Attack & Animation

        // Playing the slash animation
        // This is technically the attack, 
        // since the animation is where the katana collider shows up
        // (from the enemy collision with the collider the damage occurs)
        anim.Play("Slash");
        // Cooldown for player attacking again 
        currentAttackCooldown = attackCooldown;


        // Dash

        // Dash in the direction the katana is looking at
        StartCoroutine(playerMovement.Dash(_direction));
    }


    // Get the angle from origin to target
    float GetAngleToPoint(Vector2 origin, Vector2 target) 
    {
        // The triangle of the origin to the target
        Vector2 triangle = origin - target;
        // The angle of the tangent of the triangle from radian to degrees
        return Mathf.Atan2(triangle.x, triangle.y) * Mathf.Rad2Deg;
    }


    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If object has the component of hittable
        // then trigger the hit
        if (other.GetComponent<IHittable>() != null)
        {
            // Hit trigger 
            other.GetComponent<IHittable>().Hit();


            // Sound

            // Getting sound Clip
            hitSound.clip = other.GetComponent<IHittable>().HitClip;
            // Playing sound clip
            hitSound.Play();

        
            // Reset dash after hitting attack
            playerMovement.ResetDash();
        }
    }
}