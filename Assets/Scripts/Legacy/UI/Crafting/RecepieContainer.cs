using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
///using Unity.VisualScripting;

public class RecepieContainer : MonoBehaviour
{
    /*
    public GameObject parentObject;
    public GameObject recipePrefab;
    public RecipeDatabase recipeDatabase;
    public int cost;
    public float yStart;
    public float spacing;
    public GameObject[] slots;

    private void Awake()
    {
        for (int i = 0; i < recipeDatabase.recepies.Length; i++)
        {
            var obj = Instantiate(recipePrefab,Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = recipeDatabase.recepies[i].uiDisplay;
            obj.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = recipeDatabase.recepies[i].objectName;
            obj.GetComponent<CraftngWindowInteraction>().heldRecepie = recipeDatabase.recepies[i];
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(0 + 0 * (i % 1), yStart + (-spacing * (i / 1)), 0f);
    }*/
}
