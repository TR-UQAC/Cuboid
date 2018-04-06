using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidTrap : MonoBehaviour {

    #region Variable

    [Tooltip("Les dégats reçus quand le joueur ce fait écrasé")]
    public int m_dmg = 5;

    [Tooltip("A qui la presse fait des DMG")]
    public LayerMask dommageHit;

    #endregion

    //  si le joueur est détecter, l'écrase et lui fait des DMG
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject coll = collision.gameObject;

            if (dommageHit == (dommageHit | (1 << coll.layer)))
            {
                Personnages en = coll.GetComponent<Personnages>() as Personnages;
                en.DommagePerso(m_dmg);
            }
        }
    }
}
