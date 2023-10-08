using System;
using UnityEngine;

public class VinePoint : MonoBehaviour
{
    public HingeJoint2D grabJoint;
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
    }
}
