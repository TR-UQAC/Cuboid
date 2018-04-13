using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bossTeleport : MonoBehaviour {
    #region variable

    //  le core, qui est désactiver de base mais s'active quand les 4 autre sont mort
    private Transform m_Core;

    private Transform m_FirePoint;

    private List<Transform> m_lstDir;

    private bool m_shootState = false;
    private bool m_TPState = true;


    [Tooltip("le noeud actuelle où ce trouve le boss")]
    public Node m_CurrentNode;

    [Tooltip("liste des différents noeud où le boss pourra ce téléporté aléatoirement")]
    public List<Node> m_LstNode;

    [Tooltip("nombre de fois que le boss ce téléporte durant ça phase de téléportation")]
    public int m_nbTP;
    private int m_currentTP;

    [Tooltip("Effect d'explosion à la mort du boss")]
    public GameObject m_ExplosionEffect;

    [Tooltip("Effect de téléportation")]
    public GameObject m_TeleportEffect;

    [Tooltip("Le préfab du laser tirée par le boss")]
    public GameObject m_Laser;

    public float m_AngleRotation = 360.0f;
    public float m_rotationTP = 720.0f;
    public int m_dmgLaser = 10;

    #endregion

    // Use this for initialization
    void Start ()
    {
        m_currentTP = m_nbTP;
        m_lstDir = new List<Transform>();

        if(m_LstNode.Count == 0)
        {
            Debug.LogError("Aucun noeud dans la liste de noeud du boss téléportation");
            gameObject.SetActive(false);
        }
        
        if(!m_CurrentNode)
        {
            Debug.LogError("Aucun noeud courant pour le boss téléporteur");
        }

        foreach (Transform child in transform)
        {
            if (child.tag == "Ennemi")
            {
                if (child.name == "Core")
                    m_Core = child;
            }

            if(child.tag == "Ancre")
            {
                if (child.name == "FirePoint")
                    m_FirePoint = child;
                else
                    m_lstDir.Add(child);
            }
        }

        //TirePartout();
        //Direction();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Core)
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
                las.transform.SetParent(null, true);

                las.GetComponent<laser>().Disparait();
            }
            //FindObjectOfType<AudioManager>().Mute("LaserBossMilieu");

            GameMaster.KillBossTP(this);
        }

        if (m_shootState == true)
        {
            TirePartout();
        }
        else if(m_TPState == true)
        {
            Direction();
        }
    }

    //  trouve un noeud aléatoire parmis la liste de noeud disponible et l'envoie dans TeleportTo(n)
    private void Direction()
    {
        Node dir = m_CurrentNode;

        do
        {
            int pos = Random.Range(0, m_LstNode.Count - 1);
            dir = m_LstNode[pos];
        }
        while (dir == m_CurrentNode);

        TeleportTo(dir);
    }

    //  téléporte le boss au noeud passer en paramètre
    private void TeleportTo(Node n)
    {
        Vector3 angle = transform.rotation.eulerAngles;
        angle.z += m_rotationTP;

        float scale = transform.lossyScale.x;

        if (m_TeleportEffect)
        {
            GameObject tpeffect = Instantiate(m_TeleportEffect, transform.position, transform.rotation);
            GameObject destEffect = Instantiate(m_TeleportEffect, n.transform.position, n.transform.rotation);
        }

        Sequence tp = DOTween.Sequence();
        tp.SetDelay(0.5f);
        tp.Append(transform.DORotate(angle, 3.0f, RotateMode.FastBeyond360)).SetEase(Ease.InBack);
        tp.Insert(0.0f, transform.DOScale(0.1f, 3.0f)).SetEase(Ease.InBack);
        tp.AppendCallback(() =>
        {
            Debug.Log("téléportation");
            transform.position = n.transform.position;
        });

        if (m_TeleportEffect)
        {
            //tp.Insert(3.0f, tpeffect); fadeOut les 2 effect de TP à 0.5f
            //tp.Insert(3.0f, destEffect
        }

        tp.Append(transform.DOScale(scale, 3.0f)).SetEase(Ease.OutBack);
        tp.AppendCallback(() =>
        {
            m_currentTP--;
            if (m_currentTP == 0)
            {
                m_shootState = true;
                m_currentTP = m_nbTP;
            }
            else
                Direction();
        });

        tp.Play();
        m_CurrentNode = n;

        

        m_TPState = false;

        //Direction();
    }

    //  fait en sorte que le boss tire dans les 4 direction et tourne en même temps et rotation 
    private void TirePartout()
    {
        m_shootState = false;

        Vector3 angle = transform.rotation.eulerAngles;
        angle.z += m_AngleRotation;

        Sequence pew = DOTween.Sequence();
        pew.SetDelay(0.5f);
        pew.Append(transform.DORotate(angle, 6f, RotateMode.FastBeyond360)).SetEase(Ease.InQuad);
        pew.InsertCallback(0.0f, () =>
        {
            for (int i = 0; i < m_lstDir.Count; i++)
            {
                Vector2 dir = m_lstDir[i].position - m_FirePoint.position;

                Tirer(dir, m_dmgLaser);
            }
        });
        pew.InsertCallback(7.5f, () =>
        {
            //  après le spinning, réactive la TP
            m_TPState = true;
        });

        pew.Play();
        m_shootState = false;
    }

    public void Tirer(Vector2 dir, int dmg)
    {
        if (m_Laser == null)
        {
            Debug.LogWarning("Il n'y a aucun prefab de balle dans " + name);
            return;
        }

        WeaponEnnemi canon = m_Core.GetComponent<WeaponEnnemi>();

        Bullet bul = Instantiate(m_Laser, m_FirePoint.position, m_FirePoint.rotation, transform).GetComponent<Bullet>() as Bullet;
        bul.direction = dir;
        bul.noHit = canon.noHit;
        bul.dommageHit = canon.dommageHit;
        bul.dmg = dmg;
    }
}
