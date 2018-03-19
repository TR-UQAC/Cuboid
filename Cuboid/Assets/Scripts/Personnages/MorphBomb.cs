using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphBomb : MonoBehaviour {

    public float explosionTimer = 1.5f;
    public int explosionForce = 5000;
    public float destroyRadius = 1.0f;
    public GameObject explosionEffect;
    
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
                item.attachedRigidbody.AddForce(new Vector2(0, explosionForce));
            }

            //Trouve les objets destructibles
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, destroyRadius);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.CompareTag("Destructible"))
                {
                    Destroy(col.gameObject);
                }            
            }

            GameObject clone = Instantiate(explosionEffect, transform.position, transform.rotation);
            clone.GetComponent<ShockWaveForce>().radius = 2f;
            Destroy(clone.gameObject, 1f);

            FindObjectOfType<AudioManager>().Play("BombExplosion");

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
