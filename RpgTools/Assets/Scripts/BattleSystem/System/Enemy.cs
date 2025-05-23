using UnityEngine;

public class Enemy : Character
{
    private EnemyScriptableObject _enemyData;

    public Enemy(EnemyScriptableObject enemyData) : base(enemyData)
    {
        _enemyData = enemyData;
    }
}
