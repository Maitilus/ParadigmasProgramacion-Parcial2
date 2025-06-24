using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    #region Variables


    private NavMeshAgent navMesh;
    private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    //Attacking
    [SerializeField] public float attackCD;
    [SerializeField] private bool CanAttack;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;

    //States
    [SerializeField] private float LOSRange, attackRange;
    [SerializeField] private bool playerIsInLOS, playerIsInAttackRange;
    [SerializeField] private float unawareVision, awareVision;

    #endregion

    private void Awake()
    {
        //Get References
        player = GameObject.Find("Playerbody").transform;
        navMesh = GetComponent<NavMeshAgent>();
        CanAttack = true;
    }

    private void Update()
    {
        //Check for LOS and Attack Range
        playerIsInLOS = Physics.CheckSphere(transform.position, LOSRange, whatIsPlayer);
        playerIsInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Set State
        if (!playerIsInLOS && !playerIsInAttackRange)
        {
            Patrol();
            LOSRange = unawareVision;
        }
        if (playerIsInLOS && !playerIsInAttackRange)
        {
            Chase();
            LOSRange = awareVision;
        }
        if (playerIsInLOS && playerIsInAttackRange)
        {
            Attack();
            LOSRange = awareVision;
        }
    }

    private void PathFind()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) { walkPointSet = true; }
    }

    private void ResetAttack()
    {
        CanAttack = true;
    }

    #region States

    private void Patrol()
    {
        if (!walkPointSet) { PathFind(); }

        if (walkPointSet) { navMesh.SetDestination(walkPoint); }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1) { walkPointSet = false; }
    }

    private void Chase()
    {
        navMesh.SetDestination(player.position);
    }

    private void Attack()
    {
        navMesh.SetDestination(transform.position);

        transform.LookAt(player);

        if (CanAttack)
        {
            CanAttack = false;

            Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);

            Invoke(nameof(ResetAttack), attackCD);
        }
    }

    #endregion

}