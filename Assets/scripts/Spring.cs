using System;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("enter");
        if (collider.gameObject.GetComponentInParent<Frog>() != null)
        {
            Debug.Log("springing");
            collider.gameObject.GetComponentInParent<Frog>().StartJump(new Vector2(0, 600), Frog.JumpType.SPRING);
        }
    }
}
