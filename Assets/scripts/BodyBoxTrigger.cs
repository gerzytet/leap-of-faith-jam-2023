/*
@Authors - Craig and Patrick
@Description - This is where all collision code for tilemap layers goes
*/

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
        else if (collider.gameObject.layer == LayerMask.NameToLayer("spikes"))
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
