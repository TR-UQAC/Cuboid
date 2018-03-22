using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    #region variable
    public float speed = 25f;
    public float maxTimeToLive = 2f;
    public bool facingRight;
    public int dmg;

    public  Vector2 direction = new Vector2(0,0);

    public LayerMask noHit;
    public LayerMask dommageHit;

    private Rigidbody2D m_Rigidbody2D;

    public DegatAttaque statAttaque;
    protected Transform myTransform;

    public Transform effetExplosion;
#endregion
    // Use this for initialization

    void Start () {
        
        m_Rigidbody2D = GetComponent<Rigidbody2D>() as Rigidbody2D;
        myTransform = transform;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        m_Rigidbody2D.velocity = speed * direction.normalized;

        Destroy(gameObject, maxTimeToLive);
        SetUpLaser();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        GameObject go = other.gameObject;
        if (noHit != (noHit | (1 << go.layer)))
        {
            /*if(go.layer == 15)
            {
                Debug.Log("Ricoche dans trigger");
                return;
            }*/

            //if (statAttaque.ePower == 0 || statAttaque.eRadius == 0) {
            if (dommageHit == (dommageHit | (1 << go.gameObject.layer)))
            {
                Personnages en = go.GetComponent<Personnages>() as Personnages;
                en.DommagePerso(dmg);
            }
            //}else if
            if (myTransform != null)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(myTransform.position, statAttaque.eRadius, dommageHit);
                foreach (Collider2D nerbyObject in colliders)
                {
                   if (dommageHit == (dommageHit | (1 << nerbyObject.gameObject.layer)))
                   {
                        if (Rigidbody2DExt.AddExplosionForce(nerbyObject.GetComponent<Rigidbody2D>(), statAttaque.ePower, myTransform.position, statAttaque.eRadius, statAttaque.upwardsModifier))
                        {
                           // if (nerbyObject == other)
                               // continue;

                            Personnages en = nerbyObject.GetComponent<Personnages>() as Personnages;
                            en.DommagePerso(dmg);
                        }
                   }
                }

                if (effetExplosion != null && statAttaque.eRadius != 0)
                {
                    Transform clone = Instantiate(effetExplosion, myTransform.position, myTransform.rotation) as Transform;
                    ShockWaveForce wave = clone.GetComponent<ShockWaveForce>();
                    wave.radius = statAttaque.eRadius * 1.3f;
                    Destroy(clone.gameObject, 1f);
                }
            }

            //TODO: Effet particule de contact
            Destroy(gameObject);
        }
    }

    //  méthode qui sera appeler pour paramétré le laser
    protected virtual void SetUpLaser()
    { }
}
