using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class DialogManager : MonoBehaviour
{
    #region Singleton
    
    static DialogManager _instance;

    public static DialogManager Instance 
    {
        get 
        {
            return _instance;
        }
    }

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    #endregion


    [SerializeField] float typeTime;

    [SerializeField] AudioSource dialogSound;
    [SerializeField] DialogSpeaker[] dialogSpeakers;
    [SerializeField] DialogLine[] dialogChain;

    [Header("Events")]
    [SerializeField] UnityEvent onDialogFinish;

    // Local variables
    int currentLine;
    int currentSpeaker;

    bool onDialog;
    bool typing;

    TMP_Text dialogText;
    GameObject finishDialogIndicator;
    GameObject speechBubble;

    // Dependencies
    InputManager inputManager;

    
    // Start is called on the frame when a script is enabled just before
    // any of the Update methods is called the first time.
    void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        // Player can skip dialog on go to the next line
        if ((inputManager.GetInteract() || inputManager.GetFirePressed()) && onDialog)
        {
            // Skipping current line
            if (typing)
                typing = false;
            // Showing next line
            else
                ShowNextLine();
        }
    }


    public void ChangeDialog(DialogLine[] _dialogChain = null, UnityEvent _onDialogFinish = null)
    {
        if (_dialogChain != null)
            dialogChain = _dialogChain;
        
        if (_onDialogFinish != null)
            onDialogFinish = _onDialogFinish;
    }


    // Function for starting that triggers the dialog to start
    public void StartDialog() 
    {
        // Reset variables
        currentLine = 0;
        currentSpeaker = dialogChain[0].speaker;
        onDialog = true;


        // Changing variables
        dialogText = dialogSpeakers[currentSpeaker].dialogText;
        finishDialogIndicator = dialogSpeakers[currentSpeaker].finishDialogIndicator;
        dialogSound.clip = dialogSpeakers[currentSpeaker].sound;
        // If there's a speech bubble for this speaker then show it
        if (dialogSpeakers[currentSpeaker].speechBubble != null)
        {
            speechBubble = dialogSpeakers[currentSpeaker].speechBubble;
            speechBubble.SetActive(true);
        }


        // Actually starting dialog
        ShowNextLine();
    }


    // Function that decides what should be the next dialog line
    void ShowNextLine()
    {
        // There's no more dialog lines to show
        if (currentLine == dialogChain.Length)
        {
            FinishDialog();


            // Don't continue the script
            return;
        }


        // If the speaker has changed in this line
        // Change the dialog info with it
        if (dialogChain[currentLine].speaker != currentSpeaker)
        {
            // Resetting previous bubble
            dialogText.text = "";
            finishDialogIndicator.SetActive(false);
            if (speechBubble != null)
                speechBubble.SetActive(false);


            // Actual change in variables
            currentSpeaker = dialogChain[currentLine].speaker;
            // Changing dialog info
            dialogText = dialogSpeakers[currentSpeaker].dialogText;
            finishDialogIndicator = dialogSpeakers[currentSpeaker].finishDialogIndicator;
            dialogSound.clip = dialogSpeakers[currentSpeaker].sound;
            // Is there a dialog bubble to change?
            if (dialogSpeakers[currentSpeaker].speechBubble != null)
            {
                speechBubble = dialogSpeakers[currentSpeaker].speechBubble;
                speechBubble.SetActive(true);
            }
        }



        // Reset variables
        dialogText.maxVisibleCharacters = 0;
        finishDialogIndicator.SetActive(false);
        typing = true;


        // Type the line
        dialogText.text = dialogChain[currentLine].line.GetLocalizedString();
        StartCoroutine(TypeDialog());


        // We are at the next line now
        currentLine++;
    }


    // Function that types the dialog on screen
    IEnumerator TypeDialog() 
    {
        // Actually typing the dialog
        while (dialogText.maxVisibleCharacters < dialogText.text.Length - 1 && typing)
        {
            // Show a character
            dialogText.maxVisibleCharacters++;

            // Sound
            // Only if current character isn't nothing
            if (dialogText.text[dialogText.maxVisibleCharacters] != ' ')
            {
                dialogSound.pitch = Random.Range(1, 2);
                dialogSound.Play();
            }

            float _typeTime = dialogText.text[dialogText.maxVisibleCharacters] != '.' ? typeTime : typeTime * 2;


            // Wait a little bit before showing the next character
            yield return new WaitForSeconds(_typeTime);
        }
        

        // Reset variables
        dialogText.maxVisibleCharacters = dialogText.text.Length;
        // Is not typing anymore
        typing = false;


        // Show player that he can skip now
        finishDialogIndicator.SetActive(true);
    }


    // Function that finishes the dialog
    public void FinishDialog()
    {
        // Not on a dialog anymore
        onDialog = false;
        finishDialogIndicator.SetActive(false);
        dialogText.maxVisibleCharacters = 0;

        if (speechBubble != null)
            speechBubble.SetActive(false);

        // Event that happens when dialog finishes
        onDialogFinish.Invoke();
    }
}