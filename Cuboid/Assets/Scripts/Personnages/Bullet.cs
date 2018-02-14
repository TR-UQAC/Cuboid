using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    
    public float speed = 25f;
    public float maxTimeToLive = 2f;
    public bool facingRight;
    public int dmg;

    public  Vector2 direction = new Vector2(0,0);

    public LayerMask noHit;
    public LayerMask dommageHit;

    private Rigidbody2D m_Rigidbody2D;

    public DegatAttaque statAttaque;
    private Transform myTransform;

    public Transform effetExplosion;
    // Use this for initialization
    void Start () {
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>() as Rigidbody2D;
        myTransform = transform;

        Vector3 theScale = myTransform.localScale;
        theScale.x *= -1;
        myTransform.localScale = theScale;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        m_Rigidbody2D.velocity = speed * direction.normalized;

        Destroy(gameObject, maxTimeToLive);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject go = other.gameObject;
        if (noHit != (noHit | (1 << go.layer))) {
            
            if (dommageHit == (dommageHit | (1 << go.layer)) && myTransform != null) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(myTransform.position, statAttaque.eRadius, dommageHit);
                foreach (Collider2D nerbyObject in colliders) {

                    if (statAttaque.ePower != 0) {
                        Rigidbody2DExt.AddExplosionForce(nerbyObject.GetComponent<Rigidbody2D>(), statAttaque.ePower, myTransform.position, statAttaque.eRadius, statAttaque.upwardsModifier);

                        if (effetExplosion != null) {
                            Transform clone = Instantiate(effetExplosion, myTransform.position, myTransform.rotation) as Transform;
                            ShockWaveForce wave = clone.GetComponent<ShockWaveForce>();
                            wave.radius = statAttaque.eRadius;

                            Destroy(clone.gameObject, 1f);
                        }
                    }
                    Personnages en = nerbyObject.GetComponent<Personnages>() as Personnages;
                    en.DommagePerso(dmg);
                }
            }

            //TODO: Effet particule de contact
            Destroy(gameObject);
        }
    }
}
