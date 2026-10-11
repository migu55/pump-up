using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairController : MonoBehaviour
{

    [SerializeField]
    Texture2D normalCrosshair;
    [SerializeField]
    Texture2D activeCrosshair;

    PlayerInput playerInput;
    InputAction mousePosAction;
    InputAction fireAction;

    public Vector3 worldPos;
    private Vector2 crosshairCenter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        worldPos = Vector3.zero;
        playerInput = new PlayerInput();
        mousePosAction = playerInput.Gameplay.Aim;
        fireAction = playerInput.Gameplay.Shoot;
        crosshairCenter = new Vector2(normalCrosshair.width / 2, normalCrosshair.height / 2);
    }

    private void OnEnable()
    {
        mousePosAction.Enable();
        fireAction.Enable();
    }

    private void OnDisable()
    {
        mousePosAction.Disable();
        fireAction.Disable();
    }

    public Vector3 getWorldPosOfCrosshair()
    {
        return worldPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = mousePosAction.ReadValue<Vector2>();
        //Debug.Log("MOUSE -- X:" + mousePos.x +  ", Y:" + mousePos.y);
        Vector3 absMousePos = new Vector3(mousePos.x, mousePos.y);
        worldPos = Camera.main.ScreenToWorldPoint(absMousePos);
        //Debug.Log("WORLD -- X:" + worldPos.x + ", Y:" + worldPos.y);

        Cursor.SetCursor(fireAction.IsPressed() ? activeCrosshair : normalCrosshair, crosshairCenter, CursorMode.Auto);
    }
}
