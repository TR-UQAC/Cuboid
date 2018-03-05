using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBeam : MonoBehaviour {

    public GameObject grappleHingeAnchor;
    public DistanceJoint2D grappleJoint;
    public Transform firePoint;
    public SpriteRenderer crosshairSprite;
    public PlayerCharacter2D player;

    public LineRenderer grappleRenderer;
    public LayerMask grappleLayerMask;

    private float grappleMaxCastDistance = 20f;
    private List<Vector2> grapplePositions = new List<Vector2>();

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
        HandleInput();
	}

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Tir le grappin");

            //Empeche d'utiliser le grappin si on est deja attaché
            if (isGrappleAttached)
            {
                return;
            }

            grappleRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, Vector2.right, grappleMaxCastDistance, grappleLayerMask);

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
}
