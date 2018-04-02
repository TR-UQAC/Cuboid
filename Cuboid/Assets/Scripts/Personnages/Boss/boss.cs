using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class boss : MonoBehaviour
{
    //  list des parties du boss, ces 4 côté, le core, ces des ennemis normal mais qui seront modifier
    private List<Transform> m_lstEnnemis;
    //  le core, qui est désactiver de base mais s'active quand les 4 autre sont mort
    private Transform m_Core;
    //  Le shield, repousse les projectiles et est desactiver à la 2e phase
    private Transform m_shield;

    //  si le joueur est au dela de cette distance, Désactivation
    static float distActivation = 100;

    private Rigidbody2D rb;
    //private SpriteRenderer sr;
    private Transform tr;

    [Tooltip("le noeud actuelle où ce trouve le boss")]
    public Node m_CurrentNode;

    private Node m_StartNode;

    [Tooltip("Temps en seconde entre 2 déplacement")]
    public float m_MovingRate;

    [Tooltip("Temps en seconde entre 2 déplacement à la 2e phase")]
    public float m_MovingRate2;

    private float m_currentWait;

    //  référence au joueur à poursuivre
    private GameObject m_Player;
    private float m_ScaleY = 1.0f;
    //  la box collider du boss pour l'écrasement
    private BoxCollider2D bc;

    //  si le joueur meur, le chercher
    private bool searchingForPlayer = false;
    private bool m_arreter = true;

    //  2e phase du boss, les 4 côté détruit, le core attack
    private bool m_Phase2 = false;

    public GameObject m_ExplosionEffect;

    private Sequence m_bouger;

    //  set les variable avant d'être actif
    private void Awake()
    {
        //m_Player = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        //sr = GetComponent<SpriteRenderer>() as SpriteRenderer;
        tr = GetComponent<Transform>() as Transform;
    }

    // Use this for initialization
    void Start ()
    {
        m_StartNode = m_CurrentNode;

        bc = GetComponent<BoxCollider2D>() as BoxCollider2D;

        m_lstEnnemis = new List<Transform>();

        foreach (Transform child in transform)
        {
            if (child.tag == "Ennemi")
            {
                if (child.name == "CoreEcrabouilleur")
                    m_Core = child;

                m_lstEnnemis.Add(child);
            }

            if(child.tag == "Shield")
            {
                m_shield = child;
            }
        }

        // !*!  lancé l'intro du boss


        Invoke("directionBoss", m_MovingRate);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        for (int i = 0; i < m_lstEnnemis.Count; i++)
        {
            if (m_lstEnnemis[i] == null)
                m_lstEnnemis.RemoveAt(i);
        }

        //  Paramètrage pour la 2e phase
        if (m_lstEnnemis.Count == 1 && m_Phase2 == false)
        {
            m_Phase2 = true;

            Ennemis core = m_Core.GetComponent<Ennemis>() as Ennemis;
            core.ennemiStats.immortel = false;
            core.enabled = true;
            

            bc.enabled = false;
            m_MovingRate = m_MovingRate2;

            Sequence sdown = DOTween.Sequence();

            sdown.Append(m_shield.DOScale(0.1f, 2.0f).SetEase(Ease.InBounce));
            sdown.InsertCallback(0.5f, () =>
            {
                Transform eff = m_shield.GetChild(1);
                eff.gameObject.SetActive(false);

                FindObjectOfType<AudioManager>().Play("ShieldBossDown");
            });
            sdown.AppendCallback(() =>
            {
                m_shield.gameObject.SetActive(false);
                
            });

        }

        if(m_lstEnnemis.Count == 0)
        {
            //  le boss est mort

            GameObject ex = Instantiate(m_ExplosionEffect, transform.position, transform.rotation);
            FindObjectOfType<AudioManager>().Play("ExplosionBoss");

            Sequence bossKill = DOTween.Sequence();
            bossKill.SetDelay(2.0f);
            bossKill.AppendCallback(() =>
            {
                Destroy(ex);
            });

            List<GameObject> lstLaser = new List<GameObject>(GameObject.FindGameObjectsWithTag("laserBoss"));
            foreach (GameObject las in lstLaser)
            {
                las.GetComponent<laser>().Disparait();
            }
            //FindObjectOfType<AudioManager>().Mute("LaserBossMilieu");

            GameMaster.KillBoss(this);
        }

        /*if (m_Player == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
        }*/

        //  ajustement du boxCollider2D pour qu'il écrase que si le joueur est en dessous
        //  Quelque soit l'angle
        if (m_Phase2 == false)
        {
            int angle = (int)tr.rotation.eulerAngles.z;

            if (angle > 320 || angle < 40)
            {
                bc.offset = new Vector2(0.0f, -1.1f);
                bc.size = new Vector2(2.0f, 0.6f);
            }
            else if (angle > 50 && angle < 130)
            {
                bc.offset = new Vector2(-1.1f, 0.0f);
                bc.size = new Vector2(0.6f, 2.0f);
            }
            else if (angle > 140 && angle < 220)
            {
                bc.offset = new Vector2(0.0f, 1.1f);
                bc.size = new Vector2(2.0f, 0.6f);
            }
            else if (angle > 230 && angle < 310)
            {
                bc.offset = new Vector2(1.1f, 0.0f);
                bc.size = new Vector2(0.6f, 2.0f);
            }
        }
    }
    
    //  détermine si le boss va à gauche ou a droite
    private void directionBoss()
    {
        if (m_Player)
        {
            if (m_Player.transform.position.x < tr.position.x)
                jumpRot(true);
            else
                jumpRot(false);
        }

        Invoke("directionBoss", m_MovingRate);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject coll = other.gameObject;

        if(m_Player == coll)
        {
            //  désactive les controle
            PlayerCharacter2D p = coll.GetComponent<PlayerCharacter2D>();

            Rigidbody2D rbp = m_Player.GetComponent<Rigidbody2D>() as Rigidbody2D;
            rbp.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            //rbp.constraints = RigidbodyConstraints2D.FreezeAll;

            if (p)
                p.setEnableInput(false);

            //  rapetisse le joueur
            Sequence ecrase = DOTween.Sequence();

            ecrase.Append(m_Player.transform.DOScaleY(1.0f, 0.2f));
            ecrase.Insert(0.0f, m_Player.transform.DOMoveY(m_Player.transform.position.y - 0.4f, 0.2f));
            ecrase.Play();
            //Invoke("p.setEnableInput(false)", m_MovingRate + 0.1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject coll = other.gameObject;

        if (m_Player == coll)
        {
            //  désactive les controle
            PlayerCharacter2D p = coll.GetComponent<PlayerCharacter2D>();

            Rigidbody2D rbp = m_Player.GetComponent<Rigidbody2D>() as Rigidbody2D;
            rbp.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (p)
                p.setEnableInput(true);

            //  agrandir le joueur
            Sequence decrase = DOTween.Sequence();
            decrase.Append(m_Player.transform.DOScaleY(m_ScaleY, 0.2f));
            //decrase.Insert(0.0f, m_Player.transform.DOMoveY(m_Player.transform.position.y + 0.4f, 0.2f));

            decrase.Play();
        }
    }

    //  fonction de déplacement vers le noeud précicé, true = gauche / false = droite
    public void jumpRot(bool gd)
    {
        //  test de déplacement avec DOTween
        Vector3 pos = tr.position;
        Vector3 angle = tr.rotation.eulerAngles;

        if (m_arreter)
            return;

        //Vector3 pos = new Vector3(7.0f, 0.0f, 0.0f) + tr.position;
        if (m_CurrentNode != null)  // le boss est sur un noeud de la liste
        {
            if (gd == true) //  il veux aller à gauche
            {
                if (m_CurrentNode.m_VoisinGauche != null)
                {
                    pos += (m_CurrentNode.m_VoisinGauche.transform.position - pos);
                    angle += new Vector3(0.0f, 0.0f, 90.0f);
                    m_CurrentNode = m_CurrentNode.m_VoisinGauche;
                }
                else if (m_CurrentNode.m_VoisinDroit != null)  // aucun voisin gauche, va à droite
                {
                    pos += (m_CurrentNode.m_VoisinDroit.transform.position - pos);
                    angle += new Vector3(0.0f, 0.0f, -90.0f);
                    m_CurrentNode = m_CurrentNode.m_VoisinDroit;
                }
                else
                    return;
            }
            else    // il veux aller à droite
            {
                if (m_CurrentNode.m_VoisinDroit != null)
                {
                    pos += (m_CurrentNode.m_VoisinDroit.transform.position - pos);
                    angle += new Vector3(0.0f, 0.0f, -90.0f);
                    m_CurrentNode = m_CurrentNode.m_VoisinDroit;
                }
                else if (m_CurrentNode.m_VoisinGauche != null)// aucun voisin droite, va à gauche
                {
                    pos += (m_CurrentNode.m_VoisinGauche.transform.position - pos);
                    angle += new Vector3(0.0f, 0.0f, 90.0f);
                    m_CurrentNode = m_CurrentNode.m_VoisinGauche;
                }
                else
                    return;
            }
        }
        else
        {
            Debug.LogError("Aucun noeud courrant pour le boss écrabouilleur");
            return;
        }

        if (m_Phase2)
            pos.y -= 6.0f;

        Sequence bouge = DOTween.Sequence();

        bouge.Append(tr.DOJump(pos, 8.0f, 1, 0.8f).SetEase(Ease.InOutQuad));
        bouge.Insert(0.1f, tr.DORotate(angle, 0.75f).SetEase(Ease.InOutQuart));

        if (m_Phase2 == false)
        {
            bouge.AppendCallback(() =>
            {
                CameraShaker.Instance.ShakeOnce(3f, 2f, .1f, 1.0f);
                if (FindObjectOfType<AudioManager>() != null)
                {
                    FindObjectOfType<AudioManager>().Play("StompBoss");
                }
            });
        }
        else if (m_Player != null)
        {
            bouge.AppendCallback(() =>
            {

                WeaponEnnemi canon = m_Core.GetComponent<WeaponEnnemi>();

                canon.Tirer(new Vector2(1.0f, 0.0f), 10, 0.0f);
                canon.Tirer(new Vector2(0.0f, 1.0f), 10, 0.0f);
                canon.Tirer(new Vector2(-1.0f, 0.0f), 10, 0.0f);
            });
        }

        bouge.Play();
    }

    public void CheatLifeBoss()
    {
        for (int i = 0; i < m_lstEnnemis.Count; i++)
        {
            if (m_lstEnnemis[i] == null)
                continue;

            Ennemis pasFin = m_lstEnnemis[i].GetComponent<Ennemis>() as Ennemis;
            pasFin.ennemiStats.vie = 1;
        }
    }

    #region Activation
    //**** Activation/ Desactivation par rapport à la distance ****//

    //  reset les pv de chacune de ces partie à la mort du joueur
    public void resetPV()
    {
        m_Player = null;

        foreach (Transform item in m_lstEnnemis)
        {
            Ennemis pasFin = item.GetComponent<Ennemis>() as Ennemis;
            pasFin.ennemiStats.vie = pasFin.ennemiStats.vieMax;
            item.GetComponent<SpriteMask>().alphaCutoff = 1.0f;
        }

        List<GameObject> lstLaser = new List<GameObject>(GameObject.FindGameObjectsWithTag("laserBoss"));
        foreach (GameObject las in lstLaser)
        {
            las.GetComponent<laser>().Disparait();
        }
        //FindObjectOfType<AudioManager>().Mute("LaserBossMilieu");

        searchingForPlayer = true;
        Sequence retour = DOTween.Sequence();

        retour.AppendCallback(() =>
        {
            m_CurrentNode = m_StartNode.m_VoisinGauche;
            jumpRot(false);
        }).SetDelay(0.9f);
        retour.AppendCallback(() =>
        {
            enabled = false;
        });
        retour.Play();
    }

    public void ActiveBoss(GameObject p)
    {
        m_Player = p;
        m_ScaleY = p.transform.lossyScale.y;
        searchingForPlayer = false;
        m_arreter = false;
    }


    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            m_Player = sResult;
            m_ScaleY = sResult.transform.lossyScale.y;
            searchingForPlayer = false;
            m_arreter = false;
            yield break;
        }
    }
    #endregion
}
