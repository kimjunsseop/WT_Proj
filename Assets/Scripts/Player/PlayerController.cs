using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float stopDistance = 0.1f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 5f;
    public bool isGrounded;

    private Vector3 targetPosition;
    private bool hasTarget = false;
    private bool isJump = false;
    private bool isAttacking = false; // 🔥 공격 상태

    public float Speed => new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
    public bool IsJumping => isJump;

    public static event Action OnJumpStart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnClickMove += SetTarget;
        PlayerInputHandler.OnJump += HandleJump;
        PlayerInputHandler.OnAttack += HandleAttackStart; // 🔥 공격 이벤트
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnClickMove -= SetTarget;
        PlayerInputHandler.OnJump -= HandleJump;
        PlayerInputHandler.OnAttack -= HandleAttackStart;
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    void SetTarget(Vector3 pos)
    {
        if (isAttacking) return; // 🔥 공격 중 이동 금지

        targetPosition = pos;
        hasTarget = true;
    }

    void MoveToTarget()
    {
        if (!hasTarget || isJump || isAttacking) return;

        Vector3 dir = targetPosition - transform.position;
        dir.y = 0;

        float distance = dir.magnitude;

        // 도착 처리
        if (distance < stopDistance)
        {
            hasTarget = false;
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        dir.Normalize();

        // 회전
        Quaternion targetRot = Quaternion.LookRotation(dir);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);

        // 이동
        Vector3 velocity = dir * moveSpeed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    // 🔥 공격 시작 → 이동 즉시 중단
    void HandleAttackStart()
    {
        isAttacking = true;

        hasTarget = false;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    // 🔥 공격 종료 → 이동 다시 가능 애니메이션 event로 추가
    public void EndAttack()
    {
        isAttacking = false;
    }

    void HandleJump()
    {
        if (!isGrounded || isAttacking) return;

        isJump = true;
        hasTarget = false;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
        OnJumpStart?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            isJump = false;
        }
    }
}