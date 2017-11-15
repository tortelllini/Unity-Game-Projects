using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //added in bishop, need to transfer
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    //for UI, change in main save 
    public Text countText;
    public Text winText;
    public Text restartText;
    public Text deathText;
    public int winCount;
    public int pickUpCount;
    //end

    public bool doubleJump;
    public bool canJump;
    public Transform moveEffect;

    Animator animator;
    Transform cameraT;
    CharacterController controller;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        //change in main file
        winCount = 0;
        pickUpCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        SetCountText();
        winText.text = "";
        restartText.text = "";
        deathText.text = "";
        doubleJump = false;
        //end

        //animator.SetBool("isJumping", false);
    }

    // Update is called once per frame
    void Update() {

        //input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);

        Move(inputDir, running);

        if (controller.isGrounded) { canJump = true; }

        if (Input.GetKeyDown(KeyCode.Space)) {            
            Jump();
        }

        //Done in Bishop, transfer over to main desktop
        if (Input.GetKeyDown(KeyCode.R)) {
            Restart();
        }
        //end of transfer

        //animator
        //to not have character stop when running into walls
        //float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void Move(Vector2 inputDir, bool running) {
        if (inputDir != Vector2.zero) {
            //Transform effect = Instantiate(moveEffect, transform.position, transform.rotation) as Transform;
            //Destroy(effect.gameObject, 1);

            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        //if character is hitting a wall
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded) {
            velocityY = 0;
            animator.SetBool("isJumping", false);
            //animator.SetBool("isFalling", false);
        }

        
    }

    void Jump() {
        float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);

        if (controller.isGrounded || canJump) { //controller.isGrounded
            canJump = false;
 
            velocityY = jumpVelocity;
            doubleJump = true;
            animator.SetBool("isJumping", true);
            //animator.SetBool("isFalling", true);
        }
        else if (doubleJump) {
            doubleJump = false;

            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime) {
        if (controller.isGrounded) {
            return smoothTime;
        }

        if (airControlPercent == 0) { return float.MaxValue; }

        return smoothTime / airControlPercent;
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetCountText() {
        countText.text = "Count: " + winCount.ToString();
        if (winCount >= pickUpCount) { 
            winText.text = "You Win!";
            restartText.text = "Press R to restart.";
        }
    }
    //end of change

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pick Up")) {           
            SetCountText();
        }

        if (other.gameObject.CompareTag("Death")) {
            gameObject.SetActive(false);
            for (int i = 0; i < 49; i++) {
                deathText.text = "You have died. Game will restart shortly.";
                Restart();
            }
        }
    }

}
