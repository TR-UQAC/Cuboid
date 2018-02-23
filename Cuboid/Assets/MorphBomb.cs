using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphBomb : MonoBehaviour {

    public float explosionTimer = 1.5f;
    public int explosionForce = 5000;

    private float spawnTime = 0f;
    private List<Collider2D> listCollider = new List<Collider2D>();

	void Start ()
    {
        spawnTime = Time.time;
	}
	

	void Update ()
    {
        if ((Time.time - spawnTime) > explosionTimer)
        {
            //Applique la force de jump au player
            foreach (Collider2D item in listCollider)
            {
                Debug.Log(item.ToString());

                item.attachedRigidbody.AddForce(new Vector2(0, explosionForce));
            }

            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!listCollider.Contains(other))
        {
            listCollider.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (listCollider.Contains(other))
        {
            listCollider.Remove(other);
        }
    }
}
