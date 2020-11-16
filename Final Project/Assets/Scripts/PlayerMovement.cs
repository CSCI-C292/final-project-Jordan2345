using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] LayerMask _ground;
    [SerializeField] LayerMask _collectable;
    [SerializeField] float _jumpForce = 7f;
    [SerializeField] float _runningBoost = 3f;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] List<string> _landables;
    private bool canMove = false;
    private bool canJump = true;
    private bool isRunning = false;

    private Rigidbody2D rigidbody;
    private CapsuleCollider2D collider2D;
    private string sceneName;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();
        collider2D = transform.GetComponent<CapsuleCollider2D>();
        _runtimeData._timeLeft = 400;
        _runtimeData._totalScore = 0;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        animator.SetBool("hasWon", false);
        sceneName = LoadScenes.SceneInstance.getSceneName();
        AudioManager.AudioInstance.PlaySound(sceneName);
        InvokeRepeating("DecreaseTimer", 1f, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded();
        if (canMove)
        {
            CheckJump();
            CheckIfFalling();
            Movement();
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
    public void DecreaseTimer()
    {
        _runtimeData._timeLeft--;
    }
    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Debug.Log("Jumping");
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
    private void flipSprite(Vector2 movementVector)
    {
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
            _runtimeData._totalScore += 1000;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collectables"))
            Destroy(collision.gameObject);
    }
}
