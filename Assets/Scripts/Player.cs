using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float WalkSpeed { get; set; }
    public float RunSpeed { get; set; }
    public float Stamina { get;set; }
    public float Exhaustion { get; set; }
    public float ExhaustRecovery { get; set; } 
    public bool Exhausted { get; set; }
    public float StaminaRecoveryRate { get; set; }
    public float RotateSpeed { get; set; }
    public Vector3 StartPos { get; set; }
    

    public Player(float walkSpeed, float runSpeed, float stam, float exhaustion, float exhaustRecovery, bool exhausted,
        float stamRecovRate, float rotSpeed, Vector3 startPos) : base()
    {
        WalkSpeed = walkSpeed;
        RunSpeed = runSpeed;
        Stamina = stam;
        Exhaustion = exhaustion;
        ExhaustRecovery = exhaustRecovery;
        Exhausted = exhausted;
        StaminaRecoveryRate = stamRecovRate;
        RotateSpeed = rotSpeed;
        StartPos = startPos;
    }
}
