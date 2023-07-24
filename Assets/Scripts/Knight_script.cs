using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading;
using System.Collections;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float maxJumpHeight = 10f;
    private bool isGrounded = false;

    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static Knight Instance { get; private set; }

    public void GetDamage()
    {
        lives -= 1;
        UnityEngine.Debug.Log("Knight's remaining lives: " + lives);
        if (lives < 1)
        {
            Die(); // Call the Die() method when lives reach 0.
        }
    }
    private void Die()
    {
        // Destroy the knight GameObject when it dies.
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Instance = this;
        isRecharged = true;
    }
    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded && !isAttacking) State = States.idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if  (isGrounded && !isAttacking && Input.GetButton("Jump"))
            Jump();
        if (Input.GetButton("Fire1"))
            Attack();
    }

    private void Attack()
    {
        if (isGrounded && isRecharged)
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;
            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private IEnumerator AttackAnimation()
    {
        // Play the attack animation here.
        anim.SetTrigger("attack"); // Assuming you have a trigger parameter named "attack" in the Animator.

        // Wait for the animation to complete before resetting the attacking flag.
        yield return new WaitForSeconds(1f); // Replace '1f' with the actual duration in seconds.

        // Reset the attacking flag after the animation is complete.
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        // Wait for the cooldown duration before recharging the attack.
        yield return new WaitForSeconds(2f); // Replace '2f' with the actual cooldown duration in seconds.

        // Recharge the attack after the cooldown is complete.
        isRecharged = true;
    }

    private void OnAttack()
    {
        UnityEngine.Debug.Log("OnAttack called!");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
        StartCoroutine(AttackAnimation());
    }


    private void Run()
    {
        if (isGrounded) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            if (rb.velocity.y < maxJumpHeight)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }


    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded) State = States.jump;
    }

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    public enum States
    {
        idle,
        run,
        jump,
        attack
    }
}
