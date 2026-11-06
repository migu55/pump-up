using Unity.VisualScripting;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    public float Lifetime = 0.0f;
    public float MaxLifetime = 5.0f;
    public float Damage = 15.0f;

    public bool IsPlayerOwned = false;

    public bool CanPierce = false;
    public int PiercingDepth = 0;
    public int MaxPiercingDepth = 3;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Gate")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player" && !IsPlayerOwned)
        {
            GameManager.Instance.Energy -= Damage;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy" && IsPlayerOwned)
        {
            collision.gameObject.GetComponent<Enemy>().Health -= Damage;

            if (CanPierce)
            {
                PiercingDepth++;
                if (PiercingDepth >= MaxPiercingDepth)
                {
                    Destroy(gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}
