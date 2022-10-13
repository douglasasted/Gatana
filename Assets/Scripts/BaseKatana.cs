using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseKatana : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected float angleOffset;
    [SerializeField] protected float attackCooldown;

    [Space]

    [Header("Sounds")]
    [SerializeField] AudioSource slashSound;
    [SerializeField] protected AudioSource hitSound;

    [Space]

    [Header("Visual")]
    public SpriteRenderer katanaVisual;

    // Local Variables
    protected float currentAttackCooldown;

    // Dependencies
    protected Animator anim;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Getting dependencies
        anim = GetComponent<Animator>();
    }


    // Update is called every frame, if the MonoBehaviour is enabled.
    protected virtual void Update()
    {
        // Updating variables
        currentAttackCooldown -= Time.deltaTime;
    }


    protected virtual void MoveKatana(Vector2 _targetPosition)
    {
        // Getting the angle the katana should be
        float _katanaAngle = -GetAngleToPoint(transform.position, _targetPosition) + angleOffset;
        // Set katana in the cursor direction
        transform.rotation = Quaternion.Euler(0, 0, _katanaAngle);
    }


    public virtual void Attack(Vector2 _direction) 
    {
        // Cooldown for attacking again 
        currentAttackCooldown = attackCooldown;


        // Sound

        // Playing the sound of the slash
        slashSound.Play();


        // Attack & Animation

        // Playing the slash animation
        // This is technically the attack, 
        // since the animation is where the katana collider shows up
        // (from the enemy collision with the collider the damage occurs)
        anim.Play("Slash");
    }


    #region Utility    

    // Get the angle from origin to target
    float GetAngleToPoint(Vector2 origin, Vector2 target) 
    {
        // The triangle of the origin to the target
        Vector2 triangle = origin - target;
        // The angle of the tangent of the triangle from radian to degrees
        return Mathf.Atan2(triangle.x, triangle.y) * Mathf.Rad2Deg;
    }

    #endregion
}
