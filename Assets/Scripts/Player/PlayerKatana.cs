using UnityEngine;

public class PlayerKatana : BaseKatana
{
    [SerializeField] SpriteRenderer characterVisual;

    [Space]

    [SerializeField] float cameraShakeIntensity;
    [SerializeField] float cameraShakeTime;

    [Header("Player Attributes")]
    [SerializeField] bool canAttack = true;
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject altCursor;

    // Local Variables

    // Dependencies
    PlayerMovement playerMovement;
    InputManager inputManager;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Getting dependencies
        inputManager = InputManager.Instance;
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


        if (playerMovement.cantMove)
        {
            // This stops the animation from being in a weird position
            anim.Play("Idle");


            // This makes the arrow dissapear even when is on idle
            katanaVisual.enabled = false;


            // The script should not continue
            return;
        }


        // If we are going forward, then the katana visual needs to be enabled
        katanaVisual.enabled = true;


        // Only continue if dash is over
        if (playerMovement.isDashing)
            return;


        // Local Variables

        // Cursor position on the world
        Vector2 _cursorPosition = Camera.main.ScreenToWorldPoint(inputManager.GetCursorPosition());
        

        MoveKatana(_cursorPosition);


        // Visual

        // If the player is on wall, don't change direction
        if (playerMovement.IsOnWall() == 0)
            // Change character to face the cursor side
            characterVisual.flipX = _cursorPosition.x > transform.position.x ? false : true;


        // Does player wants to attack? then attack
        if (inputManager.GetFirePressed() && currentAttackCooldown < 0 && canAttack) 
            // This gives us the direction of the cursor in relation to the katana
            Attack((Vector2) transform.position - _cursorPosition);
    }


    protected override void MoveKatana(Vector2 _targetPosition) 
    {  
        base.MoveKatana(_targetPosition);
    }


    public override void Attack(Vector2 _direction) 
    {
        base.Attack(_direction);


        // Dash

        // Dash in the direction the katana is looking at
        StartCoroutine(playerMovement.Dash(_direction));
    }


    // Set "canAttack variable" true or false
    public void SetAttack(bool _canAttack)
    {
        // Actually setting the attack
        canAttack = _canAttack;

        if (cursor == null) return;

        // Which cursor should be activated?
        cursor.SetActive(_canAttack);
        altCursor.SetActive(!_canAttack);
    }


    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If object has the hittable component
        // then trigger the hit
        if (other.GetComponent<IHittable>() != null)
        {
            // Hit trigger 
            if (!other.GetComponent<IHittable>().Hit())
                return;


            // Visual

            // Getting variable
            CinemachineShake _cameraShake = RoomManager.Instance.currentRoom.roomCamera.GetComponent<CinemachineShake>();
            // Shaking camera
            _cameraShake.StartCoroutine(_cameraShake.ShakeCamera(cameraShakeIntensity, cameraShakeTime));

        
            // Reset dash after hitting attack
            playerMovement.ResetDash();
        }
    }
}