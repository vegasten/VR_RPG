using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]

public class EnemyData : ScriptableObject
{
    public DropTableData DropTable;
    public GameObject Prefab;
    public int Health;
}
