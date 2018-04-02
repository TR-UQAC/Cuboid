using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxMultiCam : MonoBehaviour {

    public Camera cam;
    private bool InZone = false;
    private Transform m_ancreGB, m_ancreDH;

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogWarning("Aucune camera relier a triggerBoxMultiCam");
            cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        }

        foreach (Transform tr in transform)
        {
            if (tr.name == "AncreDH")
                m_ancreDH = tr;
            else if (tr.name == "AncreGB")
                m_ancreGB = tr;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" || InZone == true)
            return;

        foreach (Transform item in transform)
        {
            if(item != null)
                cam.GetComponentInParent<MultipleTargetCamera>().targets.Add(item);
        }
        InZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        Transform tr = collision.transform;

        if(collision.gameObject.activeSelf == true)
        {
            if (m_ancreDH && m_ancreGB)
            {
                if (tr.position.x >= m_ancreGB.position.x && tr.position.y >= m_ancreGB.position.y &&
                    tr.position.x <= m_ancreDH.position.x && tr.position.y <= m_ancreDH.position.y)
                    return;
            }
        }

        foreach (Transform item in transform)
        {
            if (item != null)
                cam.GetComponentInParent<MultipleTargetCamera>().targets.Remove(item);
        }

        InZone = false;
    }

    //  désactive la camera manuellement
    public void DeactivateCam()
    {
        foreach (Transform item in transform)
        {
            if (item != null)
                cam.GetComponentInParent<MultipleTargetCamera>().targets.Remove(item);
        }

        InZone = false;
    }


}
