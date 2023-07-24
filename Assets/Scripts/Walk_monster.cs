using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Walk_monster : Entity
{
    private float speed = 1.5f;
    private Vector3 direction;
    private SpriteRenderer sprite;
    private int lives = 3;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        direction = -transform.right; // Set the initial direction to the left
        UpdateSpriteFacing();
    }

    private void Move()
    {
        float distance = speed * Time.deltaTime;
        Vector3 newPosition = transform.position + direction * distance;

        // Check if there is a collider in front of the character
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition + transform.up * 0.1f, 0.1f);
        bool shouldChangeDirection = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && !collider.isTrigger)
            {
                shouldChangeDirection = true;
                break;
            }
        }

        if (shouldChangeDirection)
        {
            direction *= -1f; // Change direction when colliding with an object
            UpdateSpriteFacing();
        }
        else
        {
            transform.position = newPosition;
        }
    }

    private void UpdateSpriteFacing()
    {
        // Flip the sprite based on the direction.
        if (direction.x > 0.0f)
        {
            sprite.flipX = false; // Face right
        }
        else if (direction.x < 0.0f)
        {
            sprite.flipX = true; // Face left
        }
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Knight.Instance.gameObject)
        {
            Knight.Instance.GetDamage();
        }
    }
}