using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName ="New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    /*
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory container;
    

 
    public void AddItem(Item _item, int _amount)
    {
        if (_item.buffs.Length > 0)
        {
            FirstEmptySlot(_item, _amount);
            return;
        }
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].ID == _item.ID)
            {
                container.items[i].AddAmount(_amount);
                return;
            }
        }
        FirstEmptySlot(_item, _amount);
    }
    public InventorySlot FirstEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if (container.items[i].ID <= -1)
            {
                container.items[i].UpdateSlot(_item.ID,_item,_amount);
                return container.items[i];
            }
        }
        /// setup functionality for full inevntorry
        Debug.Log("Inventory is Full");
        return null;
    }
    public void MoveItem(InventorySlot item1, InventorySlot item2 )
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }
    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            if ( container.items[i].item==_item)
            {
                container.items[i].UpdateSlot(-1, null, 0);
            }
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)));
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < container.items.Length; i++)
            {
                container.items[i].UpdateSlot(newContainer.items[i].ID, newContainer.items[i].item, newContainer.items[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }
    */
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[30];
}

[System.Serializable]
public class InventorySlot
{
    public UserInterface parent;
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void  UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;

    }


    public void AddAmount (int value)
    {
        amount += value;
    }
}