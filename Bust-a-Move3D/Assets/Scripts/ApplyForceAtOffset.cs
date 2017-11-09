using UnityEngine;
using System.Collections;

public class ApplyForceAtOffset : MonoBehaviour
{
    [SerializeField]
    float mForce;

    [SerializeField]
    Vector3 mDirection = Vector3.forward;

    [SerializeField]
    string mButtonName;

    [SerializeField]
    Vector3 mOffset;

    Rigidbody mRigidbody;

    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        float force = Input.GetButton(mButtonName) ? mForce : -mForce;
        mRigidbody.AddForceAtPosition(mDirection.normalized * force, transform.position + mOffset);
    }
}
