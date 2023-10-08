/*
@Authors - Craig and Patrick
@Description - This is where all collision code for tilemap layers goes
*/

using System;
using UnityEngine;

public class BodyBoxTrigger : MonoBehaviour
{
    public int floorContacts = 0;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            floorContacts++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collding " + (1 << collision.gameObject.layer) + " " + LayerMask.NameToLayer("spikes"));
        if (collision.gameObject.layer == LayerMask.NameToLayer("spikes"))
        {
            Frog.instance.Die();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            floorContacts--;
        }
    }
}
