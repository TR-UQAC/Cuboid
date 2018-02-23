using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MorphBall : MonoBehaviour {

    public Sprite morphSprite;
    public Sprite standardSprite;

    private bool ismorphed = false;

	void Start ()
    {
        //morphSprite = Resources.Load("morph01", typeof(Sprite)) as Sprite;


    }
	
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("TriggerAction1"))
        {
            GameObject playergo = transform.root.gameObject;

            if (!ismorphed)
            {
                //Change le sprite           
                playergo.GetComponent<SpriteRenderer>().sprite = morphSprite;

                //Disable/change l'animator
                playergo.GetComponent<Animator>().enabled = false;

                //Disable le capsule collider
                playergo.GetComponent<PolygonCollider2D>().enabled = false;

                //Active le sphere collider
                playergo.GetComponent<CircleCollider2D>().enabled = true;

                ismorphed = true;
            }
            else
            {
                //Utilise le ceiling check pour voir si on peux unmorph
                if (!playergo.GetComponent<PlayerCharacter2D>().IsUnderCeiling())
                {
                    //TODO: Ajouter une force pour pas que le sprite aparaisse dans le plancher
                    playergo.GetComponent<SpriteRenderer>().sprite = standardSprite;
                    //playergo.GetComponent<Animator>().enabled = true;
                    playergo.GetComponent<PolygonCollider2D>().enabled = true;
                    playergo.GetComponent<CircleCollider2D>().enabled = false;

                    ismorphed = false;
                }
            }

        }
    }

}
