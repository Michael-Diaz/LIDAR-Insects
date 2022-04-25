using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_SingleMovement : MonoBehaviour {

    // State
    public Vector3 position;
    public Vector3 forward;
    Vector3 velocity;

    // To update:
    Vector3 acceleration;
    public Vector3 avgFlockHeading;
    public Vector3 avgAvoidanceHeading;
    public Vector3 centreOfFlockmates;
    public int numPerceivedFlockmates;

    // Cached
    Material material;
    Transform cachedTransform;
    Transform target;

    LayerMask obstacles;

    void Awake () {
        obstacles = LayerMask.GetMask("Barrier", "Bound");
        cachedTransform = transform;
    }

    public void Initialize (Transform target) {
        this.target = target;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        velocity = transform.forward * 3.5f;
    }

    public void UpdateBoid () {
        Vector3 acceleration = Vector3.zero;

        if (target != null) {
            Vector3 offsetToTarget = (target.position - position);
            acceleration = SteerTowards (offsetToTarget);
        }

        if (numPerceivedFlockmates != 0) {
            centreOfFlockmates /= numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

            var alignmentForce = SteerTowards (avgFlockHeading);
            var cohesionForce = SteerTowards (offsetToFlockmatesCentre);
            var seperationForce = SteerTowards (avgAvoidanceHeading);

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        if (IsHeadingForCollision ()) {
            Vector3 collisionAvoidDir = ObstacleRays ();
            Vector3 collisionAvoidForce = SteerTowards (collisionAvoidDir) * 10.0f;
            acceleration += collisionAvoidForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp (speed, 2, 5);
        velocity = dir * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = dir;
        position = cachedTransform.position;
        forward = dir;
    }

    bool IsHeadingForCollision () {
        RaycastHit hit;
        if (Physics.SphereCast (position, 0.3f, forward, out hit, 2.0f)) {
            return true;
        } else { }
        return false;
    }

    Vector3 ObstacleRays () {
        Vector3[] rayDirections = new Vector3[300];

        float goldenRatio = (1 + Mathf.Sqrt (5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < 300; i++) {
            float t = (float) i / 300;
            float inclination = Mathf.Acos (1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin (inclination) * Mathf.Cos (azimuth);
            float y = Mathf.Sin (inclination) * Mathf.Sin (azimuth);
            float z = Mathf.Cos (inclination);
            rayDirections[i] = new Vector3 (x, y, z);
        }

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = cachedTransform.TransformDirection (rayDirections[i]);
            Ray ray = new Ray (position, dir);
            if (!Physics.SphereCast (ray, 0.3f, 2.0f)) {
                return dir;
            }
        }

        return forward;
    }

    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = (vector.normalized * 3.0f) - velocity;
        return Vector3.ClampMagnitude (v, 3.0f);
    }

}