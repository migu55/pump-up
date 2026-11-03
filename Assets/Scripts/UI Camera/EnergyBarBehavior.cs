using UnityEngine;
using UnityEngine.UI;

public class EnergyBarBehavior : MonoBehaviour
{
    [SerializeField]
    Image energyBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        energyBar.fillAmount = GameManager.Instance.Energy / GameManager.Instance.MaxEnergy;
    }
}
