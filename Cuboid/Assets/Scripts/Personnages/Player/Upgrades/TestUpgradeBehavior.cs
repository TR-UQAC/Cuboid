using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TestUpgradeBehavior : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        

        if (CrossPlatformInputManager.GetButtonDown("TriggerAction1"))
        {
            //Debug.Log("Nouveau Pouvoir activé! PewPew");

            if (FindObjectOfType<AudioManager>() != null)
            {
                FindObjectOfType<AudioManager>().Play("temp_UseItem");
            }
        }
	}
}
