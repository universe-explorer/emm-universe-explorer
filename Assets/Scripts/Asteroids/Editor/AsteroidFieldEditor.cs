using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkManager))]
public class AsteroidFieldEditor : Editor
{
    ChunkManager meteorSpawnerField;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawSettingsEditor(meteorSpawnerField.AsteroidSettings, ref meteorSpawnerField.SettingsFoldout);
    }

    void DrawSettingsEditor(Object settings, ref bool foldout)
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            if (foldout)
            {
                Editor editor = CreateEditor(settings);
                editor.OnInspectorGUI();
            }
        }
    }

    private void OnEnable()
    {
        meteorSpawnerField = (ChunkManager)target;
    }
}
