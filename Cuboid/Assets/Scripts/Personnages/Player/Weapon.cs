using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour
{
    public float fireCooldown = 0;
    public int dmg = 100;
    public LayerMask noHit;
    public LayerMask dommageHit;

    public GameObject bulletPref;
    public GameObject missilePref;
    private GameObject activeBullet;
    private Transform firePoint;
    private GameObject missileUI;

    //Les parametres pour les explosion
    public DegatAttaque statAttaque;

    public bool vise = false;

    private Transform myTransform;
    public bool M_FacingRight { get; set; }
    public Vector2 direction;

    void Awake()
    {
        activeBullet = bulletPref;

        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found!");
        }

        if (GameObject.FindGameObjectWithTag("MissileUI"))
        {
            missileUI = GameObject.FindGameObjectWithTag("MissileUI");
            missileUI.SetActive(false);
        }

        myTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Changer la position de la sourir pour la position d'un objet qui tourne autour du jouer selon le déplacement de la sourie ou du joystick de la manette

        float x = CrossPlatformInputManager.GetAxis("Horizontal2");
        float y = CrossPlatformInputManager.GetAxis("Vertical2");

        //float y = CrossPlatformInputManager.GetAxis("Mouse Y");

        if (vise) {

            direction.x = -x;
            direction.y = y;
            //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - myTransform.position;

            if (direction == Vector2.zero)
                direction = M_FacingRight ? Vector2.left : Vector2.right;

            direction.Normalize();
            /*
            if (M_FacingRight) {
                if (direction.x > 0)
                    direction.x *= -1;
            } else if (direction.x < 0)
                direction.x *= -1;
                */
        } else 
            direction = M_FacingRight ? Vector2.left : Vector2.right;

    

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        myTransform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    private float Direction(float x) {
        if (x != 0)
            return Mathf.Sign(x);
        else
           return 0;
    }

    public void UpdateGUI(bool on)
    {
        if (!missileUI.activeSelf)
        {
            missileUI.SetActive(true);
        }

        if (on)
        {
            missileUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 1);
        }
        else
        {
            missileUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 0.098f);
        }
    }

    public void UseMissile(bool on)
    {
        if (on)
        {
            activeBullet = missilePref;
        }
        else
        {
            activeBullet = bulletPref;
        }
    }

    public void Shoot()
    {
        //TODO: Effet particule de tir
        Bullet bul = Instantiate(activeBullet, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bul.direction = direction;

        bul.noHit = noHit;
        bul.dommageHit = dommageHit;
        bul.dmg = dmg;
        bul.statAttaque = statAttaque;

        if (FindObjectOfType<AudioManager>() != null)
            FindObjectOfType<AudioManager>().Play("Shoot");
        
    }
}



