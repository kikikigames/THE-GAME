using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    public InventorySlot[] Slots = new InventorySlot[5];
    [SerializeField] private Transform playerHoldPosition;
    //Variables
    public int currentSlotId;
    int lastSlotId;
    //Bools
    public bool canAddItem;
    void Start()
    {
        currentSlotId=0 ;
        lastSlotId=0;
        canAddItem=true;
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].slotId = i;
        }
    }
    void Update()
    {
        ChooseSlot();
    }
    /// <summary>
    /// Slot seçimi için tuşlara basıldığında çağrılır. 1-5 arası slotları seçer.
    /// </summary>
    private void ChooseSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            lastSlotId = currentSlotId;
            currentSlotId = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            lastSlotId = currentSlotId;
            currentSlotId = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            lastSlotId = currentSlotId;
            currentSlotId = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            lastSlotId = currentSlotId;
            currentSlotId = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            lastSlotId = currentSlotId;
            currentSlotId = 4;
        }
        ChangeSelectedSlotColor();
        ChangeItem();
    }
    /// <summary>
    /// Slotlara item ekler. Eğer slot doluysa item eklenemez.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if(CanAdd())
        {
            Slots[currentSlotId].UpdateSlot(item, item.id, currentSlotId);
            Slots[currentSlotId].gameObject.transform.GetChild(0).GetComponent<Image>().sprite=item.icon;
            Slots[currentSlotId].gameObject.GetComponent<Image>().color=new Color(1, 1, 1, 1);
        }
        else
        {
            Debug.Log("Slot is full");
        }
    }
    /// <summary>
    /// Slotlardan item siler. Eğer slot boşsa item silinemez.
    /// </summary>
    public void DropItem()
    {
        if (Slots[currentSlotId].IsEmpty())
        {
            Debug.Log("Slot is empty");
        }
        else
        {
            Slots[currentSlotId].ClearSlot();
            Slots[currentSlotId].gameObject.transform.GetChild(0).GetComponent<Image>().sprite=null;
            Slots[currentSlotId].gameObject.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            Slots[currentSlotId].gameObject.transform.GetChild(0).GetComponent<Image>().color=new Color(1, 1, 1, 1);
            ChangeItem();
            Destroy( playerHoldPosition.GetChild(currentSlotId).GetChild(0).gameObject);
        }
    }
    /// <summary>
    /// Slotların rengini değiştirir. Seçilen slotun rengi değişir.
    /// </summary>
    private void ChangeSelectedSlotColor()
    {
        if (lastSlotId != -1) 
        {
            Slots[lastSlotId].gameObject.GetComponent<Image>().color=new Color(1, 1, 1, 1);
            Slots[lastSlotId].gameObject.transform.GetChild(0).GetComponent<Image>().color=new Color(1, 1, 1, 1);
        }
        Slots[currentSlotId].gameObject.GetComponent<Image>().color=new Color(1, 0.8431373f, 0.6666667f, 1);
        Slots[currentSlotId].gameObject.transform.GetChild(0).GetComponent<Image>().color=new Color(1, 0.8431373f, 0.6666667f, 1);
    }
    /// <summary>
    /// Elle tutulan item aktif olur. Son ele alinan slotun itemi gizlenir.
    /// </summary>
    public void ChangeItem()
    {
        if(lastSlotId != -1)
        {
            playerHoldPosition.GetChild(lastSlotId).gameObject.SetActive(false);
        }
        playerHoldPosition.GetChild(currentSlotId).gameObject.SetActive(true);
    }
    /// <summary>
    /// Item eklenebilir mi? Kontrol eder.
    /// </summary>
    /// <returns></returns>
    public bool CanAdd()
    {
        return canAddItem= Slots[currentSlotId].IsEmpty();
    }
}
