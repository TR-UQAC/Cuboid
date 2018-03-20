using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class laser : Bullet {

    private LineRenderer lr;
    private ParticleSystem ps;
    private Transform target;
    private CapsuleCollider2D cc;
    private bool m_lasActive = false;

    private void Update()
    {
        if (m_lasActive == false)
        {
            SetUpLaser();
        }
    }


    //  méthode qui sera appeler pour paramétré le laser
    private void SetUpLaser()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "TrailLaser")
            {
                lr = child.GetComponent<LineRenderer>() as LineRenderer;
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

        cc = GetComponent<CapsuleCollider2D>();

        m_lasActive = true;
        AvanceLaser();
    }

    private void AvanceLaser()
    {
        //Sequence movelas = DOTween.Sequence();

        float miDist = target.localPosition.x;

        //if(cc)
        //{
            cc.size.Set(miDist, 0.0f);
        //}

        //movelas.Append(lr)
        lr.SetPosition(lr.positionCount - 1, new Vector3(miDist / 3.33f, 0.0f));// target.localPosition);
        //lr = target.position;
    }
}
