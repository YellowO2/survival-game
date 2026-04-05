using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArenaBounds))]
public class ArenaBoundsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ArenaBounds arenaBounds = (ArenaBounds)target;

        GUILayout.Space(20);
        EditorGUILayout.LabelField("Editor Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate / Update Walls", GUILayout.Height(30)))
        {
            arenaBounds.ClearWalls();
            arenaBounds.UpdateBounds(true);
            EditorUtility.SetDirty(arenaBounds);
        }

        if (GUILayout.Button("Clear Walls", GUILayout.Height(25)))
        {
            arenaBounds.ClearWalls();
            EditorUtility.SetDirty(arenaBounds);
        }
    }
}
