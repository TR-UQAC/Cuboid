﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlanetExplosion : MonoBehaviour {

    private GameObject ship;
    private GameObject Fadego;

    public float elapsedTime = 0f;
    public GameObject m_ExplosionEffect;
    public Animator animatorCredit;
    private bool goMenu = false;

    void Awake()
    {
        Fadego = GameObject.Find("Canvas").transform.Find("FadeToWhite").gameObject;
        ship = GameObject.Find("PlayerShip");
        ship.GetComponent<Animator>().Play("ShipTakeOff");
        InvokeRepeating("ShipAnim", 4f, Time.deltaTime);
        InvokeRepeating("PlanetExplo", 0.5f, 0.75f);
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().ChangeMusique("EscapeMusic", "MusiqueFin");
        }
    }
	
	void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
    }

    void Update() {
        if (Input.anyKey && goMenu)
        {
            if (FindObjectOfType<AudioManager>() != null)
            {
                FindObjectOfType<AudioManager>().ChangeMusique("MusiqueFin", "Musique_Jeu");
            }
            SceneManager.LoadScene(0);
        }
    }
    void PlanetExplo()
    {
        if (FindObjectOfType<AudioManager>() != null)
        {
            FindObjectOfType<AudioManager>().Play("ExplosionP");
        }
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
            animatorCredit.enabled = true;
        }
        if (elapsedTime > 80f) {
            goMenu = true;
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
            if (FindObjectOfType<AudioManager>() != null)
            {
                FindObjectOfType<AudioManager>().Play("ExplosionPlanet");
            }
            ship.SetActive(false);
            CancelInvoke();
            InvokeRepeating("Fade", 0f, 0.04f);
        }
    }
}
