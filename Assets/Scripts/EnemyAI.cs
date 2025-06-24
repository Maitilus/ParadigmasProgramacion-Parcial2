using NUnit.Framework;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

//Gracias a la Abstraccion es posible crear el comportamiento base de un enemigo y luego hacer variantes ddel mismo
public abstract class EnemyAI : MonoBehaviour
{
    #region Variables

    //Variables protegidas pueden ser Heredadas desde la superclase para no tener que declararlas multiples veces
    protected NavMeshAgent navMesh;
    protected Transform player;
    [SerializeField] protected LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    [SerializeField] protected Vector3 walkPoint;
    [SerializeField] protected bool walkPointSet;
    [SerializeField] protected float walkPointRange;

    //Attacking
    [SerializeField] protected float attackCD;
    [SerializeField] protected bool CanAttack;
    [SerializeField] protected Transform bulletSpawn;
    [SerializeField] protected GameObject bulletPrefab;

    //States
    [SerializeField] protected float LOSRange, attackRange;
    [SerializeField] protected bool playerIsInLOS, playerIsInAttackRange;
    [SerializeField] protected float unawareVision, awareVision;

    #endregion

    protected virtual void Awake()
    {
        //Get References
        player = GameObject.Find("Playerbody").transform;
        navMesh = GetComponent<NavMeshAgent>();
        CanAttack = true;
    }

    protected virtual void Update()
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

    protected virtual void PathFind()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) { walkPointSet = true; }
    }

    protected virtual void ResetAttack()
    {
        CanAttack = true;
    }

    #region States

    protected virtual void Patrol()
    {
        if (!walkPointSet) { PathFind(); }

        if (walkPointSet) { navMesh.SetDestination(walkPoint); }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1) { walkPointSet = false; }
    }

    protected virtual void Chase()
    {
        navMesh.SetDestination(player.position);
    }

    protected virtual void Attack()
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