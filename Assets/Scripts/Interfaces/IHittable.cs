using UnityEngine;

// Interface for classes that can be hitten
public interface IHittable
{
    // Function that happens when the enemy gets hitten
    bool Hit();
}