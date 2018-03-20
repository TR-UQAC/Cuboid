using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    public string PickupType = "Health";
    public int Valeur = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter2D pc = other.GetComponent<PlayerCharacter2D>();

            switch (PickupType)
            {
                case "Health":
                    pc.SoinPerso(Valeur);
                break;
                case "Missile":
                    //pc.AddMunition(Valeur);
                break;
                default:
                    break;
            }

            FindObjectOfType<AudioManager>().Play("PickupSound");
            Destroy(gameObject);
        }
    }

}
