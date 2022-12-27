public class BossEnemy : BaseEnemy
{
    public override bool Hit() 
    {
        base.Hit();

        // Player should not move during the rest of the scene
        PlayerManager.Instance.player.GetComponent<PlayerMovement>().cantMove = true;
        
        // Transition with the camera to the next scene
        Invoke("EndScene", 5);

        return true;
    }

    public void EndScene() 
    {
        CameraTransition.Instance.TransitionScene("Ending");
    }
}