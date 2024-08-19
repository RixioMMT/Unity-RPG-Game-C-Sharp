using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public bool isSword; // New property to indicate if the item is a sword
}