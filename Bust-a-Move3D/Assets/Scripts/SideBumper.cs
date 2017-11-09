using UnityEngine;
using System.Collections;

public class SideBumper : MonoBehaviour
{
    [SerializeField]
    float mBumperForce;

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Ball")
        {
            col.rigidbody.AddForce(mBumperForce * transform.forward, ForceMode.Impulse);
        }
    }
}
