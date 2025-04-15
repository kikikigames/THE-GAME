using UnityEngine;

public class Loot : MonoBehaviour, IInteractable
{
    [Header("Loot")]
    [SerializeField] public Item item;
    [SerializeField] public string itemNameText;
    [SerializeField] public string itemDescriptionText;
    void Start()
    {
        this.name = item.itemName;
        itemNameText = item.itemName;
        itemDescriptionText = item.description;
    }
    public void SetTexts()
    {
        itemNameText = item.itemName;
        itemDescriptionText = item.description;
    }
    public void InteractWithoutPressingButton()
    {
        //Itemin ismi gozukebilir
    }
    public void InteractWithPressingButton(Inventory playerInventory)
    {
        playerInventory.AddItem(item);
    }
}
