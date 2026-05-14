using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputActions input;
    private Camera mainCam;

    [Header("Raycast")]
    public LayerMask groundLayer;

    public static event Action<Vector3> OnClickMove;
    public static event Action OnJump;
    public static event Action OnAttack;

    private void Awake()
    {
        input = new PlayerInputActions();
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += OnMovePerformed;   // 이제 Move = 클릭
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Attack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMovePerformed;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Attack.performed -= OnAttackPerformed;

        input.Disable();
    }

    // -------------------------
    // Mouse Click → Ground만 허용
    // -------------------------
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = mainCam.ScreenPointToRay(mousePos);

        // 👇 핵심: GroundLayer만 체크
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            OnClickMove?.Invoke(hit.point);
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke();
    }
}