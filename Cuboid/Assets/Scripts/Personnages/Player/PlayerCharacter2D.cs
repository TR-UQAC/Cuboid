
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using DG.Tweening;

public class PlayerCharacter2D : Personnages {

    #region Variables
    public PersoStats joueurStats = new PersoStats();
    private float m_speed;

    Dictionary<string, bool> activeUpgradeTable { get; set; }
    private List<string> weaponList;
    public int selectedWeaponIndex = 0;

    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    public Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    public GameObject morphBombPrefab;
    public GameObject m_backSphere;     //  !*! Ajout pour animer le fond avec le transforme
    public GameObject m_ExplosionEffect;
    public bool upgrading = false;

    public float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.

    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private SpriteRenderer spriteR;
    private GameObject bar;
    private Weapon currentWeapon;

    //private bool m_FacingRight = true;  // For determining which way the player is currently facing.
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

        weaponList = new List<string>();
        AddWeapon("BasicBeam");

        if(GameObject.FindGameObjectWithTag("HealthUI"))
            bar = GameObject.FindGameObjectWithTag("HealthUI");

        GameObject.FindGameObjectWithTag("BulletUI").GetComponent<Image>().color = new Vector4(1, 1, 1, 1);

        spriteR = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(LateStart(0.1f));
        
        currentWeapon = (Weapon)transform.Find("Weapon").gameObject.GetComponent(typeof(Weapon));

        if (m_backSphere != null)
            m_backSphere.GetComponent<SpriteRenderer>().flipX = spriteR.flipX;

