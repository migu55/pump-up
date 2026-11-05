using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public float Health;
    public float Speed;
    public float AttackCooldown;
    public float ExperienceDropsMean;
    public float MoneyDropsMean;

    public bool IsRanged;
    public bool RangedFollowDistance;

    [SerializeField]
    private GameObject ExperienceOrb;

    [SerializeField]
    private GameObject Coin;


    private NavMeshAgent navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {

            //calculate drops randomly based on mean values


            Destroy(gameObject);
        }
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
