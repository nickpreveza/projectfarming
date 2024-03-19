using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GridInstance))]
public class GridSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridInstance source = (GridInstance)target;

        if (GUILayout.Button("Generate Grid"))
        {
            source.CreateGrid();
        }
        
    }
}
