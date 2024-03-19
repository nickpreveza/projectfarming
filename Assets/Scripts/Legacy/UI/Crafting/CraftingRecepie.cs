using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Object", menuName = "Crafting/Recipe")]
public class CraftingRecepie : ScriptableObject
{
    public int ID;
    public string objectName;
    public Sprite uiDisplay;
    public Item resultItem;
    public Item[] requiredItems;
    public int LevelToCraft;
}
