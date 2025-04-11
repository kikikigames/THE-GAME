using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Inventory : MonoBehaviour,ISerializationCallbackReceiver
{
    public InventorySlot[] Slots = new InventorySlot[5];
    [SerializeField] private Transform playerHoldPosition;
    public int currentSlotId;
    int lastSlotId;
    public bool canAddItem;
    void Start()
    {
        currentSlotId=0 ;
        lastSlotId=0;
        canAddItem=true;
    }
    void Update()
    {
        ChooseSlot();
    }
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
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].slotId = i;
        }
    }
    public void OnBeforeSerialize()
    {
    }
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
    public void ChangeItem()
    {
        if(lastSlotId != -1)
        {
            playerHoldPosition.GetChild(lastSlotId).gameObject.SetActive(false);
        }
        playerHoldPosition.GetChild(currentSlotId).gameObject.SetActive(true);
    }

    public bool CanAdd()
    {
        return canAddItem= Slots[currentSlotId].IsEmpty();
    }
}
