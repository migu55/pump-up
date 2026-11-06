using UnityEngine;

public class PickupBehavior : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            GameManager.Instance.Money += collision.gameObject.GetComponent<Coin>().Value;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Energy")
        {
            ;
            GameManager.Instance.AddEnergy(collision.gameObject.GetComponent<EnergyOrb>().Amount);
            Destroy(collision.gameObject);
        }
    }
}
