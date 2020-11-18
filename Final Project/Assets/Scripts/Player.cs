using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] LayerMask _ground;
    [SerializeField] LayerMask _collectable;
    [SerializeField] float _jumpForce = 7f;
    [SerializeField] float _runningBoost = 3f;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] List<string> _landables;
    [SerializeField] GameObject _bullet;
    private bool canMove = false;
    private bool canJump = true;
    private bool isRunning = false;
    private bool hasLevelEnded = false;

    private Rigidbody2D rigidbody;
    private CapsuleCollider2D collider2D;
    private string sceneName;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();
        collider2D = transform.GetComponent<CapsuleCollider2D>();

        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        animator.SetBool("hasWon", false);
        sceneName = LoadScenes.SceneInstance.getSceneName();
        AudioManager.AudioInstance.PlaySound(sceneName);
    }
    // Update is called once per frame
    void Update()
    {
        if(!hasLevelEnded)
        {
            isGrounded();
            if (canMove)
            {
                CheckJump();
                CheckIfFalling();
                CheckWeapon();
                Movement();
            }
        }
       
    }
    public void Movement()
    {
        float move = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(move));
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;
        Vector2 movementVector = new Vector2(move * _movementSpeed, rigidbody.velocity.y);
        if (isRunning && move!=0f)
        {
            movementVector.x = movementVector.x < 0 ? movementVector.x - _runningBoost : movementVector.x + _runningBoost;
        }
        animator.SetBool("isRunning", isRunning);

        rigidbody.velocity = movementVector;

        flipSprite(movementVector);
    }
    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            //Debug.Log("Jumping");
            if (isGrounded())
            {
                float extraBoost = isRunning ? .8f *_runningBoost + _jumpForce : _jumpForce;
                rigidbody.velocity = Vector2.up * extraBoost;
                AudioManager.AudioInstance.PlaySound("Jump");
                animator.SetBool("isJumping", true);
                canJump = false;
            }
            
        }
    }
    private void CheckIfFalling()
    {
        bool isFalling = false;
        if (rigidbody.velocity.y < -1.5f)
            isFalling = true;
        animator.SetBool("isFalling", isFalling);
    }
    private void CheckWeapon()
    {
        //TODO - Check Input for when to shoot
        GameObject weapon = gameObject.transform.Find("Weapon").gameObject;
        if(_runtimeData._hasWeapon)
        {
            weapon.GetComponent<SpriteRenderer>().enabled = true;
            weapon.GetComponent<SpriteRenderer>().sprite = _runtimeData._currentWeapon;
            FireWeapon();
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void FireWeapon()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            //Debug.Log("FIRING");
            Instantiate(_bullet, gameObject.transform.Find("Weapon").position, Quaternion.identity);
            AudioManager.AudioInstance.PlaySound("Fire Bullet");
        }
    }
    private void flipSprite(Vector2 movementVector)
    {
        if(_runtimeData._hasWeapon)
        {
            GameObject weapon = gameObject.transform.Find("Weapon").gameObject;
            if (movementVector.x < 0)
            {
                weapon.GetComponent<SpriteRenderer>().flipX = true;
                weapon.transform.position = new Vector3(gameObject.transform.position.x -.7f,weapon.transform.position.y,weapon.transform.position.z);
                _runtimeData._isGunFlipped = true;
            }
            else if (movementVector.x > 0)
            {
                weapon.GetComponent<SpriteRenderer>().flipX = false;
                weapon.transform.position = new Vector3(gameObject.transform.position.x + .7f, weapon.transform.position.y, weapon.transform.position.z);
                _runtimeData._isGunFlipped = false;

            }
        }
        if (movementVector.x < 0)
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else if (movementVector.x > 0)
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }
    private bool isGrounded()
    {
        float padding = 1f;
        RaycastHit2D hit = Physics2D.Raycast(collider2D.bounds.center, Vector2.down, collider2D.bounds.extents.y + padding, _ground);
        Color ray;
        if (hit.collider != null)
            ray = Color.green;
        else
            ray = Color.red;
        Debug.DrawRay(collider2D.bounds.center, Vector2.down * collider2D.bounds.extents.y, ray);
        return hit.collider != null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string landingSpot = collision.gameObject.tag;
        if (_landables.Contains(landingSpot) && isGrounded())
        {
            canMove = true;
            canJump = true;
            animator.SetBool("isJumping", false);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.tag.Equals("untagged"))
            AudioManager.AudioInstance.PlaySound(collision.tag);
        if (collision.tag.Equals("Coin"))
        {
            _runtimeData._totalCoins++;
            _runtimeData._totalScore += 10;
        }
        else if (collision.tag.Equals("Dragon Coin"))
        {
            _runtimeData._totalScore += 1000;
            _runtimeData._totalDragonCoins++;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collectables"))
            Destroy(collision.gameObject);
    }
}
