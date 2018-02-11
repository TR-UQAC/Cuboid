using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ennemis))]
public class PatrolControl : MonoBehaviour {

    private Rigidbody2D rb;

    public Vector2 direction;

    public LayerMask detectWhat;
    public float edgeCheckRadius;

    public Transform sightStart;
    public Transform sightEnd;

    public Transform lEC; //leftEdgeCheck

    void Start () {
        rb = this.GetComponent<Rigidbody2D>();

        sightStart = transform.Find("sightStart");
        sightEnd = transform.Find("sightEnd");

        lEC = transform.Find("leftEdgeCheck");
    }
	
	public Vector2 checkDirection() {
            if (detectColH() || !detectEdge() && rb.gravityScale != 0) {
                transform.localScale = new Vector2(transform.localScale.x*-1, 1);
                rb.velocity = new Vector2(-rb.velocity.x/10, rb.velocity.y);
            direction.x *= -1;
        }
        return direction;
    }

    bool detectColH() {
        return Physics2D.Linecast(sightStart.position, sightEnd.position, detectWhat);
    }

    bool detectEdge() {
        return Physics2D.OverlapCircle(lEC.position, edgeCheckRadius, detectWhat); 
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(sightStart.position, sightEnd.position);
        Gizmos.DrawSphere(lEC.position, edgeCheckRadius);
    }

}
