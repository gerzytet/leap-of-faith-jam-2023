/*
@Authors - Craig
@Description - keeps track of collider objects
*/

using UnityEngine;

public class ToggleCollider : MonoBehaviour
{
    private int count = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        count++;
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        count--;
    }
    
    public bool IsColliding()
    {
        return count > 0;
    }
}
