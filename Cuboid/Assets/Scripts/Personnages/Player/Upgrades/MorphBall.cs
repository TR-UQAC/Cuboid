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
               // playergo.GetComponent<SpriteRenderer>().sprite = morphSprite;

                //Disable/change l'animator
                //playergo.GetComponent<Animator>().enabled = false;

                //Disable le collider standard
                //Active le petit collider
                CircleCollider2D col1 = playergo.GetComponents<CircleCollider2D>()[0];
                CircleCollider2D col2 = playergo.GetComponents<CircleCollider2D>()[1];
                if (col1.radius > col2.radius)
                {
                    col1.enabled = false;
                    col2.enabled = true;
                }
                else
                {
                    col1.enabled = true;
                    col2.enabled = false;
                }

                ismorphed = true;
                playergo.GetComponent<PlayerCharacter2D>().SetMorph(ismorphed);
                playergo.GetComponent<Animator>().SetBool("Morphed", ismorphed);    
            }
            else
            {
                //Utilise le ceiling check pour voir si on peux unmorph
                if (!playergo.GetComponent<PlayerCharacter2D>().IsUnderCeiling())
                {
                    //playergo.GetComponent<SpriteRenderer>().sprite = standardSprite;
                    //playergo.GetComponent<Animator>().enabled = true;

                    ismorphed = false;
                    playergo.GetComponent<PlayerCharacter2D>().SetMorph(ismorphed);
                    playergo.GetComponent<Animator>().SetBool("Morphed", ismorphed);

                    //Active le collider standard
                    //Desactive le petit collider
                    CircleCollider2D col1 = playergo.GetComponents<CircleCollider2D>()[0];
                    CircleCollider2D col2 = playergo.GetComponents<CircleCollider2D>()[1];
                    if (col1.radius < col2.radius)
                    {
                        col1.enabled = false;
                        col2.enabled = true;
                    }
                    else
                    {
                        col1.enabled = true;
                        col2.enabled = false;
                    }
                }
            }

        }
    }

}
