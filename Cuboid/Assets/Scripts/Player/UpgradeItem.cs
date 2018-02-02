using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class UpgradeItem : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider2D player)
    {
        //feedback de pickup

        //fait de quoi sur le player
        //GameObject monjoueur = GameObject.FindGameObjectWithTag("Player");
        //PlatformerCharacter2D pc = (PlatformerCharacter2D) monjoueur.GetComponent(typeof(PlatformerCharacter2D));

        //pc.ToggleUpgrade("Test");

        //player.gameObject.AddComponent(typeof(SphereCollider));

        player.gameObject.AddComponent(typeof(TestUpgradeBehavior));

        Destroy(gameObject);
    }

}
