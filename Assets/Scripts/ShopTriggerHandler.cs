using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTriggerHandler : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction interactAction;

    [SerializeField]
    GameObject shopUIReference;

    bool isPlayerInTrigger = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
        interactAction = playerInput.Gameplay.Interact;

    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopUIReference.SetActive(false);
            isPlayerInTrigger = false;
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && interactAction.WasPressedThisFrame())
        {
            shopUIReference.SetActive(true);
        }
    }
}
