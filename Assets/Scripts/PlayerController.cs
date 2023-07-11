using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player pl;
    public float stamina;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isRunning;
    [SerializeField] bool stoppedRunning;
    [SerializeField] bool hasStoppedRunning;
    public Vector3 startPos;
    public Quaternion startRot;
    [SerializeField] GameManager gameManager;
    private Animator playerAnim;
    private float vertInput;
    private float camInput;

    private float xRange = 16;
    private float zRange = -10;

    public int lives = 3;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        pl = new Player(5, 10, 7, 2.5f, 5, false, 2, 95, transform.position);
        playerAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();  
        startPos = pl.StartPos;
        startRot = transform.rotation;
        stamina = pl.Stamina;
    }

    private void Update()
    {
        vertInput = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        camInput = Input.GetAxis("Horizontal");

        stoppedRunning = Input.GetKeyUp(KeyCode.LeftShift);

        if (stoppedRunning && stamina < pl.Exhaustion)
            hasStoppedRunning = true;

        RangeCheck();
    }

    void FixedUpdate()
    {
        if (hasStoppedRunning && stamina < pl.Exhaustion)
            pl.Exhausted = true;

        if (isRunning && stamina > 0 && !pl.Exhausted)  //running
        {
            MovementActions(vertInput, 2.5f, pl.RunSpeed);
        }
        else if(Mathf.Abs(vertInput) > 0)   //walking
        {
            isRunning = false;
            MovementActions(vertInput, 1.5f, pl.WalkSpeed) ;
        }
        else
        {
            MovementActions(vertInput, 0.5f, 0);
        }
    }

    private void LateUpdate()
    {
        CameraMovement(camInput);
    }

    private void MovementActions(float verInput, float animSpeed, float playerSpeed)
    {
        if(gameManager.gameStarted)
        {
            SetAnimSpeedf(verInput, animSpeed);
            PlayerMovement(verInput, playerSpeed);
            StaminaCheck();
            stamina = stamina + (animSpeed > 1.5f ? -Time.deltaTime : (Time.deltaTime * pl.StaminaRecoveryRate));
        }
        else
        {
            ResetPlayer();
        }
    }

    private void SetAnimSpeedf(float verInput, float speedf)
    {
        //checks which direction(forward or backward)
        if (verInput >= 0)
        {
            playerAnim.SetFloat("speed_f", speedf);
        }
        else if(verInput < 0)
        {
            playerAnim.SetFloat("speed_f", -speedf);
        }
    }

    private void PlayerMovement(float verInput, float speed)
    {
        Vector3 movement = transform.forward * verInput * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    private void CameraMovement(float horInput)
    {
        transform.Rotate(Vector3.up * Time.deltaTime * horInput * pl.RotateSpeed);
    }

    private void StaminaCheck()
    {
        if (stamina < 0)
        {
            stamina = 0;
            pl.Exhausted= true;
        }
        else if (stamina >= pl.ExhaustRecovery)
        {
            pl.Exhausted = false;
            hasStoppedRunning = false;
            if (stamina > pl.Stamina)
                stamina = pl.Stamina;
        }        
    }
    
    private void IsAttacking()
    {
        isAttacking = !isAttacking;
    }

    public float GetStaminaRate()
    {
        return stamina / pl.Stamina;
    }


    private void RangeCheck()
    {
        if (Mathf.Abs(transform.position.x) > xRange)
        {
            transform.position = new Vector3(transform.position.x > 0 ? xRange : -xRange, transform.position.y, transform.position.z);
        }

        if(transform.position.z < -10)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
    }

    private void ResetPlayer()
    {
        hasStoppedRunning = true;
        stamina = pl.Stamina;
        SetAnimSpeedf(0, 0);
        PlayerMovement(0, 0);
        StaminaCheck();
    }
}
