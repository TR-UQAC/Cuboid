using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class boss : MonoBehaviour
{

    private List<Transform> m_lstEnnemis;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform tr;

    //  Temps en seconde entre 2 déplacement
    public float m_MovingRate;

    //  Temps restant avant de pouvoir ce déplacer de nouveau
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

        jumpRot();


    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("m"))
            jumpRot();
    }

    public void jumpRot()
    {
        //  test de déplacement avec DOTween

        Vector3 pos = new Vector3(7.0f, 0.0f, 0.0f) + tr.position;
        Vector3 angle = new Vector3(0.0f, 0.0f, -90.0f) + tr.rotation.eulerAngles;

        Sequence bouge = DOTween.Sequence();

        bouge.SetDelay(2.0f);

        bouge.Append(tr.DOJump(pos, 4.0f, 1, 1.0f).SetEase(Ease.InSine));
        bouge.Insert(2.1f, tr.DORotate(angle, 0.95f));

        bouge.Play();
    }
}
