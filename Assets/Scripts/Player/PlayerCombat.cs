using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    public static event Action<int> OnAttack;

    private int comboIndex = 0;
    private float lastAttackTime = 0f;

    [Header("Combo")]
    public float comboResetTime = 1.0f;
    public int maxCombo = 3;

    private void OnEnable()
    {
        PlayerInputHandler.OnAttack += HandleAttack;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnAttack -= HandleAttack;
    }

    void HandleAttack()
    {
        float currentTime = Time.time;

        if (currentTime - lastAttackTime > comboResetTime)
        {
            comboIndex = 0;
        }

        comboIndex++;

        if (comboIndex > maxCombo)
            comboIndex = 1;

        lastAttackTime = currentTime;

        OnAttack?.Invoke(comboIndex);
    }
}