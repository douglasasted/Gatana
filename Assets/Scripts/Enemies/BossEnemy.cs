using UnityEngine;

public class BossEnemy : BaseEnemy
{
    // Movement Attributes
    [SerializeField] float speed;

    [SerializeField] DialogPrompt prompt;
    [SerializeField] GameObject visualPrompt;

    // Local variables
    bool moving;


    public override bool Hit() 
    {
        base.Hit();

        visualPrompt.SetActive(false);

        // Player should not move during the rest of the scene
        PlayerManager.Instance.player.GetComponent<PlayerMovement>().cantMove = true;
        
        // Transition with the camera to the next scene
        Invoke("EndScene", 5);

        return true;
    }

    protected override void Update()
    {
        // Start the main loop of the enemy
        // Player needs to not be dead
        if (!isDead) 
            Main();
    }


    protected override void Main()
    {
        base.Main();

        if (moving)
        {
            // Animation
            anim.Play("Walk");

            // Moving enemy in intended direction
            rb.velocity = new Vector2(speed, rb.velocity.y);
        
            // Visual
            mainVisual.flipX = false;

            if (transform.position.x >= 84)
            {
                rb.velocity = Vector2.zero;
                moving = false;
                prompt.enabled = true;
            }

            // Don't continue the code
            return;
        }

        anim.Play("Idle");

        // Enemy needs to face the player direction when not moving

        // The enemy is being flipped when the player x is greater than he's x
        // meaning they are to the the right of the player
        mainVisual.flipX = player.transform.position.x > transform.position.x ? false : true;
    }


    // Function for starting the movement
    public void StartMovement() 
    {
        moving = true;
    }


    public void EndScene() 
    {
        CameraTransition.Instance.TransitionScene("Ending");
    }
}