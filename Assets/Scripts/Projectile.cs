using Unity.VisualScripting;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    public float Lifetime = 0.0f;
    public float MaxLifetime = 5.0f;
    public float Damage = 15.0f;

    public bool IsPlayerOwned = false;

    public Texture2D texture;
    public Vector2 velocity;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Travel()
    {
        rb.linearVelocity = velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Travel();

        Lifetime += Time.fixedDeltaTime;

        if (Lifetime >= MaxLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" && !IsPlayerOwned)
        {
            GameManager.Instance.Energy -= Damage;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy" && IsPlayerOwned)
        {
            collision.gameObject.GetComponent<Enemy>().Health -= Damage;
            Destroy(gameObject);
        }

        
    }
}
