using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 25f;
    public float maxTimeToLive = 2f;
    public bool facingRight;

    //[HideInInspector]
    public LayerMask noHit;
    //[HideInInspector]
    public LayerMask dommageHit;


    private float direction;
    private Rigidbody2D m_Rigidbody2D;

    // Use this for initialization
    void Start () {
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (!facingRight)
        {
            speed = speed * (-1);
        }
        else
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);

        Destroy(gameObject, maxTimeToLive);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject go = other.gameObject;
        if(noHit != (noHit | (1 << go.layer)))
        {
            if (dommageHit == (dommageHit | (1 << go.layer)))
            {
                Personnages en = (Personnages)go.GetComponent(typeof(Personnages));
                en.DommagePerso(100);
            }
            Destroy(gameObject);
        }
        
    }
}
