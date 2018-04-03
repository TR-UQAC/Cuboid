using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EnnemiSpawner))]
public class ObjectBuilderEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        EnnemiSpawner myScript = (EnnemiSpawner)target;
        if (GUILayout.Button("Build Object")) {
            myScript.SpawnEnnemi();
        }
    }
}