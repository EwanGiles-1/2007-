using UnityEngine;

namespace Assets.Scripts.Actor
{
    public class ActorLocomotion : MonoBehaviour
    {

        InputManager inputManager;

        public Vector3 moveDirection;
        public Transform cameraObject;
        Rigidbody playerRB;

        [Header("Movement")]
        private float moveSpeed;
        public float walkSpeed = 5;
        public float runSpeed = 10;
        public float rotateSpeed = 20;
        public float groundDrag;


        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask collisionLayer;
        public bool grounded;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            playerRB = GetComponent<Rigidbody>();
            moveSpeed = walkSpeed;
        }

        private void Update()
        {
            grounded = Physics.CheckSphere(transform.position + new Vector3(0,0.1f,0), 0.15f, collisionLayer);
        }

        public void HandleAllMovement()
        {
            HandleMovement();
            HandleDrag();
            HandleRotation();
        }

        private void HandleDrag()
        {
            if (grounded)
                playerRB.drag = groundDrag;
            else
                playerRB.drag = 0;
        }

        private void HandleMovement()
        {
            moveDirection = cameraObject.forward.normalized * inputManager.movement.y;
            moveDirection = moveDirection + cameraObject.right.normalized * inputManager.movement.x;
            //moveDirection.Normalize();
            moveDirection.y = 0;
            Vector3 moveVelocity = Vector3.zero;
            moveVelocity = inputManager.moveAmount >= 0.5f ? moveDirection * runSpeed : //If moveAmount is less than 0.5f use walk speed
                                                             moveDirection * walkSpeed; //Else use run speed

            playerRB.velocity = moveVelocity;
            //playerRB.AddForce(moveVelocity * 10, ForceMode.Force);
            //SpeedControl();
        }
        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);

            //limit Velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                playerRB.velocity = new Vector3(limitedVel.x, playerRB.velocity.y, limitedVel.z);
            }
        }

        private void HandleRotation()
        {
            Vector3 targetDirection = Vector3.zero;
            targetDirection = cameraObject.forward * inputManager.movement.y;
            targetDirection = targetDirection + cameraObject.right * inputManager.movement.x;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            transform.rotation = playerRotation;
        }
    }
}
