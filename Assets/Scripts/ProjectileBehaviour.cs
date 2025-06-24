using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float Lifetime;
    [SerializeField] private float Damage;
    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Lifetime -= Time.deltaTime;

        if(Lifetime <= 0) { Destroy(gameObject); }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * Speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Temple");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<ITakeDamage>()?.ReduceHealth(Damage);
        }
    }
}
