using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour {

    public float health = 100;
    public Image healthImage;
    public TextMeshProUGUI textMesh;

    void Update()
    {
        healthImage.fillAmount = (health/ 100);
        textMesh.text = Mathf.Round(health).ToString();
    }

}
