using UnityEngine;


public class CutsceneController : MonoBehaviour
{
    [SerializeField] bool dollMoving;
    [SerializeField] Animator playerDoll;


    public void ChangeAnimation(string animation) 
    {
        // Change players animation to selected one
        playerDoll.Play(animation);
        playerDoll.GetComponent<PlayerDoll>().moving = dollMoving;
    }

    public void ShowPlayer() 
    {
        // Show player and hide doll
        playerDoll.gameObject.SetActive(false);
        PlayerManager.Instance.player.SetActive(true);
    }
}
