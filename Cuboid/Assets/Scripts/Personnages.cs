using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Personnages : MonoBehaviour {

    [System.Serializable]
    public class PersoStats {
        [Header("Vie")]
        public int vie = 100;
        public int vieMax = 100;

        [Space]

        [Header("Mouvement")]
        //La vitesse
        public float hauteurSaut = 100;
        public float speed = 600f;
        public ForceMode2D fMode;

        [Space]

        [Header("Resitance")]
        public bool resitGlace  = false;
        public bool resitPoison = false;
        public bool resitFoudre = false;

        [Header("Debuff")]
        public bool glace  = false;
        public bool poison = false;
        public bool foudre = false;
    }

    abstract public void DommagePerso(int dommage);
}
