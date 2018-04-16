using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    private LineRenderer viseur;

    //Les parametres pour les explosion
    public DegatAttaque statAttaque;

    public bool M_viser { get; set; }

    private Transform myTransform;
    [HideInInspector]
    public SpriteRenderer spriteR;

    public bool M_FacingRight { get; set; }
    public Vector2 direction;

    static bool manette = false;

    void Start() {
        activeBullet = bulletPref;

        firePoint = transform.Find("FirePoint");
        if (firePoint == null) {
            Debug.LogError("FirePoint not found!");
        }

        if (GameObject.FindGameObjectWithTag("MissileUI")) {
            missileUI = GameObject.FindGameObjectWithTag("MissileUI");
            missileUI.SetActive(false);
        }

        if (GetComponent<LineRenderer>() != null)
            viseur = GetComponent<LineRenderer>();


        spriteR = gameObject.GetComponent<SpriteRenderer>();

        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Changer la position de la sourir pour la position d'un objet qui tourne autour du jouer selon le déplacement de la sourie ou du joystick de la manette

        float x = CrossPlatformInputManager.GetAxis("Horizontal2");
        float y = CrossPlatformInputManager.GetAxis("Vertical2");

        if (x != 0 || y != 0)
            manette = true;

        if (M_viser) {

            if (manette) {
                direction.x = x;
                direction.y = y;
            }
            else
                direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - myTransform.position;

            if (direction == Vector2.zero)
                direction = M_FacingRight ? Vector2.left : Vector2.right;

            direction.Normalize();

            if (viseur != null)
                ViseurDistance();

            /*
            if (M_FacingRight) {
                if (direction.x > 0)
                    direction.x *= -1;
            } else if (direction.x < 0)
                direction.x *= -1;
                */
        } else 
            direction = M_FacingRight ? Vector2.left : Vector2.right;


        if (!M_FacingRight)
        {
            transform.localPosition = new Vector3(-0.1f, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(0.1f, transform.localPosition.y, transform.localPosition.z);
        }
        
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        myTransform.rotation = Quaternion.Euler(0f, 0f, rotZ);

    }

    void ViseurDistance() {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100.0f, ~noHit);

            if (hit.collider != null)
                viseur.SetPosition(1, Vector3.right * hit.distance);
            else
                viseur.SetPosition(1, Vector3.right * 100.0f);
    }

    private float Direction(float x) {
        if (x != 0)
            return Mathf.Sign(x);
        else
           return 0;
    }

    public void UpdateGUI(bool? on)
    {
        if (!missileUI.activeSelf)
        {
            missileUI.SetActive(true);
        }

        PlayerCharacter2D pc = FindObjectOfType<PlayerCharacter2D>();
        if (pc == null)
        {
            Debug.Log("Pas trouve le joueur en updatant le UI");
        }
        else
        {
            missileUI.GetComponentInChildren<TextMeshProUGUI>().text = pc.joueurStats.nbMissile + " / " + pc.joueurStats.nbMissileMax;
        }

        if (on == null)
        {
            return;
        }

        if ((bool)on)
        {
            missileUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 1);
        }
        else
        {
            missileUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 0.392f);
        }
    }

    public void UseMissile(bool on)
    {
        if (on)
        {
            activeBullet = missilePref;
            dmg *= 2;
            statAttaque.eRadius *= 2;
        }
        else
        {
            activeBullet = bulletPref;
            dmg /= 2;
            statAttaque.eRadius /= 2;
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
        {
            if(activeBullet.name == "Missile")
                FindObjectOfType<AudioManager>().Play("LaunchMissile");
            else
                FindObjectOfType<AudioManager>().Play("Shoot");
        }
    }
}



