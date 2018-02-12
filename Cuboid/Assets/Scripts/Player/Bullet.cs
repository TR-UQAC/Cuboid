using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 25f;
    public float maxTimeToLive = 2f;
    public bool facingRight;
    public int dmg;

    public LayerMask noHit;
    public LayerMask dommageHit;


    private float direction;
    private Rigidbody2D m_Rigidbody2D;

    public float eForce = 0;
    public float eRadius = 0;
    public float upwardsModifier = 0;

    private Transform myTransform;
    // Use this for initialization
    void Start () {
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>() as Rigidbody2D;
        myTransform = transform;

        if (!facingRight)
        {
            speed = speed * (-1);
        }
        else
        {
            Vector3 theScale = myTransform.localScale;
            theScale.x *= -1;
            myTransform.localScale = theScale;
        }

        m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);

        Destroy(gameObject, maxTimeToLive);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject go = other.gameObject;
        if (noHit != (noHit | (1 << go.layer))) {
            
            if (dommageHit == (dommageHit | (1 << go.layer)) && myTransform != null) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(myTransform.position, eRadius, dommageHit);
                foreach (Collider2D nerbyObject in colliders) {

                //for (int i = colliders.Length-1; i < 0; i--) {
                    Rigidbody2DExt.AddExplosionForce(nerbyObject.GetComponent<Rigidbody2D>(), eForce, myTransform.position, eRadius, upwardsModifier);

                    Personnages en = nerbyObject.GetComponent<Personnages>() as Personnages;
                    en.DommagePerso(dmg);
                }
            }

            //TODO: Effet particule de contact
            Destroy(gameObject);
        }
    }
}