        UpdateHealthBar();
    }

    void Update() {
        Vector3 clampVel = m_Rigidbody2D.velocity;
        clampVel.x = Mathf.Clamp(clampVel.x, -m_speed, m_speed);
        clampVel.y = Mathf.Clamp(clampVel.y, -fallMaxSpeed, fallMaxSpeed);

        m_Rigidbody2D.velocity = clampVel;
        

        //  section qui gère la rotation de la partie arrière de la sphere, ne pas enlever
        if (m_backSphere != null && (m_Rigidbody2D.velocity.x > 0.1f || m_Rigidbody2D.velocity.x < -0.1f))
            m_backSphere.transform.Rotate(0.0f, 0.0f, -m_Rigidbody2D.velocity.x / 2);


        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameMaster.StartEscapeSequence();
        }
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

       /* if (currentWeapon.M_viser)
            m_speed = 5f;
        else */if (IsRunning)
            m_speed = joueurStats.maxSpeed*1.5f;
        else
            m_speed = joueurStats.maxSpeed;
    }
    void LateUpdate()
    {
        Vector3 clampVel = m_Rigidbody2D.velocity;
        clampVel.x = Mathf.Clamp(clampVel.x, -m_speed, m_speed);
        clampVel.y = Mathf.Clamp(clampVel.y, -fallMaxSpeed, fallMaxSpeed);

        m_Rigidbody2D.velocity = clampVel;
    }

    public void UseWeapon()
    {
        if (m_enableInput == false)
            return;

        //Weapon currentWeapon = (Weapon)transform.Find("Weapon").gameObject.GetComponent(typeof(Weapon));
        if (currentWeapon == null)
        {
            Debug.LogError("Failed to find active weapon!");
        }
        else
        {
            if (isPlayerMorphed && activeUpgradeTable.ContainsKey("MorphBomb"))
            {
                if (shootTimer > currentWeapon.fireCooldown)
                {
                    Instantiate(morphBombPrefab, m_Rigidbody2D.position, Quaternion.identity);
                    shootTimer = 0;
                }
            }
            else if (!isPlayerMorphed)
            {
                if (shootTimer > currentWeapon.fireCooldown)
                {
                    if (activeUpgradeTable.ContainsKey("GrappleBeam") && weaponList[selectedWeaponIndex] == "GrappleBeam")
                    {
                        gameObject.GetComponent<GrappleBeam>().UseGrapple();
                    }
                    else
                    {
                        if (weaponList[selectedWeaponIndex] == "Missile")
                        {
                            if (joueurStats.nbMissile > 0)
                            {
                                currentWeapon.Shoot();
                                joueurStats.nbMissile--;
                                currentWeapon.UpdateGUI(true);
                            }
                            else
                            {
                                //jouer un son
                            }
                        }
                        else
                        {
                            currentWeapon.Shoot();
                        }
                    }

                    shootTimer = 0;
                }
            }
        }
    }

    public void Viser() {
        currentWeapon.M_viser = !currentWeapon.M_viser;

        if (currentWeapon.GetComponent<LineRenderer>())
            currentWeapon.GetComponent<LineRenderer>().enabled = currentWeapon.M_viser;
    }

    //  changer la valeur de m_enableInput qui désactive les mouvement et le tir du joueur
    public void setEnableInput(bool v)
    {
        m_enableInput = v;
    }

    IEnumerator LateStart(float waitTime) {
        yield return new WaitForSeconds(waitTime);

        currentWeapon.M_FacingRight = spriteR.flipX;
        currentWeapon.spriteR.flipX = spriteR.flipX;
        currentWeapon.M_viser = false;
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

    public bool HasUpgrade(string name)
    {
        if (activeUpgradeTable.ContainsKey(name))
        {
            return true;
        }

        return false;
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

    public void AddWeapon(string weaponName)
    {
        weaponList.Add(weaponName);

        if (weaponName == "Missile")
        {
            Weapon currentWeapon = (Weapon)transform.Find("Weapon").gameObject.GetComponent(typeof(Weapon));
            currentWeapon.UpdateGUI(false);
        }
        
    }

    public void SetMorph(bool morph)
    {
        isPlayerMorphed = morph;

        Weapon currentWeapon = (Weapon)transform.Find("Weapon").gameObject.GetComponent(typeof(Weapon));
        if (currentWeapon == null)
        {
            Debug.LogError("Failed to find active weapon!");
        }
        else
        {
            if (morph)
            {
                currentWeapon.fireCooldown /= 2;
            }
            else
            {
                currentWeapon.fireCooldown *= 2;
            }        
        }
    }

    public bool IsUnderCeiling()
    {
        if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            return true;

        return false;
    }

    public bool IsGrounded()
    {
        return m_Grounded;
    }

    public void WeaponSwitch(bool avance)
    {
        string previousWeapon = weaponList[selectedWeaponIndex];

        if (avance)
        {
            selectedWeaponIndex++;
        }
        else
        {
            selectedWeaponIndex--;
        }
        
        if (selectedWeaponIndex == weaponList.Count)
        {
            selectedWeaponIndex = 0;
        }
        else if (selectedWeaponIndex < 0)
        {
            selectedWeaponIndex = weaponList.Count - 1;
        }

        if(weaponList.Count > 1)
        {
            if (FindObjectOfType<AudioManager>() != null)
                FindObjectOfType<AudioManager>().Play("ChangeGun");
        }

        Weapon currentWeapon = (Weapon)transform.Find("Weapon").gameObject.GetComponent(typeof(Weapon));

        switch (weaponList[selectedWeaponIndex])
        {
            case "BasicBeam":
                if (previousWeapon == "GrappleBeam")
                {
                    gameObject.GetComponent<GrappleBeam>().UpdateGUI(false);
                }
                else if(previousWeapon == "Missile")
                {
                    currentWeapon.UseMissile(false);
                    currentWeapon.UpdateGUI(false);
                }
                GameObject.FindGameObjectWithTag("BulletUI").GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            break;
            case "GrappleBeam":
                if (previousWeapon == "Missile")
                {
                    currentWeapon.UseMissile(false);
                    currentWeapon.UpdateGUI(false);
                }
                gameObject.GetComponent<GrappleBeam>().UpdateGUI(true);
                GameObject.FindGameObjectWithTag("BulletUI").GetComponent<Image>().color = new Vector4(1, 1, 1, 0.392f);
                break;
            case "Missile":
                gameObject.GetComponent<GrappleBeam>().UpdateGUI(false);
                currentWeapon.UseMissile(true);
                currentWeapon.UpdateGUI(true);
                GameObject.FindGameObjectWithTag("BulletUI").GetComponent<Image>().color = new Vector4(1, 1, 1, 0.392f);
                break;
            default:
                gameObject.GetComponent<GrappleBeam>().UpdateGUI(false);
                currentWeapon.UseMissile(false);
                currentWeapon.UpdateGUI(false);
            break;
        }

        Debug.Log("Active Weapon: " + weaponList[selectedWeaponIndex]);
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
            if (!GetComponent<GrappleBeam>().isGrappleAttached)
            {
                if (Mathf.Abs(move) < 0.5f)
                    decelleration = 30f;
                else if (Mathf.Abs(move) > 0.9f)
                    decelleration = 10f;
                else
                    decelleration = Mathf.Lerp(decelleration, 10f, Time.deltaTime * 2f * Mathf.Abs(move));
            }
            else
            {
                decelleration = 0f;
            }           
                
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
            if ((move > 0 && spriteR.flipX) || (move < 0 && !spriteR.flipX))
                Flip(move);

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

    private void Flip(float move)
    {
        spriteR.flipX = !spriteR.flipX;
        currentWeapon.M_FacingRight = spriteR.flipX;
        //currentWeapon.spriteR.flipX = spriteR.flipX;
      
        //currentWeapon.transform.localPosition = new Vector3(currentWeapon.transform.localPosition.x * -(Mathf.Sign(move)), currentWeapon.transform.localPosition.y, currentWeapon.transform.localPosition.z);

        m_backSphere.GetComponent<SpriteRenderer>().flipX = spriteR.flipX;
        
        // Switch the way the player is labelled as facing.
        //m_FacingRight = !m_FacingRight;

    }
    #endregion

    #region Dommage

    public override void DommagePerso(int dommage){
        //Debug.Log("Dommage");
        if (!joueurStats.immortel && joueurStats.vie > 0) {

            if(isPlayerMorphed == false)
                GetComponent<Animator>().Play("PlayerDamage");
            

            joueurStats.immortel = true;
            StartCoroutine(ChangeImmortel());

            joueurStats.vie -= dommage;
            UpdateHealthBar();

            if (joueurStats.vie <= 0)
            {
                GameObject ex = Instantiate(m_ExplosionEffect, transform.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("ExplosionBoss");

                Sequence bossKill = DOTween.Sequence();
                bossKill.SetDelay(2.0f);
                bossKill.AppendCallback(() =>
                {
                    Destroy(ex);
                });

                GameMaster.KillJoueur(this);
            }
            else
            {
                CameraShaker.Instance.ShakeOnce(3f, 2f, .1f, dureeImmortel);
                if (FindObjectOfType<AudioManager>() != null)
                    FindObjectOfType<AudioManager>().Play("HurtPlayer");
            }
        }
    }

    public override void SoinPerso(int valeur)
    {
        joueurStats.vie += valeur;
        if (joueurStats.vie > joueurStats.vieMax)
        {
            joueurStats.vie = joueurStats.vieMax;
        }

        UpdateHealthBar();
    }

    public void UpdateMissileUI()
    {
        currentWeapon.UpdateGUI(null);
    }

    public void UpdateHealthBar()
    {
        if(bar != null)
        {
            if (joueurStats.vie < 0)
            {
                bar.GetComponent<HealthBar>().health = 0;
            }
            else
            {
                bar.GetComponent<HealthBar>().health = joueurStats.vie;
                bar.GetComponent<HealthBar>().maxHealth = joueurStats.vieMax;
            }           
        }             
    }

    IEnumerator ChangeImmortel() {
        yield return new WaitForSeconds(dureeImmortel);
        joueurStats.immortel = false;
    }
    #endregion
}
