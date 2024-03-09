using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public ItemData Leather;
    public ItemData Rock;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public ItemData GetItem(uint ItemId)
    {
        switch (ItemId)
        {
            case 0:
                return Leather;
            case 1:
                return Rock;
        }

        return null;
    }
}
