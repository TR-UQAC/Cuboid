using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxing : MonoBehaviour {

    public List<Transform> m_Backgrounds;   //  la liste des différent background pour le parallaxing
    private float[] m_ParallaxScales;   //  la proportion du mouvement de la camera a bouger le background
    public float m_smoothing = 1.0f;    //  a quelle point le parallax est smooth, doit être > 0

    private Transform cam;
    private Vector3 m_previousCamPos;

    void Awake() {
        cam = GameObject.Find("CameraHolder").transform;

        //cam = Camera.main.GetComponentInParent<Transform>().transform;
    }

    // Use this for initialization
    void Start () {
        //  initialiser la position
        m_previousCamPos = cam.position;

        //  assigner proportion de déplacement des background
        m_ParallaxScales = new float[m_Backgrounds.Count];
        for (int i = 0; i < m_Backgrounds.Count; i++)
        {
            m_ParallaxScales[i] = m_Backgrounds[i].position.z * -1;
        }

	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < m_Backgrounds.Count; i++)
        {
            //  le parallax est l'opposé du mouvement de la camera car la frame précédente multiplie le scale
            float parallax = (m_previousCamPos.x - cam.position.x) * m_ParallaxScales[i];

            //  set une target x position qui est la position actuelle + le parallax
            //  creer une postion cible qui est la position actuelle du background avec la cible en X
            Vector3 backgroundTarget = new Vector3(m_Backgrounds[i].position.x + parallax, m_Backgrounds[i].position.y, m_Backgrounds[i].position.z);

            //  smooth le changement de position entre celle actuelle et celle cibler avec lerp
            m_Backgrounds[i].position = Vector3.Lerp(m_Backgrounds[i].position, backgroundTarget, m_smoothing * Time.deltaTime);
        }

        //  set la position précédente avec la nouvelle position
        m_previousCamPos = cam.position;
	}
}
