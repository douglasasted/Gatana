using System.Collections;
using UnityEngine;

public class PlayerKatana : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float angleOffset;

    [Header("Visual")]
    [SerializeField] SpriteRenderer characterVisual;

    // Local Variables

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
        if (inputManager.GetFirePressed()) 
            // This gives us the direction of the cursor in relation to the katana
            Attack((Vector2) transform.position - _cursorPosition);
    }

    void Attack(Vector2 _direction) 
    {
        // Sound

        // Playing the sound of the slash


        // Attack & Animation

        // Playing the slash animation
        // This is technically the attack, 
        // since the animation is where the katana collider shows up
        // (from the enemy collision with the collider the damage occurs)
        anim.Play("Slash");


        // Dash

        // Dash in the direction the katana is looking at
        StartCoroutine(playerMovement.Dash(_direction));
    }

    // Get the angle from  
    float GetAngleToPoint(Vector2 origin, Vector2 target) 
    {
        // The triangle of the origin to the target
        Vector2 triangle = origin - target;
        // The angle of the tangent of the triangle from radian to degrees
        return Mathf.Atan2(triangle.x, triangle.y) * Mathf.Rad2Deg;
    }
}