using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform itemHoldPos;
    [SerializeField] private Transform itemDropPos;
    [SerializeField] private GameObject lootPrefab;

    private IInteractable lastInteractableComp;
    private GameObject lastInteractedObj;
    [SerializeField] private float range;
    [SerializeField] private KeyCode itemTakeKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode itemDropKey = KeyCode.G;
    private bool canHit;

    private void Start()
    {
        lastInteractableComp = null;
        canHit = true;
    }
    private void Update()
    {
        Shoot();
        DropItem();
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*10, Color.green);     
    }
    private void DropItem()
    {
        if(Input.GetKeyDown(itemDropKey) &&playerInventory.Slots[playerInventory.currentSlotId].IsFull() )
        {
            GameObject droppedItem = Instantiate(lootPrefab, itemDropPos.position, itemDropPos.rotation);
            droppedItem.GetComponent<Loot>().item = playerInventory.Slots[playerInventory.currentSlotId].GetItem();
            droppedItem.GetComponent<Loot>().SetTexts();
            droppedItem.transform.SetPositionAndRotation(itemDropPos.position, itemDropPos.rotation);
            droppedItem.transform.SetParent(null);
            playerInventory.DropItem();
            
        }
    }
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            GameObject interactedObj = hit.transform.gameObject;
            IInteractable InteractedComp = hit.transform.GetComponent<IInteractable>();
            if (InteractedComp != null && canHit)
            {
                if (InteractedComp != lastInteractableComp)
                {
                    //Hitting to a new object
                    if (lastInteractableComp != null)
                    {
                        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*10, Color.red);     
                        InteractedComp.InteractWithoutPressingButton();
                    }
                }
                lastInteractableComp = InteractedComp;
                lastInteractedObj = interactedObj;
                //InteractableObj.InteractWithoutPressingButton(true);
               
                if (Input.GetKeyDown(KeyCode.Mouse0)&& playerInventory.CanAdd())
                {
                    InteractedComp.InteractWithPressingButton(playerInventory);
                    interactedObj.transform.SetPositionAndRotation(itemHoldPos.GetChild(playerInventory.currentSlotId).transform.position, itemHoldPos.GetChild(playerInventory.currentSlotId).transform.rotation);
                    interactedObj.transform.SetParent(itemHoldPos.GetChild(playerInventory.currentSlotId).transform);
                    lastInteractableComp = null;
                    lastInteractedObj = null;
                }
            }
        }
        else
        {
            if (lastInteractableComp != null)
            {
                lastInteractableComp.InteractWithoutPressingButton();
                lastInteractableComp = null;
                lastInteractedObj = null;
            }
        }
    }
}