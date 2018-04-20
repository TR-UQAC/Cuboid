using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerBossStage : MonoBehaviour {

    public GameObject m_boss;
    public GameObject m_Porte;
    public GameObject m_Trape;
    public GameObject m_upgrade;
    public GameObject m_bossCam;

    private GameObject[] canons;

    [Tooltip("1 = boss finale / 2 = boss téléporteur / 3 = mini boss")]
    public int m_NoBoss = 1;

    private bool m_bossActive = false;
    private bool m_bossFin = false;

    private Vector3 m_lastPosBoss;

    // Use this for initialization
    void Start () {
        if (!m_boss)
            enabled = false;

        if (m_NoBoss != 3)
            m_boss.SetActive(false);

        if(m_NoBoss == 1)
            m_boss.GetComponent<boss>().enabled = false;

        if (m_NoBoss == 2)
            m_boss.GetComponent<bossTeleport>().enabled = false;

        if (m_NoBoss == 3) {
            m_boss.GetComponent<Ennemis>().activer = false;
            m_boss.GetComponent<Ennemis>().enabled = false;

            m_boss.transform.position = new Vector3(0, -165, 0);
            // m_boss.GetComponent<EnnemiAI>().enabled = false;
            canons = GameObject.FindGameObjectsWithTag("Canon");
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(m_boss == null && m_bossFin == false)
        {
            //le boss est mort

            m_bossFin = true;
            m_bossActive = false;

            if(m_bossCam)
            {
                m_bossCam.GetComponent<TriggerBoxMultiCam>().DeactivateCam();
                m_bossCam.SetActive(false);
            }

            //  ouvrir la porte
            if (m_Porte)
            {
                m_Porte.GetComponent<BoxCollider2D>().enabled = false;
                m_Porte.GetComponent<Animator>().SetBool("DoClose", false);
            }

            //  ouvrir la trape
            if (m_Trape)
                m_Trape.GetComponent<BoxCollider2D>().enabled = false;


            //  faire pop l'upgrade
            if (m_upgrade)
            {
                Sequence loot = DOTween.Sequence();

                loot.Append(m_upgrade.transform.DOMove(m_lastPosBoss, 0.01f));
                loot.Append(m_upgrade.transform.DOShakePosition(5.0f, new Vector3(0.0f, -0.05f, 0.0f), 2, 40.0f, false, false).SetLoops(1000));
                loot.Play();
            }

            //Si boss 3 detruire Canon
            if (m_NoBoss == 3) {
                //GameObject[] canons;

                //canons = GameObject.FindGameObjectsWithTag("Canon");
                for (int i = canons.Length-1; i >=0; i--) {
                    Canon canon = canons[i].GetComponent<Canon>();
                    canon.Detruire();
                    /*
                    canon.ennemiStats.immortel = false;
                    canon.DommagePerso(10);
                    */
                }
            }

            if(m_NoBoss == 1)
            {
                GameMaster.StartEscapeSequence();
            }

        }

        if(m_bossActive == true)
        {
            m_lastPosBoss = m_boss.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && m_bossActive == false && m_bossFin == false)
        {
            //  Le joueur entre dans la pièce
            m_boss.SetActive(true);
            if (m_NoBoss == 1)
            {
                m_boss.GetComponent<boss>().enabled = true;
                m_boss.GetComponent<boss>().ActiveBoss(collision.gameObject);
            }

            else if(m_NoBoss == 2)
            {
                m_boss.GetComponent<bossTeleport>().enabled = true;
            }
            else if (m_NoBoss == 3) {
                m_boss.GetComponent<Ennemis>().enabled = true;
                m_boss.GetComponent<Ennemis>().activer = true;
                m_boss.transform.position = new Vector3(-23, -165, 0);
                //m_boss.GetComponent<EnnemiAI>().enabled = true;
                for (int i = canons.Length - 1; i >= 0; i--) {
                    Canon canon = canons[i].GetComponent<Canon>();
                    canon.Entrer();
                }
            }
            m_bossActive = true;
            if(m_Porte)
            {
                m_Porte.GetComponent<BoxCollider2D>().enabled = true;
                m_Porte.GetComponent<Animator>().SetBool("DoClose", true);
            }
            //m_Porte.PlayAnimation()
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if(collision.tag == "Player" && m_bossFin == false)
        {
            if (collision.gameObject.activeSelf == true)
                return;

            Debug.Log("le joueur est mouru");

            if(m_NoBoss == 1)
            {
                m_boss.GetComponent<boss>().resetPV();
                m_boss.GetComponent<boss>().enabled = false;
            }
            else if(m_NoBoss == 2)
            {
                m_boss.GetComponent<bossTeleport>().resetBoss();
                m_boss.GetComponent<bossTeleport>().enabled = false;
            }
            else if(m_NoBoss == 3) {
                m_boss.GetComponent<Ennemis>().SoinPerso(1000);
                m_boss.GetComponent<Ennemis>().activer = false;
                m_boss.GetComponent<Ennemis>().enabled = false;

                m_boss.transform.position = new Vector3(0, -165, 0);
                //m_boss.GetComponent<EnnemiAI>().enabled = false;

                for (int i = canons.Length - 1; i >= 0; i--) {
                    Canon canon = canons[i].GetComponent<Canon>();
                    canon.Sortir();
                }

                List<GameObject> lstLaser = new List<GameObject>(GameObject.FindGameObjectsWithTag("laserBoss"));
                foreach (GameObject las in lstLaser) {
                    las.GetComponent<laser>().Disparait();
                }
            }
            /*
            if (m_NoBoss == 3) {
                for (int i = canons.Length - 1; i >= 0; i--) {
                    Canon canon = canons[i].GetComponent<Canon>();
                    canon.Sortir();
                }

                List<GameObject> lstLaser = new List<GameObject>(GameObject.FindGameObjectsWithTag("laserBoss"));
                foreach (GameObject las in lstLaser)
                {
                    las.GetComponent<laser>().Disparait();
                }
            }
            */

            m_bossActive = false;
            if (m_Porte)
            {
                m_Porte.GetComponent<BoxCollider2D>().enabled = false;
                m_Porte.GetComponent<Animator>().SetBool("DoClose", false);
            }
        }
    }
}
