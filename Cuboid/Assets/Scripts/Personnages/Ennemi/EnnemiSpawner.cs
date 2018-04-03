using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiSpawner : MonoBehaviour {

    public Transform ennemi;
    public Ennemis thisEnnemi;

	// Use this for initialization
	void Start () {
        SpawnEnnemi();
    }
	
	public void SpawnEnnemi() {
        if(thisEnnemi == null && ennemi != null)
            thisEnnemi = Instantiate(ennemi, transform.position, transform.rotation).GetComponent<Ennemis>();
    }
}
