using UnityEngine;

public interface IInteractable 
{
    void InteractWithoutPressingButton();
    void InteractWithPressingButton(Inventory inventory);
}
