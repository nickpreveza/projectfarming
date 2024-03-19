using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Crafting/RecepieDatabase")]
public class RecipeDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public CraftingRecepie[] recepies;
    public Dictionary<int, CraftingRecepie> GetItem = new Dictionary<int, CraftingRecepie>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < recepies.Length; i++)
        {
            recepies[i].ID = i;
            GetItem.Add(i, recepies[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, CraftingRecepie>();
    }
}
