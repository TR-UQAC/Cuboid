using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Platformer2DControls : MonoBehaviour
{

    //[RequireComponent(typeof(PlayerCharacter2D))]

    private PlayerCharacter2D m_Character;
    private bool m_isAxisInUse = false;
    private bool m_Jump;

    private bool upgradeCheatFlag = false; //garde un check pcq si on active tous les upgrades plusieurs fois, ca risque de faire n'importe quoi


    private void Awake()
    {
        m_Character = GetComponent<PlayerCharacter2D>();
    }


    private void Update()
    {
        // Read the inputs.
        //bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        //float primaryAttack = Input.GetAxis("Fire1");

        // Pass all parameters to the character control script.
        m_Character.Move(h, false, m_Jump);
        m_Jump = false;

        if (CrossPlatformInputManager.GetButtonDown("Fire1") || CrossPlatformInputManager.GetAxis("Fire1") != 0)
        {
            m_Character.UseWeapon();
        }
        
        if (Input.GetAxisRaw("Fire3") != 0) {
            if (m_isAxisInUse == false) {
                m_Character.Viser();
                m_isAxisInUse = true;
            }
        }
        if (Input.GetAxisRaw("Fire3") == 0) {
            if(m_isAxisInUse)
                m_Character.Viser();

            m_isAxisInUse = false;
        }

        if (CrossPlatformInputManager.GetButtonDown("WeaponSelect"))
        {
            m_Character.WeaponSwitch();
        }

        //Rappel pour le grappin
        if (CrossPlatformInputManager.GetAxis("Vertical") != 0)
        {
            if (m_Character.GetComponent<GrappleBeam>().isGrappleAttached)
            {
                m_Character.GetComponent<GrappleBeam>().HandleGrappleLength(CrossPlatformInputManager.GetAxis("Vertical"));
            }
        }

        if (CrossPlatformInputManager.GetButtonDown("UpgradeCheat"))
        {
            if (!upgradeCheatFlag)
            {
                Debug.Log("All upgrade active");
                m_Character.ToggleUpgrade("MorphBall");
                m_Character.gameObject.AddComponent(typeof(MorphBall));

                m_Character.ToggleUpgrade("MorphBomb");

                m_Character.ToggleUpgrade("Missile");
                m_Character.AddWeapon("Missile");

                m_Character.ToggleUpgrade("GrappleBeam");
                m_Character.AddWeapon("GrappleBeam");
                m_Character.gameObject.GetComponent<GrappleBeam>().enabled = true;
                m_Character.gameObject.GetComponent<GrappleBeam>().UpdateGUI(false);

                m_Character.joueurStats.nbMissileMax += 5;
                m_Character.joueurStats.nbMissile = m_Character.joueurStats.nbMissileMax;
                m_Character.UpdateMissileUI();

                m_Character.joueurStats.vieMax += 100;
                m_Character.joueurStats.vie = m_Character.joueurStats.vieMax;
                m_Character.UpdateHealthBar();
                upgradeCheatFlag = true;
            }
            else
            {
                Debug.Log("Upgrade cheat deja activé");
            }
        }

        if (CrossPlatformInputManager.GetButtonDown("TriggerAction1")) {
            //m_Character.PrintAllUpgrade();
        }

        if (CrossPlatformInputManager.GetButton("Run")) {
            m_Character.IsRunning = true;
        } else {
            m_Character.IsRunning = false;
        }

        if (!m_Jump) {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            m_Character.joueurStats.immortel = !m_Character.joueurStats.immortel;
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            GameMaster.KillJoueur(m_Character);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameObject.Find("Boss_Ecrabouilleur") != null) {
                boss bo = GameObject.Find("Boss_Ecrabouilleur").GetComponent<boss>();
                bo.CheatLifeBoss();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            if (GameObject.Find("Spawn_Folder") != null) {
                AllSpawners all = GameObject.Find("Spawn_Folder").GetComponent<AllSpawners>();
                all.SpawnAllEnnemis();
            }
        }
    }
}
