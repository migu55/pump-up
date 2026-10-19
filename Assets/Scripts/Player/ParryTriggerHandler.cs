using UnityEngine;

public class ParryTriggerHandler : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("IMPLEMENT LATER = Parry Triggered with: " + collision.gameObject.name);
        // Reverse direction of enemy attack and swap tag to friendly projectile
    }
}
