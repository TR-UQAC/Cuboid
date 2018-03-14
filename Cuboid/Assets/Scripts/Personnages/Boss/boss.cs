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

    //  si le joueur meur, le chercher
    private bool searchingForPlayer = false;

    //  set les variable avant d'être actif
    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        sr = GetComponent<SpriteRenderer>() as SpriteRenderer;
        tr = GetComponent<Transform>() as Transform;
    }

    // Use this for initialization
    void Start ()
    {
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
        //if(other)
    }

    private void OnTriggerExit2D(Collider2D other)
    {

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
            searchingForPlayer = false;
            StartCoroutine(CheckDistance());
            yield break;
        }
    }
    #endregion
}
