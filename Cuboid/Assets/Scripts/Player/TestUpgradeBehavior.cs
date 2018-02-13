using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TestUpgradeBehavior : MonoBehaviour {

    private bool petit = false;

	// Update is called once per frame
	void FixedUpdate () {

        

        if (CrossPlatformInputManager.GetButtonDown("TriggerAction1"))
        {
            Debug.Log("Nouveau Pouvoir activé! PewPew");

            if (FindObjectOfType<AudioManager>() != null)
            {
                PlayerCharacter2D p = FindObjectOfType<PlayerCharacter2D>();

                if (petit)
                {
                    petit = false;
                    p.transform.localScale = new Vector3(p.transform.localScale.x * 1.334f, p.transform.localScale.y * 1.334f);            
                }
                else
                {
                    petit = true;
                    p.transform.localScale = new Vector3(p.transform.localScale.x * 0.75f, p.transform.localScale.y * 0.75f);
                }

                FindObjectOfType<AudioManager>().Play("temp_UseItem");
            }
        }
	}
}
