using UnityEngine;


public class PlayerDoll : MonoBehaviour
{
    public bool moving;
 
    [Header("Sound")]
    [SerializeField] float stepSoundCooldown;
    [SerializeField] AudioSource stepSound;
    [SerializeField] AudioClip[] stepClips;

    // Local variables
    float stepSoundCurrentCooldown;


    // Update is called once per frame
    void Update()
    {
        // Step sound

        // If player is moving play the sound of player's steps
        if (moving && stepSoundCurrentCooldown <= 0)
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
    }
}
