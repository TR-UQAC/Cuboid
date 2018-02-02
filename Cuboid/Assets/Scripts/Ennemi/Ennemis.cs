using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemis : Personnages {

    public PersoStats ennemiStats = new PersoStats();

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
            DommagePerso(50);
    }

    public override void DommagePerso(int dommage) {
        ennemiStats.vie -= dommage;
        if(ennemiStats.vie <= 0) {
            GameMaster.KillEnnemi(this);
        }
    }
}
