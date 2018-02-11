using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ennemis))]
public class PatrolControl : MonoBehaviour {

    private Rigidbody2D rb;

    public Vector2 direction;

    public LayerMask detectWhat;

    public float wallCheckRadius;
    public float edgeCheckRadius;

    //TODO: Une fois les balle réglé enlever les vérification de droite et juste multiplier le scale.x par -1
    private Transform rWC; //rightWallCheck
    private Transform lWC; //leftWallCheck

    private Transform rEC; //rightEdgeCheck
    private Transform lEC; //leftEdgeCheck

    void Start () {
        rb = this.GetComponent<Rigidbody2D>();

        rWC = transform.Find("rightWallCheck");
        lWC = transform.Find("leftWallCheck");

        rEC = transform.Find("rightEdgeCheck");
        lEC = transform.Find("leftEdgeCheck");
    }
	
	public Vector2 checkDirection() {
            if (detectColH() || !detectEdge() && rb.gravityScale != 0) {
                transform.localScale = new Vector2(transform.localScale.x*-1, 1);
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            direction.x *= -1;
        }

        return direction;
    }

    bool detectColH() {
        return //Physics2D.OverlapCircle(rWC.position, wallCheckRadius, detectWhat) ||
                Physics2D.OverlapCircle(lWC.position, wallCheckRadius, detectWhat);
    }

    bool detectEdge() {
        
        return //!Physics2D.OverlapCircle(rEC.position, wallCheckRadius, detectWhat) ||
                Physics2D.OverlapCircle(lEC.position, wallCheckRadius, detectWhat);
                
    }

}
