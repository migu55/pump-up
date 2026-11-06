using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Energy = 100.0f;
    public float MaxEnergy = 100.0f;
    public int Money = 0;

    // The public static property that other scripts access
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Enforce the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Clear out the duplicate
            return;
        }

        Instance = this;

        // Optional: Keep the singleton alive across scene changes
        DontDestroyOnLoad(gameObject);
    }

    public void AddEnergy(float energy)
    {
        Energy = Mathf.Clamp(Energy + energy, 0, MaxEnergy);
    }

    // Update is called once per frame
    void Update()
    {
        Energy = Mathf.Clamp(Energy - Time.deltaTime * 0.75f, 0, MaxEnergy); // Decrease energy over time
    }
}
