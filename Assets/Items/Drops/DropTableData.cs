using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "ScriptableObjects/DropTable", order = 1)]
public class DropTableData : ScriptableObject
{
    public TableItem[] TableItems;

    [Serializable]
    public class TableItem
    {
        public uint ItemId;
        public int Weight;   
    }
}
