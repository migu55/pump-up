using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerActions;
    private InputAction movement, focusMode, parry;

    [SerializeField]
    private GameObject parryCollider;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerActions = new PlayerInput();
        movement = playerActions.Gameplay.Movement;
        focusMode = playerActions.Gameplay.Focus;
        parry = playerActions.Gameplay.Parry;
        parry.performed += Parry;
    }

    private void OnEnable()
    {
        movement.Enable();
        focusMode.Enable();
        parry.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        focusMode.Disable();
        parry.Disable();
    }

    private void Parry(InputAction.CallbackContext obj)
    {
        parryCollider.SetActive(true);
        new WaitForSeconds(0.25f);
        parryCollider.SetActive(false);
    }

    void FixedUpdate()
    {
        Vector2 moveVector = movement.ReadValue<Vector2>();
        rb.linearVelocity = moveVector * (focusMode.IsPressed() ? 2.5f : 7.5f);
    }
}
