using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ennemis))]
public class PatrolControl : MonoBehaviour {

    private Rigidbody2D rb;

    public Vector2 direction;

    public LayerMask detectWhat;

    public Transform lEC; //leftEdgeCheck
    public float edgeCheckRadius;

    public Transform sightStart;
    public Transform sightEnd;


    void Start () {
        rb = this.GetComponent<Rigidbody2D>() as Rigidbody2D;

        sightStart = transform.Find("sightStart");
        sightEnd = transform.Find("sightEnd");

        lEC = transform.Find("leftEdgeCheck");

        if (direction.x > 0)
            ChangeDirection();
    }
	
    private Vector3 ChangeVectorX(Vector3 v) {
        v.x *= -1; 
        return v;
    }

	public Vector2 checkDirection() {
        if (detectColH() || !detectEdge() && rb.gravityScale != 0) {
            ChangeDirection();
            rb.velocity = new Vector2(-rb.velocity.x/2, rb.velocity.y);
                direction.x *= -1;
                
        }
        return direction;
    }

    private void ChangeDirection() {
        sightStart.localPosition = ChangeVectorX(sightStart.localPosition);
        sightEnd.localPosition = ChangeVectorX(sightEnd.localPosition);
        lEC.localPosition = ChangeVectorX(lEC.localPosition);
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
