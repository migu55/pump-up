using Unity.VisualScripting;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    public float Lifetime = 0.0f;
    public float MaxLifetime = 5.0f;
    public float Damage;

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
}
