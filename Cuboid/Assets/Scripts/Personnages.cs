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
        public float m_JumpForce = 1000;
        public float speed = 600f;
        public ForceMode2D fMode;

        [Header("Resitance")]
        public bool resitGlace  = false;
        public bool resitPoison = false;
        public bool resitFoudre = false;

        [Header("Debuff")]
        [Tooltip("Defini si le personnage est gelé, pour changer le temps et le multiplicateur, aller dans le script Personnages")]
        public bool glace  = false;
            //Temps en seconde que le personnage est ralentit
            public static float tempsG = 5f;
            //Multiplicateur de ralentisement
            public static float mRalentit = 0.5f;

        [Tooltip("Defini si le personnage est empoisonné, pour changer le temps et les dommages , aller dans le script Personnages")]
        public bool poison = false;
            //Durée de l'empoisonnement en seconde
            public static float tempsP = 5f;
            //Dommage du poison par seconde
            public static int dommageP = 1;

        [Tooltip("Defini si le personnage est paralysé, pour changer le temps, aller dans le script Personnages")]
        public bool foudre = false;
            //Durée de la paralysi en seconde
            public static float tempsF = 5f;
    }

    abstract public void DommagePerso(int dommage);
}
