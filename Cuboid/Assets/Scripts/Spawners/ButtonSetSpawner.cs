using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SetSpawner))]
public class ButtonSetSpawner : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        SetSpawner myScript = (SetSpawner)target;
        if (GUILayout.Button("Instancier tout les spawner Ennemis")) {
            myScript.SetSpawn();
        }

        if (GUILayout.Button("Detruire tout les Child")) {
            myScript.DestroyAllChild();
        }
    }
}