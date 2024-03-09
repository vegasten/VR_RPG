using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public uint ItemId;
    public Sprite InventorySprite;
    public string DisplayName;
    public GameObject DropItemPrefab;
    public bool Stackable;
    public int StackSize;
}
