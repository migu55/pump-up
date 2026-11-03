using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float Health;
    public float Speed;
    public float AttackCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Move()
    {
        // Implement movement logic here
    }

    public virtual void checkAttackConditions()
    {
        Attack();
    }
    public virtual void Attack()
    {
        // Implement attack logic here
    }
}
