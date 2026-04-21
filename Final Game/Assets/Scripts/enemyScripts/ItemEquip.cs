using UnityEngine;

public Transform handSocket; // Drag your 'Hand_Socket' here in Inspector
public GameObject itemToEquip;

void EquipItem()
{
    GameObject newItem = Instantiate(itemToEquip);
    newItem.transform.SetParent(handSocket); // Make it a child
    newItem.transform.localPosition = Vector3.zero; // Snap to socket
    newItem.transform.localRotation = Quaternion.identity; // Snap to rotation
}