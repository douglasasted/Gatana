using UnityEngine;

// Interface for classes that can be hitten
public interface IHittable
{
    // Audio clip that player needs to play
    public AudioClip HitClip { get; }


    // Function that happens when the enemy gets hitten
    void Hit();
}