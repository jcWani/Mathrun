using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 1.0f;

    //
    public bool isRunning = false; 

    //Animation
    private Animator anim; 

    //Movement 
    private CharacterController controller; 
    public static float jumpForce = 5.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private int desiredLane = 1;

    //Speed Modifier
    private float speed = 7.0f;
    private float originalSpeeed = 7.0f;
    private float Speed;
    private float SpeedIncreasedLastTick;
    private float SpeedIncreasedTime = 2.5f;
    private float SpeedIncreasedAmount = 0.1f;

    private void Start()
    {
        Speed = originalSpeeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        if(Time.time - SpeedIncreasedLastTick > SpeedIncreasedTime)
        {
            SpeedIncreasedLastTick = Time.time;
            Speed += SpeedIncreasedAmount;
            GameManager.Instance.UpdateModifier(Speed - originalSpeeed);
        }

        //Gather input on where the player should be 
        if (MobileInput.Instance.SwipeLeft)
            MoveLane(false);
        if (MobileInput.Instance.SwipeRight)
            MoveLane(true);

        //Calculate where should the player be 
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if(desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE; 
        else if(desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        //calculate move Delta 
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
            anim.SetBool("Grounded", isGrounded);

        //Calculate Y
        if (IsGrounded()) //if Grounded
        {
            verticalVelocity = -0.1f;


            if (MobileInput.Instance.SwipeUp)
            {
                //Jump
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if (MobileInput.Instance.SwipeDown)
            {
                //Slide
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            //Fast Fall
            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = Speed;

        //Move the Player
        controller.Move(moveVector * Time.deltaTime);

        //rotate the character to where it is going
        Vector3 dir = controller.velocity;
        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }


    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
        /*
        if (!goingRight)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane == 0;
        }
        else
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        */
    }


    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x,(controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,controller.bounds.center.z),Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);


        return (Physics.Raycast(groundRay, 0.2f + 0.1f));
    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("startRunning");
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDeath();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }


}
