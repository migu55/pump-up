using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public float Health;
    public float Speed;
    public float AttackCooldown = 1.0f;
    public int ExperienceDropsMean = 3;
    public int MoneyDropsMean = 3;
    public float DropRange = 1.0f;

    public float DropsSpawnRadius = 2.0f;

    public bool FollowsPlayer;

    public bool IsRanged;
    public float RangedFollowDistance;

    public bool CanMove = true;
    public bool CanAttack = true;

    public float MinimumAttackRange = 2.0f;

    private Transform playerPosition;

    [SerializeField]
    private GameObject ExperienceOrbPrefab;

    [SerializeField]
    private GameObject CoinPrefab;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private GameObject MeleeTrigger;
    private MeleeTriggerHandler meleeTriggerHandler;
    public float MeleeDamage = 5.0f;

    private NavMeshAgent navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = Speed;

        if (!IsRanged)
        {
            MeleeTrigger = transform.GetChild(0).gameObject;
            meleeTriggerHandler = MeleeTrigger.GetComponent<MeleeTriggerHandler>();
            meleeTriggerHandler.Damage = MeleeDamage;
        }
    }

    // Update is called once per frame
    void Update()
    {

        playerPosition = GameObject.FindWithTag("Player").transform;

        if (CanMove)
        {
            Move();
        }

        checkAttackConditions();

        if (Health <= 0)
        {
            int energyToSpawn = (int)Random.Range(ExperienceDropsMean - DropRange, ExperienceDropsMean + DropRange);
            int moneyToSpawn = (int)Random.Range(MoneyDropsMean - DropRange, MoneyDropsMean + DropRange);

            for (int i = 0; i < energyToSpawn; i++)
            {
                SpawnLoot(true);
            }

            for (int j = 0; j < moneyToSpawn; j++)
            {
                SpawnLoot(false);
            }

            Room roomRef = GameManager.Instance.CurrentRoomReference;
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
        float randomDistance = Random.Range(0.5f, DropsSpawnRadius);

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
        if (CanMove)
        {
            if (!IsRanged)
            {
                navMeshAgent.destination = playerPosition.position;
            }


        }
    }

    public virtual void checkAttackConditions()
    {
        if (CanAttack)
        {

            float distance = Vector2.Distance(transform.position, playerPosition.position);

            if (distance <= MinimumAttackRange)
            {
                Attack();
            }
        }
    }
    private IEnumerator AttackCoroutine()
    {
        CanAttack = false;
        // Implement lunge attack logic here
        CanMove = false;
        
        navMeshAgent.isStopped = true;
        navMeshAgent.updatePosition = false;

        if (!IsRanged)
        {
            MeleeTrigger.SetActive(true);
        }

        Vector2 startPos = transform.position;
        Vector2 playerPos = playerPosition.position;

        // 1. Calculate the ideal overshoot point behind the player
        Vector2 direction = (playerPos - startPos).normalized;
        Vector2 idealTargetPosition = playerPos + (direction * 2.0f);

        // 2. Project that target point onto the nearest valid point on the NavMesh
        Vector3 finalNavMeshTarget = startPos;
        NavMeshHit hit;


        Vector3 searchTarget3D = new Vector3(idealTargetPosition.x, idealTargetPosition.y, transform.position.z);
        
        // Sample position on the NavMesh
        if (NavMesh.SamplePosition(searchTarget3D, out hit, 2.0f, NavMesh.AllAreas))
        {
            finalNavMeshTarget = hit.position;
        }
        else
        {
            finalNavMeshTarget = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        }

        // Store final destination as a clean Vector2
        Vector2 finalTarget2D = finalNavMeshTarget;

        // 3. Smooth 2D Lunge Translation
        float elapsedTime = 0f;
        while (elapsedTime < 0.75f)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / 0.75f;
            float t = Mathf.Sin(percent * Mathf.PI * 0.5f);

            // Interpolate strictly on X and Y
            Vector2 currentPos2D = Vector2.Lerp(startPos, finalTarget2D, t);

            // Reapply the locked Z so it stays perfectly on its 2D plane
            transform.position = new Vector3(currentPos2D.x, currentPos2D.y, transform.position.z);

            yield return null;
        }

        // 4. Wrap up and resume NavMesh
        Vector3 finalPositions3D = new Vector3(finalTarget2D.x, finalTarget2D.y, transform.position.z);

        // Disable position updates briefly so the warp doesn't drag the transform down
        navMeshAgent.updatePosition = false;
        navMeshAgent.Warp(finalPositions3D);

        // FORCE the visual sprite to stay exactly where the 2D math calculated it
        transform.position = finalPositions3D;

        navMeshAgent.isStopped = false;
        navMeshAgent.updatePosition = true;

        // Perform lunge attack animation or movement here
        yield return new WaitForSeconds(AttackCooldown); // Wait for the lunge duration

        if (!IsRanged)
        {
            MeleeTrigger.SetActive(false);
            meleeTriggerHandler.HitPlayer = false; // Reset the hit flag for the next attack
        }

        CanMove = true;
        CanAttack = true;
    }

    public virtual void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }
}
