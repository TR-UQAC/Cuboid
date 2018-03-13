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
    private float m_CurrentWait;

    //  set les variable avant d'être actif
    private void Awake()
    {
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

        m_CurrentWait = m_MovingRate;

        //  lancé l'intro du boss

        //jumpRot();


    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("m"))
            jumpRot(false);
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

        bouge.Append(tr.DOJump(pos, 12.0f, 1, 1.0f));
        bouge.Insert(0.1f, tr.DORotate(angle, 0.98f).SetEase(Ease.OutSine));

        bouge.Play();
    }
}
