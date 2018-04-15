using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSpawner : MonoBehaviour {

    public Transform spawnPrefab;
    public Transform ennemi;
    public Ennemis thisEnnemi;
    
    private Transform tr;
	// Use this for initialization
	void Start () {
        tr = this.transform;
        SpawnEnnemi();
    }
	
	public void SpawnEnnemi() {
        if (thisEnnemi != null) {
            thisEnnemi.transform.position = tr.position;
            thisEnnemi.SoinPerso(999999);
        } else if (ennemi != null)
            thisEnnemi = Instantiate(ennemi, tr.position, tr.rotation).GetComponent<Ennemis>();

        if (thisEnnemi!= null && thisEnnemi.enabled)
            SpawnParticule();
    }

    private void SpawnParticule() {
        if (spawnPrefab != null) {
            Transform clone = Instantiate(spawnPrefab, tr.position, tr.rotation) as Transform;
            Destroy(clone.gameObject, 3f);
        }
    }
}
