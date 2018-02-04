using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    [Range(1f, 15f)]
    public float smoothSpeed = 5f;

    public Vector3 offset;

    void FixedUpdate() {

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
