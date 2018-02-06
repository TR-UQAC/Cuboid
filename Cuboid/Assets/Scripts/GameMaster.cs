using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static void KillEnnemi(Ennemis perso) {
        Destroy(perso.gameObject);
    }
}
