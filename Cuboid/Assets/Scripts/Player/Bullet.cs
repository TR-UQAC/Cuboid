using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 25f;
    public float maxTimeToLive = 2f;
    public bool facingRight;
    public LayerMask NoHit;


    private float direction;
    private Rigidbody2D m_Rigidbody2D;

    // Use this for initialization
    void Start () {
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (!facingRight)
        {
            speed = speed * (-1);
        }

        m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);

        Destroy(gameObject, maxTimeToLive);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
    }
}
