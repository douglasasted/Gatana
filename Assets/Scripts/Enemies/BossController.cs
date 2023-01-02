using UnityEngine;

public class BossController : MonoBehaviour, IHittable
{
    [SerializeField] AudioClip hitClip;

    // Audio clip that player needs to play when enemy gets hitten    
    public AudioClip HitClip 
    { 
        get
        {
            return hitClip;
        }     
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IHittable.Hit()
    {
        throw new System.NotImplementedException();
    }
}
