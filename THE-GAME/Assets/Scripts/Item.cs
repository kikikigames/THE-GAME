using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    [TextArea(15, 20)]
    public string description;
    public Sprite icon;
}
