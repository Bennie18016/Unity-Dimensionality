using Photon.Pun;
using UnityEngine;

// Created namespace called "PlayerController"
namespace PlayerController
{
    public class Movement : MonoBehaviour
    {
        // All of the values and variables needed

        public float speed = 5;
        public float sprint = 8;
        public Camera pc;
        public Camera minimap;
        float jumpHeight = 8;
        float sensitivity = 2;
        public float gravity = 20;
        float xlimit = 75;
        float rotationX = 0;
        public CharacterController cc;
        public Vector3 moveDirection;
        PhotonView PV;
        public bool isSwiming;
        public float swimSpeed;
        public float downswimSpeed;

        public float upswimSpeed;
        public Transform target;

        private void Start()
        {
            // Gets the component "PhotonView" and "CharacterController" and assigns the variables to them
            PV = GetComponent<PhotonView>();
            cc = GetComponent<CharacterController>();

            target = gameObject.transform;
            swimSpeed = 5;
            downswimSpeed = -   7;
            upswimSpeed = 3;

            // if(PV.IsMine) is true if the PhotonView component is yours and can be controlled by the client
            if (PV.IsMine)
            {
                // Locks your cursor to the middle of the screen and also makes it invisible
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // If PV isnt yours
            if (!PV.IsMine)
            {
                // Disables the other peoples cameras to avoid confliction. This will only disable for you
                pc.enabled = false;
                minimap.enabled = false;
                pc.GetComponent<AudioListener>().enabled = false;
            }
        }

        private void Update()
        {
            if (PV.IsMine)
            {
                rotationX += -Input.GetAxis("Mouse Y") * sensitivity;
                rotationX = Mathf.Clamp(rotationX, -xlimit, xlimit);
                pc.transform.localRotation = Quaternion.Euler(rotationX, 180, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);

                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);

                bool running = Input.GetKey(KeyCode.LeftShift);

                float curSpeedX = (running ? sprint : speed) * Input.GetAxis("Vertical");
                float curSpeedY = (running ? sprint : speed) * Input.GetAxis("Horizontal");

                float movementDirectionY = moveDirection.y;

                moveDirection = (-forward * curSpeedX) + (-right * curSpeedY);

                if (!isSwiming)
                {


                    /*Sets the forward and right vector3 quantitys relative to the GameObject
                    Running is true when you press/hold LeftShift

                    Sets your X and Y speed. Checks if you are running, if you are the speed is
                    "sprint", if you aren't running, your speed is "speed"
                    Multiply it by input manager Vertical (w and S) and Horizontal (a and d)

                    Sets the float "moveDirectionY" to moveDirection.y (used for jumping)

                    moveDirection is the first part to what moves you.
                    It sets the X and the Z value of moveDirection depending on the key you press and the speed you're going
                    this is because we set the Vector3 Forward (z) and Right (x) values before

                    If you press space and your character is on the ground,
                    moveDirection.y is set to jumpHeight
                    If you dont press space or your not grounded, it will constantly
                    be setting moveDirection.y to the float earlier
                    movementDirectionY. This is so that if you go up a slope,
                    it will know and change your y level accordingly.

                    If your character is not on the ground,
                    it will slowly bring you back down by taking moveDirection.y away from gravity * Time.deltaTime.
                    Time.deltaTime is the time between the last and current frame. This keeps it so it is the same no matter
                    how many frames you are on.

                    This will actually move you using your character controller. It will take all the data from above and move you
                    based on the X,Y and Z values of moveDirection. It also multiplies it by Time.deltaTime


                    rotationX adds itself to the Y axis of your mouse and times it my sensitivity.
                    It is -Input.GetAxis("Mouse Y") so that it is the x value instead of the y value
                    We then clamp how much we can look up and down by using rotaionX, -xlimit and xlimit.
                    This stops our cameras from doing 360's
                    We then rotate the camera by rotation x and 180.
                    Finally we move the player with it by getting the x axis and multiply it by sensitivty*/

                    if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
                    {
                        moveDirection.y = jumpHeight;
                    }
                    else
                    {
                        moveDirection.y = movementDirectionY;
                    }

                    if (!cc.isGrounded)
                    {
                        moveDirection.y -= gravity * Time.deltaTime;
                    }

                    
                }
                else
                {
                    Physics.IgnoreLayerCollision(0, 4, true);

                    if (Input.GetKey(KeyCode.Space)) // Swimp up!
                    {
                        moveDirection.y = upswimSpeed;
                    }
                    if (Input.GetKeyUp(KeyCode.Space)) // Reset when letting go of button. 
                    {
                        moveDirection.y = 1f; // more than zero means it'll keep momentum up
                    }
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        moveDirection.y = downswimSpeed;
                    }
                    if (Input.GetKeyUp(KeyCode.LeftShift)) // Reset when letting go of button. 
                    {
                        moveDirection.y = -0.5f; // more than zero means it'll keep momentum up
                    }
                }

                cc.Move(moveDirection * Time.deltaTime);
            }
        }
    }
}

