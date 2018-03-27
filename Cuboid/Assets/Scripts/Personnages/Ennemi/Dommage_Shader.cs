using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer), typeof(Ennemis))]
public class Dommage_Shader : MonoBehaviour {

    //Variable pour les matériaux avec le shader grid_ennemi
    private Renderer rend; // Le renderer
    private Color lightColor; //couleur de base des flash

    [Range(0f,2f)]
    public float duree; // Le temps avant de revenir à la couleur de base
    public Color dommageLightColor; //couleur des flash quand dommage

    // Use this for initialization
    void Start () {

        rend = GetComponent<SpriteRenderer>();
        rend.material.shader = Shader.Find("Unlit/grid_ennemi");

        lightColor = rend.material.GetColor("_LightColor");
    }

    // Appeler à partir de la fonction DommagePerso du script Ennemis
    public void CouleurDommage() {
        DOTween.KillAll();
        rend.material.SetColor("_LightColor", dommageLightColor);
        rend.material.DOColor(lightColor, "_LightColor", duree).SetEase(Ease.InExpo);
    }

}
