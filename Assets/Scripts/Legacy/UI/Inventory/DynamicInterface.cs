using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    /*
    public int xSpaceBetweenItem;
    public int numberOfColumn;
    public int ySpaceBetweenItem;
    public int xStart;
    public int yStart;
    public Vector3 prefabScale;
    public GameObject inventoryPrefab;
    public override void CreateSlots()
    {
        //itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
           
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.transform.localScale = prefabScale;

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            itemsDisplayed.Add(obj, inventory.container.items[i]);
        }
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + xSpaceBetweenItem * (i % numberOfColumn), yStart + (-ySpaceBetweenItem * (i / numberOfColumn)), 0f);
    }*/
}
