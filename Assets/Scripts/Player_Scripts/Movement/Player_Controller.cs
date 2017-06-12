using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public float speed, endSpeed, runningSpeed, walkingSpeed;
    public Animator player_Animator;
    private Rigidbody playerRigidBody;
    private Vector3 localVel;
    int playerVisionMinX = -50;
    int playerVisionMaxX = 65;
    //This value will increase or decrease the mouse sensitivity.
    //To decrease it pick a number between 0-1, to increase it pick any other positive number.
    // Picking 1 will keep the mouse on it's default sensitivity (though picking really big numbers might be bad...).
    float mouse_Sensitivity = 1f;
    //Vector 3 used to keep track of where the character is currently looking
    Vector3 euler;
    bool canMove, keyPressA, keyPressS, keyPressD, keyPressW, keyPressShift, keyPress;
    // Use this for initialization
    void Start()
    {
        speed = walkingSpeed;
        canMove = true;
        playerRigidBody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            //Screen.lockCursor = true;
            Cursor.lockState = CursorLockMode.Locked;
            localVel = transform.InverseTransformDirection(playerRigidBody.velocity);
            //localVel.z = speed;
            //localVel.x = speed;
            
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            if (!keyPress)
            {
                speed = walkingSpeed;
                
            }
            if (Input.GetKey(KeyCode.LeftShift) && (keyPressA || keyPressS || keyPressD || keyPressW))
            {
                keyPressShift = true;
                speed = runningSpeed;
                if (!player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Run") )
                {
                    player_Animator.SetTrigger("Run");
                }
            }else if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle"))
                {
                    player_Animator.SetTrigger("Idle");
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                keyPressShift = false;
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                //playerRigidBody.velocity += new Vector3 (speed * Time.deltaTime, 0, 0);
                keyPressD = true;
                localVel.x = speed;
                playerRigidBody.velocity = transform.TransformDirection(localVel);
                walkAnim();
            } if (Input.GetKeyUp(KeyCode.D))
            {
                keyPressD = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                keyPressA = true;
                localVel.x = -speed;
                //playerRigidBody.velocity += new Vector3 (-speed * Time.deltaTime, 0, 0);
                playerRigidBody.velocity = transform.TransformDirection(localVel);
                walkAnim();
            }if (Input.GetKeyUp(KeyCode.A))
            {
                keyPressA = false;
            }
            if (Input.GetKey(KeyCode.W))
            {
                keyPressW = true;
                localVel.z = speed;
                //playerRigidBody.velocity += new Vector3 (0, 0, speed * Time.deltaTime);
                playerRigidBody.velocity = transform.TransformDirection(localVel);
                walkAnim();
            } if (Input.GetKeyUp(KeyCode.W))
            {
                keyPressW = false;
            }
            if (Input.GetKey(KeyCode.S))
            {
                keyPressS = true;
                localVel.z = -speed;
                //playerRigidBody.velocity += new Vector3 (0, 0, -speed * Time.deltaTime);
                playerRigidBody.velocity = transform.TransformDirection(localVel);
                walkAnim();
            }if (Input.GetKeyUp(KeyCode.S))
            {
                keyPressS = false;
            }
            if(!keyPressA && !keyPressS && !keyPressD && !keyPressW)
            {
                keyPress = false;
                playerRigidBody.velocity = transform.TransformDirection(localVel / endSpeed);
                if (player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk"))
                {
                    player_Animator.SetTrigger("Idle");
                }
            }
            if (!keyPressShift && (keyPressA || keyPressS || keyPressD || keyPressW))
            {
                player_Animator.SetTrigger("Walk");
            }
            if (!Input.anyKey)
            {
                keyPressD = false;
                keyPressA = false;
                keyPressW = false;
                keyPressS = false;
                keyPressShift = false;
                if (!player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle"))
                {
                    player_Animator.SetTrigger("Idle");
                }
            }
                /*if(!Input.anyKey && keyPress)
                {
                    //player_Animator.SetTrigger("Idle");
                    keyPress = false;
                    playerRigidBody.velocity = transform.TransformDirection(localVel / endSpeed);
                    player_Animator.SetTrigger("Idle");
                }
                if (!Input.anyKey)
                {
                    if (player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk"))
                    {
                        player_Animator.SetTrigger("Idle");
                    }
                }*/

                localVel = new Vector3(0, 0, 0);
            Camera.main.transform.localEulerAngles = euler;
            euler.x -= mouseY * mouse_Sensitivity;

            //Camera.main.transform.Rotate(-mouseY,0f,0f);
            //Rotates the player object left and right through the use of the mouse (look left and right).
            transform.Rotate(0f, mouseX, 0f);

            //checks if the rotation angle exceeds the max and min allowed
            if (euler.x >= playerVisionMaxX)
            {
                //If so then the angle is locked between the max and min values.
                euler.x = Mathf.Clamp(euler.x, playerVisionMinX, playerVisionMaxX);
            }
            //The same is done here but for the min values.
            if (euler.x <= playerVisionMinX)
            {
                euler.x = Mathf.Clamp(euler.x, playerVisionMinX, playerVisionMaxX);
            }
        }
    }

    public void walkAnim()
    {
        if (!player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk") && speed != runningSpeed)
        {
            Debug.Log("When is this getting accessed");
            player_Animator.SetTrigger("Walk");
        }
    }
    public bool getCanMove()
    {
        return canMove;
    }
    public void setCanMove(bool activator)
    {
        canMove = activator;
    }
}