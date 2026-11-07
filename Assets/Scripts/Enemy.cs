using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public float Health;
    public float Speed;
    public float AttackCooldown;
    public float ExperienceDropsMean = 3;
    public float MoneyDropsMean = 3;
    public float DropRange = 1;

    public float SpawnRadius = 2.0f;

    public bool FollowsPlayer;

    public bool IsRanged;
    public float RangedFollowDistance;

    public bool CanMove = true;

    [SerializeField]
    private GameObject ExperienceOrbPrefab;

    [SerializeField]
    private GameObject CoinPrefab;

    [SerializeField]
    private LayerMask wallLayer;


    private NavMeshAgent navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            Move();
        }

        checkAttackConditions();

        if (Health <= 0)
        {
            int energyToSpawn = (int)Random.Range(ExperienceDropsMean - DropRange, ExperienceDropsMean + DropRange);
            int moneyToSpawn = (int)Random.Range(MoneyDropsMean - DropRange, MoneyDropsMean + DropRange);

            Debug.Log(energyToSpawn + ", " + moneyToSpawn);

            for (int i = 0; i < energyToSpawn; i++)
            {
                SpawnLoot(true);
            }

            for (int j = 0; j < moneyToSpawn; j++)
            {
                SpawnLoot(false);
            }

            Room roomRef = GameManager.Instance.CurrentRoomReference;
            Debug.Log(roomRef == null);
            if (roomRef != null)
            {
                roomRef.enemiesCleared++;
            }

            Destroy(gameObject);
        }
    }

    private void SpawnLoot(bool isEnergy)
    {
        GameObject objectToSpawn = (isEnergy ? ExperienceOrbPrefab : CoinPrefab);

        // 1. Get a completely random direction in 2D space
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0.5f, SpawnRadius);

        Vector2 targetPosition = (Vector2)transform.position + (randomDirection * randomDistance);

        // 2. Cast a thick circle ray towards the target to check for walls
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, randomDirection, randomDistance, wallLayer);

        if (hit.collider != null)
        {
            // Wall detected! Set the spawn point safely right in front of the wall
            // hit.centroid is where the circle projection stopped safely
            Instantiate(objectToSpawn, hit.centroid, Quaternion.identity);
        }
        else
        {
            // Path is clear!
            Instantiate(objectToSpawn, targetPosition, Quaternion.identity);
        }
    }

    public virtual void Move()
    {
        // Implement movement logic here

        if (!IsRanged)
        {
            navMeshAgent.destination = GameObject.FindWithTag("Player").transform.position;
        }

       
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
