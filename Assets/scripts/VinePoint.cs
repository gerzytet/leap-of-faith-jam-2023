/*
@Authors - Craig
@Description - Controls vine physics
*/

using System;
using UnityEngine;

public class VinePoint : MonoBehaviour
{
    public HingeJoint2D grabJoint;
    public bool hasFrog;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponentInParent<Frog>() != null)
        {
            col.gameObject.GetComponentInParent<Frog>().GrabVine(this);
        }
    }

    void Start()
    {
        grabJoint.enabled = false;
        hasFrog = false;
    }

    void Update()
    {
        if (!hasFrog)
        {
            GetComponent<Rigidbody2D>().angularVelocity *= 0.77f;
        }
    }
}
