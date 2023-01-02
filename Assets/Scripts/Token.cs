using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] AudioSource tokenSound;
    
    bool hasBeenTaken;


    void OnTriggerEnter2D(Collider2D other) 
    {
        // Can token be collected?
        if (hasBeenTaken)
            return;

        // Can't be collected anymore
        hasBeenTaken = true;


        // Collecting Token
        if (StatsManager.Instance != null) StatsManager.Instance.collectedTokens += 1;

        // Sound
        tokenSound.Play();

        // Can't collect this token again
        gameObject.SetActive(false);
    }
}
