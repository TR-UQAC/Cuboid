using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour {

    public LayerMask m_NoHit;
    public int m_dmg;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject balle = collision.gameObject;

        Debug.Log("ca ricoche");

        /*if (balle.layer == 12)// le layer de bullet, si ça chie, vérifier si ces toujours le bon layer
        {
            Bullet ballSc = balle.GetComponent<Bullet>();

            ballSc.noHit = m_NoHit;
            ballSc.dmg = m_dmg;

            foreach (ContactPoint2D touche in collision.contacts)
            {
                Vector2 ricoche = Vector2.Reflect(collision.transform.forward, touche.normal);
                Debug.Log(ricoche.ToString());
            }

            //Rigidbody2D rballe = balle.GetComponent<Rigidbody2D>() as Rigidbody2D;
            
        }*/
    }
}
