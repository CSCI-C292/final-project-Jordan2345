/*
 * https://bigmack.itch.io/wwii-mega-gun-pack - Assets for Weapons
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int _totalLives = 3;
    [SerializeField] string _enemyType;
    [SerializeField] float _movementSpeed = 5f;
    [SerializeField] List<string> objectsThatTurnsEnemy;
    [SerializeField] Animator _animator;

    private List<string> enemiesToFlip;
    private bool canMove = true;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 movementVector = new Vector2(_movementSpeed, 0f);
        rb.velocity = movementVector;
        enemiesToFlip = new List<string>();
        enemiesToFlip.Add("Dragon");
    }
    private void Start()
    {
        if (enemiesToFlip.Contains(_enemyType))
            _movementSpeed *= -1;
    }
    private void Update()
    {
        if(canMove)
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
        //koopa total lives = 2
        if (collision.name.Equals("Player"))
        {
            Debug.Log("Player Killed");
            DeathMechanics.DeathMechanicsInstance.CheckDeath();
        }
        else if(collision.tag.Equals("Bullet"))
        {
            _totalLives--;
            if(_totalLives == 1)
            {
                //koopa has been hit, change animation to small koopa
                _animator.SetBool("hasBeenHit", true);
            }
            else if(_totalLives == 0)
            {
                //koopa is dead
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
        else if(objectsThatTurnsEnemy.Contains(collision.tag))
        {
            //turn koopa around
            TurnEnemyAround();
        }
    }
    private void DragonTrigger(Collider2D collision)
    {
        //Dragon total lives = 3
        if (collision.name.Equals("Player"))
        {
            //TODO - Kills/Removes powerup from player
            Debug.Log("Player Killed");
            DeathMechanics.DeathMechanicsInstance.CheckDeath();
        }
        else if (collision.tag.Equals("Bullet"))
        {
            _totalLives--;
            Debug.Log(_totalLives);
            if (_totalLives == 2)
            {
                Debug.Log("Hit Once");
                Destroy(collision.gameObject);
                //Dragon has been hit, change animation to smaller Dragon
                _animator.SetBool("hitOnce", true);
            }
            else if (_totalLives == 1)
            {
                Debug.Log("Hit Twice");
                _animator.SetBool("hitOnce", false);
                Destroy(collision.gameObject);
                //Dragon gets even smaller
                _animator.SetBool("hitTwice", true);
            }
            else if(_totalLives == 0)
            {
                //Dragon dead
                Destroy(collision.gameObject);
                Destroy(gameObject);
                
            }
        }
        else if (objectsThatTurnsEnemy.Contains(collision.tag))
        {
            Debug.Log(collision.tag);
            //turn Dragon around
            TurnEnemyAround();
        }
    }
    private void BulletBillTrigger(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            DeathMechanics.DeathMechanicsInstance.CheckDeath();
        }
    }
    private void TurnEnemyAround()
    {
        Vector2 movementVector = new Vector2(rb.velocity.x * -1, rb.velocity.y);
        rb.velocity = movementVector;
        if (GetComponent<SpriteRenderer>().flipX)
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_enemyType.Equals("Koopa"))
        {
            KoopaTrigger(collision);
        }
        else if(_enemyType.Equals("Dragon"))
        {
            DragonTrigger(collision);
        }
        else if(_enemyType.Equals("Bullet Bill"))
        {
            BulletBillTrigger(collision);
        }
    }
}
