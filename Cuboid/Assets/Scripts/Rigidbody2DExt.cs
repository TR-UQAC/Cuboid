using UnityEngine;

public static class Rigidbody2DExt {

    public static bool AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Impulse) {

        var dir = (rb.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        if (wearoff <= 0)
            return false;
        Vector3 baseForce = dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff;
        baseForce.z = 0;
        rb.AddForce(baseForce);

        if (upwardsModifier != 0) {
            float upliftWearoff = 1 - upwardsModifier / explosionRadius;
            Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
            upliftForce.z = 0;
            rb.AddForce(upliftForce);
        }
        return true;
    }
}