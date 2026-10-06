using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.actions["Movement"].ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
