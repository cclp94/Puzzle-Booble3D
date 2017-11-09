using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour
{
    [SerializeField]
    float mBumperForce;

    [SerializeField]
    float mForceRadius;

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Ball")
        {
            col.rigidbody.AddExplosionForce(mBumperForce, transform.position, mForceRadius, 0.0f, ForceMode.Impulse);
        }
    }
}
