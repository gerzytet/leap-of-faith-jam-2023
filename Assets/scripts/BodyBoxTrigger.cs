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
    
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            floorContacts--;
        }
    }
}
