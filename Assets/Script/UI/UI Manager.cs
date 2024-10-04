using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Event listeners")]
    public CharacterEventSO healthEvent;
    public PlayerEventSO ExpEvent;

    public Player_Heart Player_Heart;
    public Player_Exp Player_Exp;

    private void OnEnable()
    {
        healthEvent.OnEventRised += OnHealthEvent;
        ExpEvent.OnEventRaised += OnExpEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRised -= OnHealthEvent;
        ExpEvent.OnEventRaised -= OnExpEvent;
    }

    private void OnExpEvent(Player player)
    {
        var percentage = player.CurrentExp/player.MaxExp;
        Player_Exp.OnExpChange(percentage);
    }

    private void OnHealthEvent(Character character)
    {
        var leave = character.current_health;
        Player_Heart.OnHealthChange(leave);
    }
}
