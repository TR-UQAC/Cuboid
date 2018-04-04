using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AllSpawners))]
public class ObjectBuilderEditor2 : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        AllSpawners myScript = (AllSpawners)target;
        if (GUILayout.Button("Spawn all ennemis")) {
            myScript.SpawnAllEnnemis();
        }
    }
}