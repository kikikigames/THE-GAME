using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot : MonoBehaviour 
{
    public int id;
    public Item item;
    public int slotId;
    public Sprite icon;

    public InventorySlot() => UpdateSlot(null, -1, -1);
    public Item GetItem()
    {
        return item.id >= 0 ? item:null ;
    }
    public void UpdateSlot(InventorySlot inventorySlot)
    {
        this.item = inventorySlot.item;
        this.id = inventorySlot.id;
        this.slotId = inventorySlot.slotId;
        this.icon = inventorySlot.icon; 
    }
    public void UpdateSlot(Item item, int id, int slotId)
    {
        this.item = item;
        this.id = id;
        this.slotId = slotId;
        this.icon = item != null ? item.icon : null;
    }
    public void ClearSlot()
    {
        this.item = null;
        this.id = -1;
        this.slotId = -1;
        this.icon = null;
    }
    public bool IsEmpty()
    {
        return item == null || item.id < 0;
    }
    public bool IsFull()
    {
        return item != null && item.id >= 0;
    }
}