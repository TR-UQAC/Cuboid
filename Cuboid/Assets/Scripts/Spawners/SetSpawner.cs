using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawner : MonoBehaviour {
    public GameObject spawnPrefab;
    public Transform ennemisFolder;

    public void SetSpawn() {
        foreach (Transform child in ennemisFolder) {
            //print("Foreach loop: " + child);

            EnnemiSpawner newSpawn = Instantiate(spawnPrefab, child.position, child.rotation).GetComponent<EnnemiSpawner>() as EnnemiSpawner;
            newSpawn.transform.parent = gameObject.transform;

            newSpawn.thisEnnemi = child.GetComponent<Ennemis>() as Ennemis;

            //TODO: trouver un moyen automatique d'initialiser le prefab
            //Debug.Log(PrefabUtility.FindPrefabRoot(child.gameObject));
            //newSpawn.ennemi = PrefabUtility.FindPrefabRoot(child.gameObject).transform;

            //Set the path as within the Assets folder, and name it as the GameObject's name with the .prefab format
            /*
            string localPath = "Assets/Prefabs/Ennemi" + child.name + ".prefab";
            Debug.Log(localPath);
            
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject));
            newSpawn.ennemi = prefab.transform;
            */
        }
    }

    public void DestroyAllChild() {
        int children = transform.childCount-1;
        for (int i = children; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);
    }
}
