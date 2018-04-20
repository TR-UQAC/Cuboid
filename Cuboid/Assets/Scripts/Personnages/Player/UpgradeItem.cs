using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class UpgradeItem : MonoBehaviour {

    public string UpgradeName;
    private bool pris = false;

    void Awake()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play(UpgradeName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (pris)
                return;

            pris = true;

            //Desactive le collider pour empêcher le double pickup
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            Debug.Log("Get Upgrade");
            if (FindObjectOfType<AudioManager>() != null)
            {
                FindObjectOfType<AudioManager>().Play("ItemPickup");
            }

            PlayerCharacter2D pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter2D>();

            if (!pc.upgrading)
            {
                Debug.Log("entre ici");
                //pc.upgrading = true;
                Pickup(other);
                pc.upgrading = false;
            }
        }
    }

    void Pickup(Collider2D player)
    {
        //fait de quoi sur le player
        GameObject monjoueur = GameObject.FindGameObjectWithTag("Player");
        PlayerCharacter2D pc = (PlayerCharacter2D) monjoueur.GetComponent(typeof(PlayerCharacter2D));

        if (pc == null)
        {
            Debug.Log("Player not found");
            return;
        }

        if (pc.upgrading)
        {
            return;
        }
        else
        {
            pc.upgrading = true;
        }

        //pc.ToggleUpgrade(UpgradeName);

        if (!pc.HasUpgrade(UpgradeName) || (UpgradeName == "Missile" && pc.HasUpgrade("Missile")))
        {
            switch (UpgradeName)
            {
                case "MorphBall":
                    pc.ToggleUpgrade(UpgradeName);
                    pc.gameObject.AddComponent(typeof(MorphBall));
                    break;
                case "MorphBomb":
                    pc.ToggleUpgrade(UpgradeName);
                    break;
                case "GrappleBeam":
                    pc.ToggleUpgrade(UpgradeName);
                    pc.AddWeapon("GrappleBeam");
                    pc.gameObject.GetComponent<GrappleBeam>().enabled = true;
                    pc.gameObject.GetComponent<GrappleBeam>().UpdateGUI(false);
                    break;
                case "Missile":
                    pc.joueurStats.nbMissileMax += 5;
                    pc.joueurStats.nbMissile = pc.joueurStats.nbMissileMax;
                    if (pc.HasUpgrade("Missile")){
                        pc.UpdateMissileUI();
                    } else {
                        pc.ToggleUpgrade(UpgradeName);
                        pc.AddWeapon("Missile");
                    }
                    Debug.Log("missile max 1: " + pc.joueurStats.nbMissileMax);
                    break;
                case "MissileExpansion":
                    pc.joueurStats.nbMissileMax += 5;
                    pc.joueurStats.nbMissile = pc.joueurStats.nbMissileMax;
                    pc.UpdateMissileUI();
                    break;
                case "HealthExpansion":
                    pc.joueurStats.vieMax += 100;
                    pc.joueurStats.vie = pc.joueurStats.vieMax;
                    pc.UpdateHealthBar();
                    break;
                default:
                    Debug.Log("Upgrade non spécifiée ou non reconnue");
                    pc.gameObject.AddComponent(typeof(TestUpgradeBehavior));
                    break;
            }

            gameObject.GetComponent<Animator>().Play("Disapear");
        }

        GetComponent<DialogueTrigger>().TriggerDialogue();

        Destroy(gameObject, 0.25f);
    }
}
