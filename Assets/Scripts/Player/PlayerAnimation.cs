using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public PlayerController controller;

    private void OnEnable()
    {
        PlayerCombat.OnAttack += PlayAttack;
        PlayerController.OnJumpStart += PlayJump;
    }

    private void OnDisable()
    {
        PlayerCombat.OnAttack -= PlayAttack;
        PlayerController.OnJumpStart -= PlayJump;
    }

    void Update()
    {
        float speed = controller.Speed;

        animator.SetFloat("Speed", speed);

        bool isMove = speed > 0.1f;
        animator.SetBool("isMove", isMove);
    }

    void PlayAttack(int combo)
    {
        animator.SetTrigger("Attack" + combo);
    }

    void PlayJump()
    {
        animator.SetTrigger("Jump");
    }
}