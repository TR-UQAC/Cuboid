using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

public class Platformer2DControls : MonoBehaviour
{

    //[RequireComponent(typeof(PlayerCharacter2D))]

    private PlayerCharacter2D m_Character;
    private bool m_Jump;


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
        

        if (CrossPlatformInputManager.GetButtonDown("TriggerAction1")) {
            //m_Character.PrintAllUpgrade();
        }

        if (CrossPlatformInputManager.GetButton("Run")) {
            m_Character.IsRunning = true;
        } else {
            m_Character.IsRunning = false;
        }

        if (!m_Jump){
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            m_Character.joueurStats.immortel = !m_Character.joueurStats.immortel;
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            GameMaster.KillJoueur(m_Character);
        }
    }
}
