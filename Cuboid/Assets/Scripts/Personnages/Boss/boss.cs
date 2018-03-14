using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class boss : MonoBehaviour
{
    //  list des parties du boss, ces 4 côté, le core, ces des ennemis normal mais qui seront modifier
    private List<Transform> m_lstEnnemis;

    //  si le joueur est au dela de cette distance, Désactivation
    static float distActivation = 100;

    //  Liste des noeuds où le boss pourra ce déplacer, il ne pourra pas aller ailleur
    private List<Transform> m_lstNode;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform tr;

    [Tooltip("le noeud actuelle où ce trouve le boss")]
    public Node m_CurrentNode;

    [Tooltip("Temps en seconde entre 2 déplacement")]
    public float m_MovingRate;

    //Temps restant avant de pouvoir ce déplacer de nouveau
    //private float m_CurrentWait;

    //  référence au joueur à poursuivre
    private GameObject m_Player;
    private float m_ScaleY = 1.0f;
    //  la box collider du boss pour l'écrasement
    private BoxCollider2D bc;

    //  si le joueur meur, le chercher
    private bool searchingForPlayer = false;

    //  set les variable avant d'être actif
    private void Awake()
    {
        //m_Player = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        sr = GetComponent<SpriteRenderer>() as SpriteRenderer;
        tr = GetComponent<Transform>() as Transform;
    }

    // Use this for initialization
    void Start ()
    {
        bc = GetComponent<BoxCollider2D>() as BoxCollider2D;

        m_lstEnnemis = new List<Transform>();

        foreach (Transform child in transform)
        {
            if (child.tag == "Ennemi")
                m_lstEnnemis.Add(child);
        }

        if (m_Player == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
        }

        StartCoroutine(CheckDistance());

        //m_CurrentWait = m_MovingRate;

        //  lancé l'intro du boss

        //jumpRot();

        InvokeRepeating("directionBoss", m_MovingRate, m_MovingRate);


    }
	
	// Update is called once per frame
	void Update ()
    {
        //  ajustement du boxCollider2D pour qu'il écrase que si le joueur est en dessous
        //  Quelque soit l'angle

        int i = (int)tr.rotation.eulerAngles.z;

        if(i > 320 || i < 40)
        {
            bc.offset = new Vector2(0.0f, -0.2f);
            bc.size = new Vector2(2.0f, 2.5f);
        }
        else if(i > 50 && i < 130)
        {
            bc.offset = new Vector2(-0.2f, 0.0f);
            bc.size = new Vector2(2.5f, 2.0f);
        }
        else if(i > 140 && i < 220)
        {
            bc.offset = new Vector2(0.0f, 0.2f);
            bc.size = new Vector2(2.0f, 2.5f);
        }
        else if(i > 230 && i < 310)
        {
            bc.offset = new Vector2(0.2f, 0.0f);
            bc.size = new Vector2(2.5f, 2.0f);
        }



        if (Input.GetKeyDown("m"))
            jumpRot(false);
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
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject coll = other.gameObject;

        if(m_Player == coll)
        {
            //  désactive les controle
            PlayerCharacter2D p = coll.GetComponent<PlayerCharacter2D>();

            if (p)
                p.setEnableInput(false);

            //  rapetisse le joueur
            Sequence ecrase = DOTween.Sequence();

            ecrase.Append(m_Player.transform.DOScaleY(1.0f, 0.2f));
            //ecrase.Insert(m_MovingRate - 0.2f, m_Player.transform.DOScaleY(m_ScaleY, 0.2f));
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

            if (p)
                p.setEnableInput(true);

            //  agrandir le joueur
            Sequence decrase = DOTween.Sequence();

            decrase.Append(m_Player.transform.DOScaleY(m_ScaleY, 0.2f));
            
            decrase.Play();
        }
    }

    //  fonction de déplacement vers le noeud précicé, true = gauche / false = droite
    public void jumpRot(bool gd)
    {
        //  test de déplacement avec DOTween
        Vector3 pos = tr.position;
        Vector3 angle = tr.rotation.eulerAngles;

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

        bool descend = (tr.position.y - 1.0f > pos.y);

        Sequence bouge = DOTween.Sequence();

        bouge.Append(tr.DOJump(pos, 12.0f, 1, 0.8f).SetEase(Ease.InOutQuad));
        bouge.Insert(0.1f, tr.DORotate(angle, 0.75f).SetEase(Ease.InOutQuart));

        bouge.Play();
    }

    #region Activation
    //**** Activation/ Desactivation par rapport à la distance ****//
    IEnumerator CheckDistance()
    {
        if (m_Player == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield break;
        }

        if ((m_Player.transform.position - tr.position).sqrMagnitude < distActivation * distActivation)
            enabled = true;
        else
        {
            rb.velocity = new Vector2(0, 0);
            enabled = false;
        }

        yield return new WaitForSeconds(5f);
        StartCoroutine(CheckDistance());
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
            StartCoroutine(CheckDistance());
            yield break;
        }
    }
    #endregion
}
