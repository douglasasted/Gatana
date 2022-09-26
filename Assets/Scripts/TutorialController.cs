using UnityEngine;


public class TutorialController : MonoBehaviour
{
    [SerializeField] Animator walkingTutorial;
    [SerializeField] Animator jumpingTutorial;
    [SerializeField] Animator climbingTutorial;
    [SerializeField] Animator attackTutorial;
    [SerializeField] Animator interactTutorial;


    // Local Variables
    int tutorial;

    // Dependencies
    InputManager inputManager;


    // Start is called before the first frame update
    void Start()
    {
        // Getting dependencies
        inputManager = InputManager.Instance;


        walkingTutorial.SetTrigger("FadeIn");
    }


    // Update is called once per frame
    void Update()
    {
        // Which part of the tutorial is the player in?
        switch(tutorial)
        {

            // Walking tutorial
            case 0:
                
                if (inputManager.GetMovement() != 0)
                {
                    // Finished walking tutorial
                    walkingTutorial.SetTrigger("FadeOut");
                    tutorial = 1;


                    // Show the jumping tutorial
                    jumpingTutorial.SetTrigger("FadeIn");
                }

                break;


            // Jumping tutorial
            case 1:
                
                if (inputManager.GetJumpPressed())
                {
                    // Finished jumping tutorial
                    jumpingTutorial.SetTrigger("FadeOut");
                }

                break;


            // Attack tutorial
            case 3:
                
                if (inputManager.GetFirePressed())
                {
                    tutorial = 4;

                    // Finished attack tutorial
                    attackTutorial.SetTrigger("FadeOut");


                    // Start interact tutorial
                    interactTutorial.SetTrigger("FadeIn");
                }

                break;
        }
    }

    // Sent when another object enters a trigger collider attached to this object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If player has reached the climbing tutorial
        if (other.gameObject.name == "Climbing Tutorial" && tutorial < 2)
        {
            // Start the tutorial

            tutorial = 2;

            // Fade out the jumping tutorial if it's still active
            jumpingTutorial.SetTrigger("FadeOut");

            // Show climbing tutorial
            climbingTutorial.SetTrigger("FadeIn");
        }
        else if (other.gameObject.name == "Climbing Tutorial F")
        {
            tutorial = 3;
            
            // Finish climbing tutorial
            climbingTutorial.SetTrigger("FadeOut");
            
            // Show next tutorial
            attackTutorial.SetTrigger("FadeIn");
        }
    }
}
