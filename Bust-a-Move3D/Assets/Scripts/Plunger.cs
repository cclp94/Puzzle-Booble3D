using UnityEngine;
using System.Collections;

public class Plunger : MonoBehaviour
{
    [SerializeField]
    float mForce;

    void OnTriggerStay(Collider col)
    {
        if(col.tag == "Ball" && Input.GetButtonDown ("Launch"))
        {
            col.attachedRigidbody.AddForce(mForce * Vector3.forward, ForceMode.Impulse);
        }
    }
}
