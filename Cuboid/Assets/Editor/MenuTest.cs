using UnityEditor;
using UnityEngine;

public class MenuTest : MonoBehaviour {

    [MenuItem("GameObject/2D Object/Ennemis/Ennemi IA")]
    static void AddEnnemiIA() {
        PrefabEnnemi("Ennemi_IA");
    }

    [MenuItem("GameObject/2D Object/Ennemis/Ennemi Patrouille")]
    static void AddEnnemiPatrouille() {
        PrefabEnnemi("Ennemi_Patrouille");
    }

    [MenuItem("GameObject/2D Object/Ennemis/Ennemi Vide")]
    static void AddEnnemiVide() {
        PrefabEnnemi("Ennemi_Vide");
    }


    static void PrefabEnnemi(string nom) {
        GameObject prefab = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<Object>("Assets/Prefabs/Ennemi/"+nom+".prefab")) as GameObject;
        prefab.name = nom;
        if (Selection.activeTransform != null) {
            prefab.transform.SetParent(Selection.activeTransform, false);
        }
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.localEulerAngles = Vector3.zero;
    }
}
