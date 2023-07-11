using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float rotateSpeed = 95f;
    [SerializeField] float maxStamina = 7.0f;
    public float stamina;
    public float exhaustCutoff = 2.5f;
    public bool exhausted = false;
    [SerializeField] float staminaRecoveryRate = 2;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isRunning;
    [SerializeField] bool stoppedRunning;
    [SerializeField] bool hasStoppedRunning;
    public float exhaustRecovCutoff = 5.0f;
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
        playerAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();  
        startPos = transform.position;
        startRot = transform.rotation;
        stamina = maxStamina;
    }

    private void Update()
    {
        vertInput = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        camInput = Input.GetAxis("Horizontal");

        stoppedRunning = Input.GetKeyUp(KeyCode.LeftShift);

        if (stoppedRunning && stamina < exhaustCutoff)
            hasStoppedRunning = true;

        //if(Input.GetKey(KeyCode.Q))
        //{
        //    isAttacking = true;
        //}
        //else
        //{
        //    isAttacking = false;
        //}

        RangeCheck();
    }

    void FixedUpdate()
    {
        if (hasStoppedRunning && stamina < exhaustCutoff)
            exhausted = true;

        if (isRunning && stamina > 0 && !exhausted)  //running
        {
            MovementActions(vertInput, 2.5f, runSpeed);
        }
        else if(Mathf.Abs(vertInput) > 0)   //walking
        {
            isRunning = false;
            MovementActions(vertInput, 1.5f, walkSpeed);
        }
        else
        {
            MovementActions(vertInput, 0.5f, 0);
        }



       // playerAnim.SetBool("attacking_b", isAttacking);

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
            stamina = stamina + (animSpeed > 1.5f ? -Time.deltaTime : (Time.deltaTime * staminaRecoveryRate));
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
        transform.Rotate(Vector3.up * Time.deltaTime * horInput * rotateSpeed);
    }

    private void StaminaCheck()
    {
        if (stamina < 0)
        {
            stamina = 0;
            exhausted = true;
        }
        else if (stamina >= exhaustRecovCutoff)
        {
            exhausted = false;
            hasStoppedRunning = false;
            if (stamina > maxStamina)
                stamina = maxStamina;
        }        
    }
    
    private void IsAttacking()
    {
        isAttacking = !isAttacking;
    }

    public float GetStaminaRate()
    {
        return stamina / maxStamina;
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
        stamina = maxStamina;
        SetAnimSpeedf(0, 0);
        PlayerMovement(0, 0);
        StaminaCheck();
    }
}
