﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        //bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // Pass all parameters to the character control script.
        m_Character.Move(h, false, m_Jump);
        m_Jump = false;

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            m_Character.UseWeapon();
        }

        if (CrossPlatformInputManager.GetButtonDown("TriggerAction1"))
        {
            //m_Character.PrintAllUpgrade();
        }
    }

}
