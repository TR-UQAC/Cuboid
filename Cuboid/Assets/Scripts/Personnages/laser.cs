using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class laser : Bullet {

    private SpriteRenderer m_sCorps;
    private SpriteRenderer m_sImpact;
    private ParticleSystem ps;
    private ParticleSystem loadPS;
    private Transform m_impact;
    private Transform m_effet;
    private Transform m_corps;
    private bool m_charge = false;

    // autre
    RaycastHit2D hit;
    float range = 100.0f;

    public LayerMask m_RayCastHit;
    public float m_LargeurLaser = 2.0f;

    private void Update()
    {

        //hit = Physics2D.Raycast(transform.position, direction, range, m_RayCastHit);
        hit = Physics2D.CircleCast(transform.position, 0.4f, direction, range, m_RayCastHit);
        if (hit)
        {
            float dist = hit.distance + 0.5f;

            var m = ps.main;
            m.startLifetime = dist / 8.5f;

            //m_effet.localPosition = new Vector3(dist - 1.6f, 0.0f);

            m_sCorps.size = new Vector2(dist, m_LargeurLaser);
            m_impact.localPosition = new Vector3(dist - 1.6f, 0.0f);
            if (hit.transform.tag == "Player" && m_charge == true)
            {
                OnTriggerEnter2D(hit.collider);
            }
        }
    }


    //  méthode qui sera appeler pour paramétré le laser
    protected override void SetUpLaser()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "sparkLaser")
            {
                m_effet = child;
                ps = child.GetComponent<ParticleSystem>() as ParticleSystem;
                continue;
            }

            if(child.name == "Corps")
            {
                m_corps = child;
                m_sCorps = child.GetComponent<SpriteRenderer>() as SpriteRenderer;
                continue;
            }

            if(child.name == "Impact")
            {
                m_impact = child;
                m_sImpact = child.GetComponent<SpriteRenderer>() as SpriteRenderer;
                continue;
            }

            if(child.name == "LoadLaser")
            {
                loadPS = child.GetComponent<ParticleSystem>() as ParticleSystem;
                continue;
            }
        }

        hit = Physics2D.CircleCast(transform.position, 0.4f, direction, range, m_RayCastHit);
        if (hit)
        {
            float dist = hit.distance + 0.5f;

            var m = ps.main;
            m.startLifetime = dist / 8.5f;
            


            //m_effet.localPosition = new Vector3(dist - 1.6f, 0.0f);

            m_sCorps.size = new Vector2(dist, m_LargeurLaser);
            m_impact.localPosition = new Vector3(dist - 1.6f, 0.0f);
        }

        Sequence charge = DOTween.Sequence();

        charge.Append(m_sCorps.DOFade(1.0f, 2.0f).SetEase(Ease.InExpo));
        charge.Insert(0.0f, m_sImpact.DOFade(1.0f, 2.0f).SetEase(Ease.InExpo));
        charge.InsertCallback(0.0f, () =>
        {
            FindObjectOfType<AudioManager>().Play("LaserBossStart");
        });
        charge.InsertCallback(1.0f, () =>
        {
            m_effet.gameObject.SetActive(true);
        });
        charge.InsertCallback(3.0f, () =>
        {
            m_charge = true;
            FindObjectOfType<AudioManager>().Play("LaserBossMilieu");
        });
        charge.InsertCallback(8.5f, () =>
        {
            FindObjectOfType<AudioManager>().Play("LaserBossFin");
        });


        charge.Play();

        Invoke("Disparait", maxTimeToLive - 2.0f);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
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
            //Destroy(gameObject);
        }
    }


    public void Disparait()
    {
        Sequence die = DOTween.Sequence();

        die.Append(transform.DOScaleY(0.2f, 2.0f));
        die.Insert(0.0f, m_sCorps.DOFade(0.0f, 2.0f).SetEase(Ease.InExpo));
        die.Insert(0.0f, m_sImpact.DOFade(0.0f, 2.0f).SetEase(Ease.InExpo));
        die.InsertCallback(1.0f, (() =>
        {
            ps.Stop();
            loadPS.Stop();

            //m_effet.gameObject.SetActive(false);
            //  !*! run son laser down
        }));

        die.Play();
    }

    /*
     * no hit normal = tout cocher sauf player et obstable
     * Dommage hit = player
     * 
     * */
}
