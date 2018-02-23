
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//TODO: ! Ajout d'un temps d'immortalité pour le joueur
public class PlayerCharacter2D : Personnages {

    public PersoStats joueurStats = new PersoStats();

    Dictionary<string, bool> activeUpgradeTable { get; set; }

    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.

    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private GameObject bar;

    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private float shootTimer = 0;

    public bool IsRunning = false;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float decelleration = 4f;

    private bool m_DoubleJump = true;

    public float dureeImmortel = 0.5f;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        activeUpgradeTable = new Dictionary<string, bool>();

        if(GameObject.FindGameObjectWithTag("HealthUI"))
            bar = GameObject.FindGameObjectWithTag("HealthUI");

        UpdateHealthBar();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
            m_DoubleJump = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        BetterJumpPhysic();
        shootTimer += Time.deltaTime;

        if (IsRunning)
        {
            Debug.Log("Je cours !");
            m_Anim.Play("RunFast");
            joueurStats.maxSpeed = 20f;
        }
        else
        {
            joueurStats.maxSpeed = 10f;
        }

        if(m_Grounded)
            m_Rigidbody2D.AddRelativeForce(-m_Rigidbody2D.velocity * decelleration);
    }

    public void PrintAllUpgrade()
    {
        foreach (KeyValuePair<string, bool> item in activeUpgradeTable)
        {
            Debug.Log(item.ToString());
        }
    }

    public void UseWeapon()
    {
        Weapon currentWeapon = (Weapon) transform.Find("Weapon").gameObject.GetComponent(typeof(Weapon));
        if (currentWeapon == null)
        {
            Debug.LogError("Failed to find active weapon!");
        }
        else
        {
            if (shootTimer > currentWeapon.fireCooldown)
            {
                shootTimer = 0;
                currentWeapon.Shoot(m_FacingRight);
            }
        }
    }
    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            Vector2 dir = new Vector2(move, 0f);

            dir *= joueurStats.speed * Time.fixedDeltaTime;
            //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x + move * joueurStats.speed, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.AddForce(dir, joueurStats.fMode);

            if (m_Rigidbody2D.velocity.x > joueurStats.maxSpeed)
                m_Rigidbody2D.velocity = new Vector2(joueurStats.maxSpeed, m_Rigidbody2D.velocity.y);

            else if (m_Rigidbody2D.velocity.x < -joueurStats.maxSpeed)
                m_Rigidbody2D.velocity = new Vector2(-joueurStats.maxSpeed, m_Rigidbody2D.velocity.y);


            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(Vector2.up * joueurStats.m_JumpForce, ForceMode2D.Impulse);
        }
        else if (m_DoubleJump && jump)
        {
            m_DoubleJump = false;
            Vector2 d_jump;
            if (m_Rigidbody2D.velocity.y >= 0)
                d_jump = Vector2.up * joueurStats.m_JumpForce;
            else 
                d_jump = Vector2.up * (joueurStats.m_JumpForce +Mathf.Abs(m_Rigidbody2D.velocity.y));


            m_Rigidbody2D.AddForce(d_jump, ForceMode2D.Impulse);
        }
    }

    private void BetterJumpPhysic()
    {

        if (m_Rigidbody2D.velocity.y < 0)
        {
            m_Rigidbody2D.gravityScale = fallMultiplier;
            //m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (m_Rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            m_Rigidbody2D.gravityScale = lowJumpMultiplier;
            //m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        } else {
            m_Rigidbody2D.gravityScale = 4f;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public void ToggleUpgrade(string name)
    {
        if (activeUpgradeTable.ContainsKey(name))
        {
            activeUpgradeTable[name] = !activeUpgradeTable[name];
        }
        else
        {
            activeUpgradeTable.Add(name, true);
        }
    }

    public override void DommagePerso(int dommage)
    {
        UpdateHealthBar();

        if (!joueurStats.immortel && joueurStats.vie > 0) {
            joueurStats.immortel = true;
            StartCoroutine(ChangeImmortel());
            joueurStats.vie -= dommage;
            if (joueurStats.vie <= 0) {
                GameMaster.KillJoueur(this);
            }
        }
    }

    public bool IsUnderCeiling()
    {
        if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
        {
            return true;
        }

        return false;
    }


    private void UpdateHealthBar()
    {

        //GameObject bar = GameObject.FindGameObjectWithTag("HealthUI");
        if(bar != null)
            bar.GetComponent<HealthBar>().health = joueurStats.vie;   
    }

    IEnumerator ChangeImmortel() {
        yield return new WaitForSeconds(dureeImmortel);
        joueurStats.immortel = false;
    }
}
