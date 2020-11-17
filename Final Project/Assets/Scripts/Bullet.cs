using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 6f;
    [SerializeField] List<string> _destroysBullet;
    [SerializeField] RuntimeData _runtimeData;
    private Rigidbody2D rb;
    private bool isFlipped = false;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isFlipped = _runtimeData._isGunFlipped;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }
    private void MoveBullet()
    {
        if (isFlipped)
        {
            rb.velocity = new Vector2(-_bulletSpeed, 0f);
        }
        else
            rb.velocity = new Vector2(_bulletSpeed, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Koopa"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (_destroysBullet.Contains(collision.gameObject.tag))
        {
            Destroy(gameObject);
            Debug.Log("TRIGGER DELETE");
        }
    }
}
