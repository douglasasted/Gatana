using UnityEngine;
using UnityEngine.Events;


public class DialogPrompt : MonoBehaviour
{
    [SerializeField] float interactRange;
    [SerializeField] GameObject interactPrompt;

    [Header("Dialog Info")]
    [SerializeField] DialogLine[] dialogChain;
    [SerializeField] UnityEvent onDialogFinish;

    // Local Variables
    bool interacted;

    // Initial Variables
    GameObject player;

    // Depedencies
    InputManager inputManager;
    DialogManager dialogManager;


    // Start is called before the first frame update
    void Start()
    {
        // Getting dependencies
        inputManager = InputManager.Instance;

        // Getting initial variables
        player = PlayerManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        // If player is in interact range than show the prompt
        if (Vector2.Distance(transform.position, player.transform.position) < interactRange && 
           Vector2.Distance(transform.position, player.transform.position) > 1 && !interacted)
        {
            // Prompt visually
            interactPrompt.SetActive(true);


            if (inputManager.GetInteract())
            {
                // Interact with dialog prompt
                interacted = true;

                // Player should not move anymore
                player.GetComponent<PlayerMovement>().cantMove = true;


                // Start dialog
                DialogManager.Instance.ChangeDialog(dialogChain, onDialogFinish);
                DialogManager.Instance.StartDialog();
            }


            // Don't continue the code
            return;
        }


        // Player is not in range
        interactPrompt.SetActive(false);
    }


    #region Debug

    void OnDrawGizmos() 
    {
        // Drawing debug circle of the color red on the interaction range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    #endregion
}