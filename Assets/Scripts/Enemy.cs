using UnityEngine;

public class Enemy : MonoBehaviour, IInteraction, ITakeDamage
{
    [SerializeField] private ScriptableEnemy EnemyData;


    //Usando variables privadas junto a un get conseguimos variables de solo lectura
    //Encapsulamiento
    private string EnemyName;
    private float Damage;
    private float Health;

    private void Start()
    {
        EnemyName = EnemyData.GSEnemyName;
        Damage = EnemyData.GSDamage;
        Health = EnemyData.GSHealth;
    }

    public void Interact()
    {
        EnemyData.PrintData();

        Debug.Log($"Current Health: {Health}");
    }

    public void ReduceHealth(float IncomingDamage)
    {
        Health -= IncomingDamage;

        if (Health <= 0) { Destroy(gameObject); }
    }
}
