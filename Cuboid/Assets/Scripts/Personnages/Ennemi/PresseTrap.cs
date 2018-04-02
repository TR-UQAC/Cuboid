using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PresseTrap : MonoBehaviour {

    #region Variable
    //  scale du joueur avant d'être écrasé
    private float m_ScaleY = 1.0f;

    [Tooltip("Le délais en seconde avant de commencer, pour synchroniser les départ")]
    public float m_DelayStart = 0.0f;

    [Tooltip("Le temps en seconde avant de rebouger")]
    public float m_Wait = 5.0f;

    [Tooltip("Le temps en seconde de mouvement, plus il est grand, plus il descend longtemps")]
    public float m_TempsDescend = 2.0f;

    [Tooltip("multiplie le déplacement effectuer à chaque instant")]
    public float m_MultipleVitesse = 1.0f;

    [Tooltip("Les dégats reçus quand le joueur ce fait écrasé")]
    public int m_dmg = 10;

    [Tooltip("A qui la presse fait des DMG")]
    public LayerMask dommageHit;

    // true = est en train de descendre / false = en train de monté
    private bool m_descend = true;
    private bool m_start = false;
    private bool m_getScale = false;

    private SpriteRenderer sr;

    private float m_currentDelay;

    #endregion

    // Use this for initialization
    void Start () {

        sr = GetComponent<SpriteRenderer>() as SpriteRenderer;
        if (!sr)
            enabled = false;

        m_currentDelay = m_DelayStart;

        
        //Invoke("ToggleDescente", m_DelayStart);
	}
	
	// Update is called once per frame
	void Update () {
        m_currentDelay -= Time.deltaTime;

        if(m_currentDelay <= 0.0f)
        {
            ToggleDescente();
        }

        if (m_start == false)
            return;

        if (m_currentDelay > m_Wait)
        {
            float y = sr.size.y;
            if (m_descend == true)
            {
                y -= Time.deltaTime * m_MultipleVitesse;
                if (y < 4.0f)
                    y = 4.0f;
            }
            else
                y += Time.deltaTime * m_MultipleVitesse;

            sr.size = new Vector2(sr.size.x, y);
        }
    }

    //  baisse ou monte la presse
    private void ToggleDescente()
    {
        m_currentDelay = m_Wait + m_TempsDescend;

        //FindObjectOfType<AudioManager>().Play("PressTrapDown");

        if (m_descend == false)
            m_descend = true;
        else
            m_descend = false;

        m_start = true;
    }

    #region TriggerEcrase
    //  si le joueur est détecter, l'écrase et lui fait des DMG
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameObject coll = collision.gameObject;

            if(m_getScale == false)
            {
                m_ScaleY = coll.transform.lossyScale.y;
                m_getScale = true;
            }

            if (dommageHit == (dommageHit | (1 << coll.layer)))
            {
                Personnages en = coll.GetComponent<Personnages>() as Personnages;
                en.DommagePerso(m_dmg);
            }

            if (m_descend == false && (m_currentDelay - m_Wait) < m_TempsDescend / 2)
            {
                //  désactive les controle
                PlayerCharacter2D p = coll.GetComponent<PlayerCharacter2D>();

                Rigidbody2D rbp = coll.GetComponent<Rigidbody2D>() as Rigidbody2D;
                rbp.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

                if (p)
                    p.setEnableInput(false);

                //  rapetisse le joueur
                Sequence ecrase = DOTween.Sequence();

                ecrase.Append(coll.transform.DOScaleY(1.0f, 0.2f));
                ecrase.Insert(0.0f, coll.transform.DOMoveY(coll.transform.position.y - 0.4f, 0.2f));
                ecrase.Play();
            }
        }
    }

    //  n'écrase plus le joueur
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject coll = collision.gameObject;

            //  désactive les controle
            PlayerCharacter2D p = coll.GetComponent<PlayerCharacter2D>();

            Rigidbody2D rbp = coll.GetComponent<Rigidbody2D>() as Rigidbody2D;
            rbp.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (p)
                p.setEnableInput(true);

            //  agrandir le joueur
            Sequence decrase = DOTween.Sequence();
            decrase.Append(coll.transform.DOScaleY(m_ScaleY, 0.2f));
            //decrase.Insert(0.0f, m_Player.transform.DOMoveY(m_Player.transform.position.y + 0.4f, 0.2f));

            decrase.Play();
        }
    }
    #endregion
}
