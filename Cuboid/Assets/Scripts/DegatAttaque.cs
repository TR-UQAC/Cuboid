using System;
using UnityEngine;


[System.Serializable]
public class DegatAttaque {
    [Header("Param Explosion")]
    public float ePower;
    public float eRadius;
    public float upwardsModifier;

    [Header("Élément")]
    public bool degGlace;
    public bool degPoison;
    public bool degFoudre;

    public DegatAttaque(float p, float r, float u, bool g = false, bool poison = false, bool f = false) {
        this.ePower = p;
        this.eRadius = r;
        this.upwardsModifier = u;

        this.degGlace = g;
        this.degPoison = poison;
        this.degFoudre = f;
    }

    public DegatAttaque(DegatAttaque a) {

        this.ePower = a.ePower;
        this.eRadius = a.eRadius;
        this.upwardsModifier = a.upwardsModifier;

        this.degGlace = a.degGlace;
        this.degPoison = a.degPoison;
        this.degFoudre = a.degFoudre;
    }

}
