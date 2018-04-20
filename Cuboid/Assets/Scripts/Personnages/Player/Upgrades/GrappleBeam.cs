using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GrappleBeam : MonoBehaviour
{

    public GameObject grappleHingeAnchor;
    public DistanceJoint2D grappleJoint;
    public Transform firePoint;
    public SpriteRenderer crosshairSprite;
    public PlayerCharacter2D player;

    public LineRenderer grappleRenderer;
    public LayerMask grappleLayerMask;

    public float climbSpeed = 10f;
    private bool isColliding;

    private float grappleMaxCastDistance = 50f;
    private float grappleMaxLength = 20f;
    private List<Vector2> grapplePositions = new List<Vector2>();

    private bool distanceSet;
    public bool isGrappleAttached;
    private Vector2 playerPosition;
    private Rigidbody2D grappleHingeAnchorRb;
    private SpriteRenderer grappleHingeAnchorSprite;
    private GameObject grappleUI;

    void Awake()
    {
        grappleJoint.enabled = false;
        playerPosition = transform.position;
        grappleHingeAnchorRb = grappleHingeAnchor.GetComponent<Rigidbody2D>();
        grappleHingeAnchorSprite = grappleHingeAnchor.GetComponent<SpriteRenderer>();

        if (GameObject.FindGameObjectWithTag("GrappleUI"))
        {
            grappleUI = GameObject.FindGameObjectWithTag("GrappleUI");
            grappleUI.SetActive(false);
        }

    }

    void Update()
    {
        playerPosition = player.transform.position;
        //HandleInput();
        UpdateRopePosition();
        //HandleGrappleLength();
    }

    public void UpdateGUI(bool on)
    {
        if (!grappleUI.activeSelf)
        {
            grappleUI.SetActive(true);
        }

        if (on)
        {
            grappleUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 1);
        }
        else
        {
            grappleUI.GetComponent<Image>().color = new Vector4(1, 1, 0, 0.392f);
        }
    }

    public void UseGrapple()
    {
        if (!isGrappleAttached)
        {
            //Empeche d'utiliser le grappin si on est deja attaché
            if (isGrappleAttached)
            {
                return;
            }

            grappleRenderer.enabled = true;

            //TODO: set le raycast selon le fire point
            var hit = Physics2D.Raycast(player.transform.position, player.transform.up, grappleMaxCastDistance, grappleLayerMask);

            if (hit.collider != null)
            {
                //Debug.Log("Raycast touche terrain");
                FindObjectOfType<AudioManager>().Play("GrappleSound");

                isGrappleAttached = true;

                transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                grapplePositions.Add(hit.point);
                grappleJoint.distance = Vector2.Distance(playerPosition, hit.point);
                grappleJoint.enabled = true;
                grappleHingeAnchorSprite.enabled = true;
            }
            else
            {
                grappleRenderer.enabled = false;
                isGrappleAttached = false;
                grappleJoint.enabled = false;
            }
        }
        else
        {
            grappleJoint.enabled = false;
            isGrappleAttached = false;
            //player.isSwinging = false;
            grappleRenderer.positionCount = 2;
            grappleRenderer.SetPosition(0, transform.position);
            grappleRenderer.SetPosition(1, transform.position);
            grapplePositions.Clear();
            grappleHingeAnchorSprite.enabled = false;
        }
    }

    public Vector2 GetSwingDirection(Vector2 dir)
    {
        if (isGrappleAttached)
        {
            Vector2 grappleHookPos = new Vector2();
            var playerToHookDir = (grappleHookPos - (Vector2)transform.position).normalized;
            Vector2 perpDirection = new Vector2();

            if (dir.x < 0)
            {
                perpDirection = new Vector2(-playerToHookDir.y, playerToHookDir.x);
                var leftPerpPos = (Vector2)transform.position - perpDirection * -2f;
                Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
            }
            else if (dir.x > 0)
            {
                perpDirection = new Vector2(-playerToHookDir.y, -playerToHookDir.x);
                var rightPerpPos = (Vector2)transform.position + perpDirection * 2f;
                Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
            }

            return new Vector2(perpDirection.x, perpDirection.y);
        }

        return new Vector2(1,1);
    }

    public void HandleGrappleLength(float direction)
    {
        bool underCeiling = player.IsUnderCeiling();
        bool grounded = player.IsGrounded();

        if ((direction > 0) && isGrappleAttached && !underCeiling)
        {
            grappleJoint.distance -= Time.deltaTime * climbSpeed;
        }
        else if ((direction < 0) && isGrappleAttached && !grounded)
        {
            grappleJoint.distance += Time.deltaTime * climbSpeed;
            if (grappleJoint.distance > grappleMaxLength)
            {
                grappleJoint.distance = grappleMaxLength;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collider)
    {
        isColliding = true;
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        isColliding = false;
    }

    private void UpdateRopePosition()
    {
        if (!isGrappleAttached)
        {
            return;
        }

        grappleRenderer.positionCount = grapplePositions.Count + 1;

        for (var i = grappleRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != grappleRenderer.positionCount - 1)
            {
                grappleRenderer.SetPosition(i, (grapplePositions[i] -(Vector2)transform.position) / transform.localScale.x);

                if (i == grapplePositions.Count - 1 || grapplePositions.Count == 1)
                {
                    //Pas sur de comprendre ce if si les 2 cas font la même chose ?...
                    var grapplePosition = grapplePositions[grapplePositions.Count - 1];
                    if (grapplePositions.Count == 1)
                    {
                        grappleHingeAnchorRb.transform.position = grapplePosition;
                        if (!distanceSet)
                        {
                            grappleJoint.distance = Vector2.Distance(transform.position, grapplePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        grappleHingeAnchorRb.transform.position = grapplePosition;
                        if (!distanceSet)
                        {
                            grappleJoint.distance = Vector2.Distance(transform.position, grapplePosition);
                        }
                    }
                }
                else if (i - 1 == grapplePositions.IndexOf(grapplePositions.Last()))
                {
                    var grapplePosition = grapplePositions.Last();
                    grappleHingeAnchorRb.transform.position = grapplePosition;
                    if (!distanceSet)
                    {
                        grappleJoint.distance = Vector2.Distance(transform.position, grapplePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                grappleRenderer.SetPosition(i, Vector3.zero);
            }
        }
    }
}
