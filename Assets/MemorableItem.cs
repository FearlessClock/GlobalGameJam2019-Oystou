using UnityEngine;

[CreateAssetMenu(fileName = "new memorableObject", menuName = "Memorable/StorageItem")]
public class MemorableItem : ScriptableObject
{
    public int ID;
    public GameObject worldItem;
}