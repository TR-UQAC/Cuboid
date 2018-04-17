using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class laser : Bullet {

    private SpriteRenderer m_sCorps;
    private SpriteRenderer m_sImpact;
    private ParticleSystem ps;
    private ParticleSystem loadPS;
    private Transform m_impact;
    private Transform m_effet;
    //private Transform m_corps;
    private Transform m_forwardLaser;
    private bool m_charge = false;
    private Sequence die;
    private Sequence charge;

    // autre
    RaycastHit2D hit;
    float range = 100.0f;

    public LayerMask m_RayCastHit;
    public float m_LargeurLaser = 1.8f;
    [Tooltip("Temps que met le laser pour charge son tir avant de faire des degat")]
    public float m_TempsLoad = 2.0f;
    [Tooltip("Temps de tire une fois le laser chargé, tout de suite après TempsLoad")]
    public float m_TempsTire = 5.5f;
    public AudioMixerGroup m_GroupeAudioLaser;

    private void Update()
    {

        //hit = Physics2D.Raycast(transform.position, direction, range, m_RayCastHit);

        Vector2 dir = m_forwardLaser.transform.position - transform.position;

        hit = Physics2D.CircleCast(transform.position, (m_LargeurLaser/2.0f), dir, range, m_RayCastHit);
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

            /*if(child.name == "Corps")
            {
                m_corps = child;
                m_sCorps = child.GetComponent<SpriteRenderer>() as SpriteRenderer;
                continue;
            }*/

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

            if (child.name == "forwardLaser")
            {
                m_forwardLaser = child;
                continue;
            }
        }

        hit = Physics2D.CircleCast(transform.position, (m_LargeurLaser / 2.0f), direction, range, m_RayCastHit);
        if (hit)
        {
            float dist = hit.distance + 0.5f;

            var m = ps.main;
            m.startLifetime = dist / 8.5f;
            


            //m_effet.localPosition = new Vector3(dist - 1.6f, 0.0f);

            m_sCorps.size = new Vector2(dist, m_LargeurLaser);
            m_impact.localPosition = new Vector3(dist - 1.6f, 0.0f);
        }

        ChargeLaser();

        //Invoke("Disparait", maxTimeToLive - 2.0f);
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


    

    private void ChargeLaser()
    {
        charge = DOTween.Sequence();

        charge.Append(m_sCorps.DOFade(1.0f, m_TempsLoad).SetEase(Ease.InExpo));
        charge.Insert(0.0f, m_sImpact.DOFade(1.0f, m_TempsLoad).SetEase(Ease.InExpo));
        charge.InsertCallback(0.0f, () =>
        {
            //  la durée du son m_TempsLoad
            m_GroupeAudioLaser.audioMixer.SetFloat("laserVolume", 0.0f);
            FindObjectOfType<AudioManager>().Play("LaserBossStart");
        });
        charge.InsertCallback(1.0f, () =>
        {
            m_effet.gameObject.SetActive(true);
        });
        charge.InsertCallback(3.0f, () =>
        {
            m_charge = true;
            //  la durée du son m_TempsTire
            FindObjectOfType<AudioManager>().Play("LaserBossMilieu");
        });
        charge.InsertCallback((maxTimeToLive - 2.0f), () =>
        {
            Disparait();
            //  la durée du son
            //FindObjectOfType<AudioManager>().Play("LaserBossFin");
        });


        charge.Play();
    }

    public void Disparait()
    {
        die = DOTween.Sequence();

        die.Append(transform.DOScaleY(0.2f, 2.0f));
        die.Insert(0.0f, m_sCorps.DOFade(0.0f, 2.0f).SetEase(Ease.InExpo));
        die.Insert(0.0f, m_sImpact.DOFade(0.0f, 2.0f).SetEase(Ease.InExpo));
        die.InsertCallback(0.0f, (() =>
        {
            //FindObjectOfType<AudioManager>().Mute("LaserBossMilieu");
            m_GroupeAudioLaser.audioMixer.SetFloat("laserVolume", -80.0f);

            FindObjectOfType<AudioManager>().Play("LaserBossFin");
        }));
        die.InsertCallback(1.0f, (() =>
        {
            ps.Stop();
            loadPS.Stop();
            m_charge = false;

            //m_effet.gameObject.SetActive(false);
        }));

        charge.Kill();

        die.Play();
    }
}
