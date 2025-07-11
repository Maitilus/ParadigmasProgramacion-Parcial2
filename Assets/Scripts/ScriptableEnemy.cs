using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableEnemy", menuName = "Scriptable Objects/ScriptableEnemy")]
public class ScriptableEnemy : ScriptableObject
{
    //Scriptable object nos permite definir una serie de atributos o estadisticas que luego podremos usar como Molde para nuestros objetos
    [SerializeField] private string EnemyName;
    [SerializeField] private float Damage;
    [SerializeField] private float Health;

    public string GSEnemyName
        { get { return EnemyName; } }
    public float GSDamage
        { get { return Damage; } }
    public float GSHealth
        { get { return Health; } }

    public void PrintData()
    {
        Debug.Log($"Name: {EnemyName}");
        Debug.Log($"Damage: {Damage}");
        Debug.Log($"Health: {Health}");
    }
}
