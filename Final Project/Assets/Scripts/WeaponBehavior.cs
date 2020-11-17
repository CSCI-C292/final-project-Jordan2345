using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] List<string> canCollideWith;
    [SerializeField] RuntimeData _runtimeData;
    private Rigidbody2D rb;
    private bool hasBeenCollected = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(_movementSpeed, rb.velocity.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasBeenCollected)
            Move();
    }
    private void Move()
    {
        float xSpeed = GetComponent<SpriteRenderer>().flipX == true ? -_movementSpeed : _movementSpeed;
        Vector2 movementVector = new Vector2(xSpeed, rb.velocity.y);
        rb.velocity = movementVector;
    }
    private void FlipWeapon()
    {
        Vector2 movementVector = new Vector2(rb.velocity.x * -1, rb.velocity.y);
        rb.velocity = movementVector;
        if (rb.velocity.x < 0)
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else if (rb.velocity.x > 0)
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Collected by player");
            hasBeenCollected = true;
            AudioManager.AudioInstance.PlaySound("Collect Weapon");
            _runtimeData._currentWeapon = gameObject.GetComponent<SpriteRenderer>().sprite;
            _runtimeData._hasWeapon = true;
            Destroy(gameObject);
        }
        else if(canCollideWith.Contains(collision.gameObject.tag))
        {
            FlipWeapon();
        }
    }
}
