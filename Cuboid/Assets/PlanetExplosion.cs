using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;

public class PlanetExplosion : MonoBehaviour {

    private GameObject ship;
    private GameObject Fadego;

    public float elapsedTime = 0f;
    public GameObject m_ExplosionEffect;

    void Awake()
    {
        Fadego = GameObject.Find("Canvas").transform.Find("FadeToWhite").gameObject;
        ship = GameObject.Find("PlayerShip");
        ship.GetComponent<Animator>().Play("ShipTakeOff");
        InvokeRepeating("ShipAnim", 4f, Time.deltaTime);
        InvokeRepeating("PlanetExplo", 0.5f, 0.75f); 
    }
	
	void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
    }

    void PlanetExplo()
    {
        GameObject ex = Instantiate(m_ExplosionEffect, new Vector3(0, 0, 1), new Quaternion());
        ex.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        Destroy(ex, 10f);
    }

    void Fade()
    {
        Color c = Fadego.GetComponent<Image>().color;
        if (elapsedTime < 14f)
        {
            Fadego.GetComponent<Image>().color = new Color(c.r, c.g, c.b, c.a + 0.01f);
        }
        else
        {
            Fadego.GetComponent<Image>().color = new Color(c.r - 0.01f, c.g - 0.01f, c.b - 0.01f, 1);
        }

        if (elapsedTime > 20f)
        {
            CancelInvoke();
            GameObject.Find("Canvas").transform.Find("Credit").GetComponent<Animator>().enabled = true;
        }
    }

    void ShipAnim()
    {
        ship.transform.localScale += new Vector3(0.01f, 0.01f);

        if (elapsedTime > 5.25f)
        {
            ship.transform.Rotate(new Vector3(0, 0, 1), 3f);
            
            if (elapsedTime > 7.0f)
            {
                CancelInvoke();
                InvokeRepeating("ShipExit", 1f, Time.deltaTime);
            }
        }
        else
        {
            ship.transform.Rotate(new Vector3(0, 0, 1), 1f);
        }
    }

    void ShipExit()
    {
        ship.transform.localScale += new Vector3(0.3f, 0.3f);
        if (elapsedTime > 9.5f)
        {
            ship.SetActive(false);
            CancelInvoke();
            InvokeRepeating("Fade", 0f, 0.04f);
        }
    }
}
