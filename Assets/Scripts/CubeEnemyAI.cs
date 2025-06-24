using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


//Enemigo con el mismo comportamiento que el original pero variando la forma de ataque
public class CubeEnemyAI : EnemyAI
{
    [SerializeField] private float timeBetweenShots;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void PathFind()
    {
        base.PathFind();
    }

    protected override void ResetAttack()
    {
        base.ResetAttack();
    }

    protected override void Patrol()
    {
        base.Patrol();
    }

    protected override void Chase()
    {
        base.Chase();
    }

    //Este es un ejemplo de polimorfismo donde bajo las mismas condiciones el objeto se comporta de manera diferente
    protected override void Attack()
    {
        navMesh.SetDestination(transform.position);

        transform.LookAt(player);

        if (CanAttack)
        {
            CanAttack = false;

            StartCoroutine(BurstFire());

            Invoke(nameof(ResetAttack), attackCD);
        }
    }

    private IEnumerator BurstFire()
    {
        Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        yield return new WaitForSeconds(timeBetweenShots);
        Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        yield return new WaitForSeconds(timeBetweenShots);
        Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
    }
}

