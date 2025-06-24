using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DartTrap : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float timeBetweenShots;



    public void Activate()
    {
        StartCoroutine(BurstFire());
    }

    //usando una corrutina podemos hacer intervalos entre lineas de codigo
    private IEnumerator BurstFire()
    {
        Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        yield return new WaitForSeconds(timeBetweenShots);
        Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        yield return new WaitForSeconds(timeBetweenShots);
        Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
    }
}