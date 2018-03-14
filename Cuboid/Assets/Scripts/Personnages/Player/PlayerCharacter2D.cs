
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class PlayerCharacter2D : Personnages {

    #region Variables
    public PersoStats joueurStats = new PersoStats();
    private float m_speed;

    Dictionary<string, bool> activeUpgradeTable { get; set; }

    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    public Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    public GameObject morphBombPrefab;
    public GameObject m_backSphere;     //  !*! Ajout pour animer le fond avec le transforme

    public float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.

    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private GameObject bar;

    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private float shootTimer = 0;

    public bool IsRunning = false;

    public float fallMaxSpeed = 35f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float decelleration = 4f;

    private bool m_DoubleJump = true;
    private bool isPlayerMorphed = false;

    public float dureeImmortel = 0.5f;

    private bool m_enableInput = true;
    #endregion

    #region Corps   
    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");

        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        activeUpgradeTable = new Dictionary<string, bool>();
        activeUpgradeTable.Clear();

        if(GameObject.FindGameObjectWithTag("HealthUI"))
            bar = GameObject.FindGameObjectWithTag("HealthUI");

        UpdateHealthBar();
    }
    
    private float LimitVelo(float velo, float max) {
        return max * Mathf.Sign(velo) * (Mathf.Abs(velo) - max);
    }
    void Update() {
        /*
        // Sert à limiter la vélocité
        Vector2 n_velo = Vector2.zero;
        
        if (Mathf.Abs(m_Rigidbody2D.velocity.x) > m_speed)
            n_velo.x = LimitVelo(m_Rigidbody2D.velocity.x, m_speed);

        if (Mathf.Abs(m_Rigidbody2D.velocity.y) > fallMaxSpeed)
            n_velo.y = LimitVelo(m_Rigidbody2D.velocity.y, fallMaxSpeed);

        if(!PauseMenu.GameIsPaused)
            m_Rigidbody2D.AddForce(-n_velo);
*/
        
        Vector3 clampVel = m_Rigidbody2D.velocity;
        clampVel.x = Mathf.Clamp(clampVel.x, -m_speed, m_speed);
        clampVel.y = Mathf.Clamp(clampVel.y, -fallMaxSpeed, fallMaxSpeed);

        m_Rigidbody2D.velocity = clampVel;
        

        //  section qui gère la rotation de la partie arrière de la sphere, ne pas enlever
        if (m_backSphere != null && (m_Rigidbody2D.velocity.x > 0.1f || m_Rigidbody2D.velocity.x < -0.1f))
            m_backSphere.transform.Rotate(0.0f, 0.0f, -Mathf.Abs(m_Rigidbody2D.velocity.x / 2));
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
            m_Anim.Play("RunFast");
            m_speed = joueurStats.maxSpeed*1.5f;
        }
        else
            m_speed = joueurStats.maxSpeed;
    }

    public void UseWeapon()
    {
        if (m_enableInput == false)
            return;

        Weapon currentWeapon = (Weapon) transform.Find("Weapon").gameObject.GetComponent(typeof(Weapon));
        if (currentWeapon == null)
        {
            Debug.LogError("Failed to find active weapon!");
        }
        else
        {
            if (isPlayerMorphed && activeUpgradeTable.ContainsKey("MorphBomb"))
            {
                Instantiate(morphBombPrefab, m_Rigidbody2D.position, Quaternion.identity);
            }
            else if (!isPlayerMorphed)
            {
                if (shootTimer > currentWeapon.fireCooldown)
                {
                    shootTimer = 0;
                    currentWeapon.Shoot(m_FacingRight);
                }
            }
        }
    }

    //  changer la valeur de m_enableInput qui désactive les mouvement et le tir du joueur
    public void setEnableInput(bool v)
    {
        m_enableInput = v;
    }

    #endregion

    #region Upgrade
    public void PrintAllUpgrade()
    {
        foreach (KeyValuePair<string, bool> item in activeUpgradeTable)
        {
            Debug.Log(item.ToString());
        }
    }

    public void ToggleUpgrade(string name)
    {
        if (activeUpgradeTable.ContainsKey(name))
        {
            //Bug: Est appelé même la première fois qu'on pickup un upgrade...
            //activeUpgradeTable[name] = !activeUpgradeTable[name];
        }
        else
        {
            activeUpgradeTable.Add(name, true);
        }
    }

    public void SetMorph(bool morph)
    {
        isPlayerMorphed = morph;
    }

    public bool IsUnderCeiling()
    {
        if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            return true;

        return false;
    }
    #endregion

    #region Mouvement
    public void Move(float move, bool crouch, bool jump)
    {
        if (PauseMenu.GameIsPaused || m_enableInput == false)
            return;

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

            //Ajuste la décélération selon l'emplitude du mouvement du joueur
            if (Mathf.Abs(move) < 0.5f)
                decelleration = 30f;
            else if (Mathf.Abs(move) > 0.9f)
                decelleration = 10f;
            else
                decelleration = Mathf.Lerp(decelleration, 10f, Time.deltaTime * 2f * Mathf.Abs(move));
                
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            Vector2 dir = new Vector2(move, 0f);

            dir *= joueurStats.speed * Time.fixedDeltaTime;

            m_Rigidbody2D.AddForce(dir, joueurStats.fMode);
            
            //Ce bout de code sert à enlever la patinage et a donné plus de controlle au joueur pour les petit mouvement au sol comme dans les air
            Vector2 n_Force;
            if (m_Grounded)
                n_Force = new Vector2(-m_Rigidbody2D.velocity.x * decelleration,0f);
            else
                n_Force = new Vector2(-m_Rigidbody2D.velocity.x * decelleration/1.5f,0f);
            
            m_Rigidbody2D.AddForce(n_Force);
            
            // If the input is moving the player right and the player is facing left...
            if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
                Flip();

        }
               
        // If the player should jump...
        if (!isPlayerMorphed && m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(Vector2.up * joueurStats.m_JumpForce, ForceMode2D.Impulse);
        }
        else if (!isPlayerMorphed && m_DoubleJump && jump)
        {
            m_DoubleJump = false;
            Vector2 d_jump;
            if (m_Rigidbody2D.velocity.y >= 0)
                d_jump = Vector2.up * joueurStats.m_JumpForce;
            else 
                d_jump = Vector2.up * (joueurStats.m_JumpForce + Mathf.Abs(m_Rigidbody2D.velocity.y)/2);


            m_Rigidbody2D.AddForce(d_jump, ForceMode2D.Impulse);
        }
    }

    private void BetterJumpPhysic(){
        if (m_Rigidbody2D.velocity.y < 0)
            m_Rigidbody2D.gravityScale = fallMultiplier;

        else if (m_Rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
            m_Rigidbody2D.gravityScale = lowJumpMultiplier;

        else
            m_Rigidbody2D.gravityScale = 4f;
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
    #endregion

    #region Dommage

    public override void DommagePerso(int dommage){
        if (!joueurStats.immortel && joueurStats.vie > 0) {

            joueurStats.immortel = true;
            StartCoroutine(ChangeImmortel());

            joueurStats.vie -= dommage;
            UpdateHealthBar();

            if (joueurStats.vie <= 0)
                GameMaster.KillJoueur(this);
            else
                CameraShaker.Instance.ShakeOnce(3f, 2f, .1f, dureeImmortel);
        }
    }

    private void UpdateHealthBar(){
        if(bar != null)
            bar.GetComponent<HealthBar>().health = joueurStats.vie;   
    }

    IEnumerator ChangeImmortel() {
        yield return new WaitForSeconds(dureeImmortel);
        joueurStats.immortel = false;
    }
    #endregion
}
