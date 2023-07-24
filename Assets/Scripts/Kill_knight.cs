using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_not_move : Entity
{
    private int lives = 3;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Knight.Instance.gameObject)
        {
            Knight.Instance.GetDamage();
            lives--;
            UnityEngine.Debug.Log("Enemy lives: " + lives);
        }

        if (lives < 1)
            Die();
    }
}
