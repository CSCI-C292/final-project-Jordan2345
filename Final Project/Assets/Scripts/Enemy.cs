using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _totalLives = 3;
    [SerializeField] string _enemyType;
    [SerializeField] float _movementSpeed = 5f;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 movementVector = new Vector2(_movementSpeed, 0f);
        rb.velocity = movementVector;
    }
    private void Update()
    {
        MoveEnemy();
    }
    private void MoveEnemy()
    {
        float xSpeed = GetComponent<SpriteRenderer>().flipX == true ? -_movementSpeed : _movementSpeed;
        Vector2 movementVector = new Vector2(xSpeed,rb.velocity.y);
        rb.velocity = movementVector;
    }
    private void KoopaTrigger(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            //TODO - Kills/Removes powerup from player
            Debug.Log("Player Killed");
        }
        else if(collision.tag.Equals("Koopa") || collision.tag.Equals("Ground"))
        {
            //turn koopa around
            TurnEnemyAround();
        }
    }
    private void TurnEnemyAround()
    {
        Vector2 movementVector = new Vector2(rb.velocity.x * -1, rb.velocity.y);
        rb.velocity = movementVector;
        if (rb.velocity.x < 0)
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else if (rb.velocity.x > 0)
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_enemyType.Equals("Koopa"))
        {
            KoopaTrigger(collision);
        }
    }
}
