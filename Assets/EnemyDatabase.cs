using UnityEngine;

public class EnemyDatabase : MonoBehaviour
{
    public static EnemyDatabase Instance;

    public EnemyData TestEnemy;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public EnemyData GetEnemy(uint id)
    {
        switch (id)
        {
            case 0:
                return TestEnemy;
        }
        return null;
    }

}
