using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public int flagNumber;
    private void OnTriggerEnter2D(Collider2D other) {
        //Craig's code to check if collider is player
        if (other.gameObject.GetComponentInParent<Frog>() != null)
        {
            Frog.instance.reachedFlag(flagNumber);
        }
    }
}