using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrappleBeam : MonoBehaviour {

    public GameObject grappleHingeAnchor;
    public DistanceJoint2D grappleJoint;
    public Transform firePoint;
    public SpriteRenderer crosshairSprite;
    public PlayerCharacter2D player;

    public LineRenderer grappleRenderer;
    public LayerMask grappleLayerMask;

    private float grappleMaxCastDistance = 50f;
    private List<Vector2> grapplePositions = new List<Vector2>();

    private bool distanceSet;
    private bool isGrappleAttached;
    private Vector2 playerPosition;
    private Rigidbody2D grappleHingeAnchorRb;
    private SpriteRenderer grappleHingeAnchorSprite;

    void Awake()
    {
        grappleJoint.enabled = false;
        playerPosition = transform.position;
        grappleHingeAnchorRb = grappleHingeAnchor.GetComponent<Rigidbody2D>();
        grappleHingeAnchorSprite = grappleHingeAnchor.GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
        playerPosition = player.transform.position;
        HandleInput();
        UpdateRopePosition();
	}

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Tir le grappin");

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
                Debug.Log("Raycast touche terrain");

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

        if (Input.GetMouseButton(1))
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
                grappleRenderer.SetPosition(i, grapplePositions[i]);

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
                grappleRenderer.SetPosition(i, transform.position);
            }
        }
    }
}
