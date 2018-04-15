using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    private BoxCollider2D trigger;

	void Awake()
    {
        trigger = GetComponent<BoxCollider2D>();
        trigger.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if (go.tag == "Player")
        {
            Debug.Log("Fin du jeu");
        }
    }

    public void Enable()
    {
        trigger.enabled = true;
    }
}
