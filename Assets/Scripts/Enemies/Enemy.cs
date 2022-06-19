using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{
    // What happens when the enemy gets hitten?
    public void Hit()
    {
        gameObject.SetActive(false);     
    }
}
