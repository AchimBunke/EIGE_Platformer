using UnityEngine;
using System.Collections;

//RequirementComponent adds the type of component to the gameobject when the script is added to it
//This is handy to ensure required components for scripts
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    //We create two Sub-classes to make handling MoveSettings and InputSettings more practical.
    //The Serializable attribute lets you embed the class with sub properties in the inspector
    //Just like [SerializeField] when using floats but for subclasses.
    [System.Serializable]
    public class MoveSettings
    {
        public float runVelocity = 12;
        public float rotateVelocity = 100;
        public float jumpVelocity = 8;
        public float distanceToGround = 1.3f;
        //Physic simulation layer. Look at Grounded() for more
        public LayerMask ground;
    }

    [System.Serializable]
    public class InputSettings
    {
        //initialisation of strings to use variables instead of actual strings to
        //prevent typos and not having to write the strings at every use 
        public string FORWARD_AXIS = "Vertical";
        public string SIDEWAYS_AXIS = "Horizontal";
        public string TURN_AXIS = "Mouse X";
        public string JUMP_AXIS = "Jump";
    }

    //create variables of both Settings classes 
    public MoveSettings moveSettings;
    public InputSettings inputSettings;


    [SerializeField] private Transform spawnPoint;
    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation;
    private float forwardInput, sidewaysInput, turnInput, jumpInput;


    // Sets all the start values
    // Check https://docs.unity3d.com/Manual/ExecutionOrder.html to read up on Unity function execution order
    void Awake()
    {
        velocity = Vector3.zero;
        forwardInput = sidewaysInput = turnInput = jumpInput = 0;
        targetRotation = transform.rotation;

        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Called every frame
    void Update()
    {
        GetInput();
        Turn();
    }

    // Called in fixed timesteps (can be changed in the project settings)
    // FixedUpdate() is generally used for RigidBody movement because physics are only executed in fixed time steps
    void FixedUpdate()
    {
        Run();
        Jump();
        playerRigidbody.AddForce(Vector3.down * 10);
    }

    // Saves user input for later use
    void GetInput()
    {
        //check if there is a definition for string name by checking if string has length 0
        if (inputSettings.FORWARD_AXIS.Length != 0)
        {
            //if axis name was given fill input variables with Input.Get... values
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        }
        if (inputSettings.SIDEWAYS_AXIS.Length != 0)
        {
            sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
        }
        if (inputSettings.TURN_AXIS.Length != 0)
        {
            //turn input is sideways mouse movement
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
        }
        if (inputSettings.JUMP_AXIS.Length != 0)
        {
            jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
        }
    }

    void Run()
    {
        //fill velocity vector z with forward movement values
        velocity.z = forwardInput * moveSettings.runVelocity;
        //fill velocity vector x with sideways movement values
        velocity.x = sidewaysInput * moveSettings.runVelocity;
        //fill velocity vector y with current rigidBody velocity
        //in y direction because we dont alter it here.
        velocity.y = playerRigidbody.velocity.y;

        //apply velocity vector to rigidBody
        playerRigidbody.velocity = transform.TransformDirection(velocity);
    }

    void Turn()
    {
        //check if the absolute value of turn input is greater than zero
        //this covers also negative values cause axis input is -1 to 1.
        if (Mathf.Abs(turnInput) > 0)
        {
            //target rotation is the current rotation. We multiply the
            //change in form of another quaternion on to the current rotation to apply it.
            //Quaternion.AngleAxis() creates a Quaternion with a rotaion angle around a specified axis.
            //Rotation angle: rotationSpeed * mouse x input * frame scale
            //Rotation axis: up because y world axis, spins around y
            targetRotation *= Quaternion.AngleAxis(
                moveSettings.rotateVelocity * turnInput * Time.deltaTime,
                Vector3.up);
        }
        //apply rotation to player transform
        transform.rotation = targetRotation;
    }

    // used to apply jump behaviour on space bar.
    // Must set up Grounded LayerMask in the editor for Grounded() to work.
    void Jump()
    {
        //check if there is jump input and if the player
        //is grounded as in not already jumping
        if ((jumpInput != 0))
        {
            //create a new vector3, change y value to preveously
            //defined jumpVelocity, dont alter the other values,
            //apply it to the rigidbody velocity
            playerRigidbody.velocity = new Vector3(
                playerRigidbody.velocity.x,
                moveSettings.jumpVelocity,
                playerRigidbody.velocity.z);
        }
    }
    //Used to check if the player is grounded or standing on a platform marked with LayerMask ground.
    bool Grounded()
    {
        //The next line is used to visualize a raycast in the editor scene view. Uncomment to display it.
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * moveSettings.distanceToGround);
        //Raycast: start position, shoot direction, raycast ray shoot length, collision Layermask
        //LayerMask is used to only collide the raycast with objects marked with this LayerMask
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            moveSettings.distanceToGround,
            moveSettings.ground);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Spawn();
        }
        if (other.gameObject.CompareTag("Checkpont"))
        {

        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Bomb bomb = other.gameObject.GetComponent<Bomb>();
            Collider col = other.gameObject.GetComponent<Collider>();
            Collider myCol = this.gameObject.GetComponent<Collider>();
            if (bomb.invincible)
            {
                OnDeath();
            }
            else if (myCol.bounds.center.y - myCol.bounds.extents.y > col.bounds.center.y + 0.5f * col.bounds.extents.y)
            {
                GameData.Instance.Score += 9001;
                JumpedOnEnemy(bomb.bumpSpeed);
                bomb.OnDeath();
            }
            else
            {
                OnDeath();
            }
        }
    }

    void Spawn()
    {
        transform.position = spawnPoint.position;
        playerRigidbody.velocity = Vector3.zero;
    }

    void OnDeath()
    {
        Spawn();
    }

    void JumpedOnEnemy(float bumpSpeed)
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, bumpSpeed, playerRigidbody.velocity.z);
    }
}

