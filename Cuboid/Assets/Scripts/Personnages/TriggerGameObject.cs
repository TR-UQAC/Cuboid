using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerGameObject : MonoBehaviour {

    public List<GameObject> m_lstActor;
    private bool InZone = false;
    private Sequence m_waitDisable;

    // Use this for initialization
    void Start()
    {
        if (m_lstActor.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        m_waitDisable = DOTween.Sequence();

        foreach (GameObject go in m_lstActor)
        {
            go.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //if(InZone == false)
        //{

        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && InZone == false)
        {
            /*if (m_waitDisable.IsActive())
            {
                m_waitDisable.Kill();
                Debug.Log("kill la sequence");
            }*/

            foreach (GameObject go in m_lstActor)
            {
                go.SetActive(true);
            }

            InZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_waitDisable = DOTween.Sequence();

            m_waitDisable.InsertCallback(2.0f, () =>
            {
                if (InZone == false)
                {
                    foreach (GameObject go in m_lstActor)
                    {
                        go.SetActive(false);
                    }
                }
            });

            InZone = false;

            m_waitDisable.Play();
        }
    }
}
