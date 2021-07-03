using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    public int shotDelay = 50;
    public float maxHealth = 100;
    public float damage = 10f;
    public float scale = 1;
    public float speedIdle = 0.1f;
    public float speedChasing = 0.3f;
    public float rotationSpeed = 0.05f;

    public float movementRadius = 200;
    public int movementDuration = 200;
    public float aggroRange = 300;
}
