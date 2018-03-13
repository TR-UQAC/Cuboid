using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [Tooltip("Le voisin de gauche du noeud actuelle")]
    public Node m_VoisinGauche;

    [Tooltip("Le voisin de Droit du noeud actuelle")]
    public Node m_VoisinDroit;

    //  retourne le voisin de gauche, s'il n'y en a pas, il va retourné celui de droite
    //  s'il y a toujours rien, il retourne null, prévoir cette éventualité en cas d'appel !!!!
    public Transform getGauche()
    {
        if (m_VoisinGauche != null)
            return m_VoisinGauche.transform;
        else
            return null;
    }

    //  retourne le voisin de droite, s'il n'y en a pas, il va retourné celui de gauche
    //  s'il y a toujours rien, il retourne null, prévoir cette éventualité en cas d'appel !!!!
    public Transform getDroit()
    {
        if (m_VoisinDroit != null)
            return m_VoisinDroit.transform;
        else
            return null;
    }

    public void OnDrawGizmos()
    {
        if (m_VoisinGauche != null)
        {
            Vector3 delta = m_VoisinGauche.transform.position - transform.position;
            Vector3 offset = delta.normalized * 0.25f;
            Gizmos.color = new Color(1, 0, 1);
            Gizmos.DrawLine(transform.position + new Vector3(0.0f, offset.y, 0), m_VoisinGauche.transform.position + new Vector3(0.0f, offset.y, 0));
        }

        if (m_VoisinDroit != null)
        {
            Vector2 delta = m_VoisinDroit.transform.position - transform.position;
            Vector3 offset = delta.normalized * 0.25f;
            Gizmos.color = new Color(1, 0, 1);
            Gizmos.DrawLine(transform.position + new Vector3(0.0f, offset.x, 0), m_VoisinDroit.transform.position + new Vector3(0.0f, offset.x, 0));
        }
    }
}
