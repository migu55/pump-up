using UnityEngine;

public class MeleeTriggerHandler : MonoBehaviour
{
    public float Damage = 5.0f;
    public bool HitPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!HitPlayer && collision.CompareTag("Player"))
        {
            GameManager.Instance.Energy -= Damage;
            HitPlayer = true;
        }
    }
}
