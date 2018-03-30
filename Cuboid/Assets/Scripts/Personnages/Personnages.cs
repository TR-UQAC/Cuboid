using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Personnages : MonoBehaviour {

    [System.Serializable]
    public class PersoStats {
#if !UNITY_EDITOR
            [Header("Vie")]
#endif
        public int vie = 100;
        public int vieMax = 100;
        public int nbMissile = 5;
        public int nbMissileMax = 5;
        public bool immortel;


#if !UNITY_EDITOR
            [Space]
            [Header("Mouvement Force")]
#endif
        //La vitesse
        public float m_JumpForce = 0;
        public float speed = 600f;
        public float maxSpeed = 10f;
        public ForceMode2D fMode;

        [Header("Resitance")]
        public bool resitGlace  = false;
        public bool resitPoison = false;
        public bool resitFoudre = false;

        [Header("Debuff")]
        [Tooltip("Defini si le personnage est gelé, pour changer le temps et le multiplicateur, aller dans le script Personnages")]
        public bool glacer  = false;
            //Temps en seconde que le personnage est ralentit
            public static float tempsG = 5f;
            //Multiplicateur de ralentisement
            public static float mRalentit = 0.5f;

        [Tooltip("Defini si le personnage est empoisonné, pour changer le temps et les dommages , aller dans le script Personnages")]
        public bool empoisoner = false;
            //Durée de l'empoisonnement en seconde
            public static float tempsP = 5f;
            //Dommage du poison par seconde
            public static int dommageP = 1;

        [Tooltip("Defini si le personnage est paralysé, pour changer le temps, aller dans le script Personnages")]
        public bool paralyse = false;
            //Durée de la paralysie en seconde
            public static float tempsF = 5f;
    }

    abstract public void DommagePerso(int dommage);
    abstract public void SoinPerso(int valeur);
}
