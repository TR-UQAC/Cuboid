using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    public string PickupType = "Health";
    public int Valeur = 1;

    void Awake()
    {
        GetComponent<Animator>().Play(PickupType);

        Destroy(gameObject, 20f);
    }

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
                    pc.joueurStats.nbMissile += Valeur;
                    if (pc.joueurStats.nbMissile > pc.joueurStats.nbMissileMax)
                    {
                        pc.joueurStats.nbMissile = pc.joueurStats.nbMissileMax;
                    }
                    pc.UpdateMissileUI();
                break;
                default:
                    break;
            }

            if(FindObjectOfType<AudioManager>())
                FindObjectOfType<AudioManager>().Play("PickupSound");
            Destroy(gameObject);
        }
    }

}
