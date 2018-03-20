using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class laser : Bullet {

    private LineRenderer lr;
    private ParticleSystem ps;
    private Transform target;
    //private CapsuleCollider2D cc;
    private bool m_lasActive = false;

    // autre
    RaycastHit2D hit;
    float range = 100.0f;
    LineRenderer line;
    public Material lineMaterial;

    private void Update()
    {
        if (m_lasActive == false)
        {
            SetUpLaser();
        }

        Debug.Log("dans l'update : " + transform.position.ToString() + target.position.ToString());
        //Ray2D ray = new Ray2D(transform.position, target.position);
        hit = Physics2D.Raycast(transform.position, target.position, range, dommageHit);
        if (hit)
        {
            Debug.Log("A toucher le : " + hit.point.ToString());
            line.enabled = true;
            line.SetPosition(1, target.localPosition);
        }
    }


    //  méthode qui sera appeler pour paramétré le laser
    private void SetUpLaser()
    {
        Debug.Log("dans le setup");
        foreach (Transform child in transform)
        {
            if (child.name == "TrailLaser")
            {
                line = child.GetComponent<LineRenderer>() as LineRenderer;
                continue;
            }

            if (child.name == "sparkLaser")
            {
                ps = child.GetComponent<ParticleSystem>() as ParticleSystem;
                continue;
            }

            if(child.name == "EndLaser")
            {
                target = child;
                continue;
            }
        }

        //cc = GetComponent<CapsuleCollider2D>();

        line.positionCount = 2;
        line.GetComponent<Renderer>().material = lineMaterial;
        line.startWidth = 0.1f;
        line.endWidth = 0.25f;

        m_lasActive = true;
        //AvanceLaser();
    }

    private void AvanceLaser()
    {
        //Sequence movelas = DOTween.Sequence();

        float miDist = target.localPosition.x;

        //if(cc)
        //{
            //cc.size.Set(miDist, 0.0f);
        //}

        //movelas.Append(lr)
        lr.SetPosition(lr.positionCount - 1, new Vector3(miDist, 0.0f));// target.localPosition);
        //lr = target.position;
    }
}
