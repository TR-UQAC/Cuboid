using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxMultiCam : MonoBehaviour {

    public Camera cam;
    private bool InZone = false;

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogWarning("Aucune camera relier a triggerBoxMultiCam");
            cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" || InZone == true)
            return;

        foreach (Transform item in transform)
        {
            if(item != null)
                cam.GetComponent<MultipleTargetCamera>().targets.Add(item);
        }
        InZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        foreach (Transform item in transform)
        {
            if (item != null)
                cam.GetComponent<MultipleTargetCamera>().targets.Remove(item);
        }
        InZone = false;
    }


}
