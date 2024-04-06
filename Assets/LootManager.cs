using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    private EnemyDatabase _enemyDatabase;
    private ItemDatabase _itemDatabase;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _enemyDatabase = EnemyDatabase.Instance;
        _itemDatabase = ItemDatabase.Instance;
    }

    public void CreateDrop(uint enemyId, Vector3 position)
    {
        var prefab = GetDropPrefab(enemyId);
        Instantiate(prefab, position, Quaternion.identity);
    }

    public GameObject GetDropPrefab(uint enemyId)
    {
        var dropTable = _enemyDatabase.GetEnemy(enemyId).DropTable;
        var itemId = RollLootTable(dropTable);
        return _itemDatabase.GetItem(itemId).DropItemPrefab;
    }

    private uint RollLootTable(DropTableData dropTable)
    {
        var totalWeight = dropTable.TableItems.Sum(x => x.Weight);
        var numberOfWeights = dropTable.TableItems.Length;

        var indexed = new List<int>
        {
            dropTable.TableItems[0].Weight
        };

        for (int i  = 1; i < numberOfWeights;  i++)
        {
            indexed.Add(indexed[i - 1] + dropTable.TableItems[i].Weight);
        }

        var randomNumber = Random.Range(0, totalWeight);

        var index = indexed.FindIndex(x => randomNumber < x);

        if (index == -1)
        {
            Debug.LogError("Could not retrieve drop.. This shouldn't happen");
        }

        return dropTable.TableItems[index].ItemId;
    }
}
