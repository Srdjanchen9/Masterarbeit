using UnityEngine;

public class MOTBallPhysics : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 1f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.useGravity = false;
        rb.isKinematic = true; // Startzustand: ruhend
    }

    public void Launch(Vector3 dir, float spd)
    {
        direction = dir.normalized;
        speed = spd;

        rb.isKinematic = false; // ❗ Bewegung wieder aktivieren
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        direction = Vector3.Reflect(direction, normal).normalized;
        rb.velocity = direction * speed;
    }

    public void StopBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // Bewegung einfrieren
    }
}