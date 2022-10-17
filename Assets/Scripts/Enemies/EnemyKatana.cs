using System.Collections;
using UnityEngine;

public class EnemyKatana : BaseKatana
{
    [Space]
    [Header("Enemy Katana Attributes")]
    [SerializeField] float anticipationTime;

    // Hidden variables
    [HideInInspector] public bool isAnticipating;

    // Local Variables

    // Dependencies
    CombatEnemy enemy;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


        // Getting dependencies
        enemy = transform.parent.GetComponent<CombatEnemy>();
    }


    // Update is called once per frame
    protected override void Update()
    {
        // Enemy should not be dashing right now
        if (enemy.isDashing || isAnticipating)
            return;


        base.Update();


        // Player must be in range and enemy must not be dead to continue
        if (enemy.PlayerOnRange() && !enemy.isDead)
        {
            // Show katana
            katanaVisual.gameObject.SetActive(true);


            // Move katana in player's direction
            MoveKatana(enemy.player.transform.position);


            // Don't continue to run the script
            return;
        }


        // Not show the katana anymore
        katanaVisual.gameObject.SetActive(false);
    }


    public IEnumerator AnticipateAttack(Vector2 _targetPosition)
    {
        // Attack cooldown must be up
        if (currentAttackCooldown > 0)
            yield break;


        // Sinalize to the script that it's current anticipating an attack
        isAnticipating = true;


        // Anticipation animation
        anim.Play("Anticipation");


        // Visual
        enemy.rightAttack = enemy.player.transform.position.x > transform.position.x ? true : false;


        // Wait for anticipation time before attacking
        yield return new WaitForSeconds(anticipationTime);


        // Reset the variable
        isAnticipating = false;


        // Attack at the target position
        Attack(_targetPosition);
    }


    public override void Attack(Vector2 _targetPosition)
    { 
        base.Attack(_targetPosition);


        // Dash

        // Dash in the direction the katana is looking at
        StartCoroutine(enemy.Dash(_targetPosition));
    }


    // For when the room needs to restart and so does the enemy
    public void Reset() 
    {
        StopAllCoroutines();
        isAnticipating = false;
        currentAttackCooldown = 0;
    }


    // Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // If object is the player reset the room
        if (other.name == "Player")
            CameraTransition.Instance.TransitionReset();
    }
}
