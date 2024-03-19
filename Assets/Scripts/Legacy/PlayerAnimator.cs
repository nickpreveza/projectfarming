using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    public float walkingSpeed, runningSpeed;
    float currentSpeed;
    GameObject player;
    Transform transformPlayer;
    public Vector2 direction = new Vector2(0, 0);
    public Vector2 lastKnownDirection;
    public Animator playerAnimator;
    bool isRunning = false;
    private InputManager inputManager;
    private Vector3 mousePos;
    public Vector3 rotation; // for bullets
    Vector3 facingDirection; // 0 down, 1up, 2 left, 3 right
    private ShootingManager shootingManager;
    private Rigidbody2D rigidBody;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    NavMeshAgent playerAgent;


    public Vector3 mouseInput;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        transformPlayer = player.GetComponent<Transform>();
        inputManager = player.GetComponent<InputManager>();
        shootingManager = player.GetComponent<ShootingManager>();
    }

    void FixedUpdate()
    {
        GetFacingDirection();
        CheckRunningInput();


        if (inputManager.GetMovementInput() == new Vector2(0, 0))
        {
            direction = new Vector2(0, 0);
            playerAnimator.SetBool("IsWalking", false);
            playerAnimator.SetBool("IsRunning", false);

        }
        else if (isRunning)
        {

            currentSpeed = runningSpeed;
            direction = inputManager.GetMovementInput();

            playerAnimator.SetBool("IsRunning", true);
            playerAnimator.SetBool("IsWalking", true);
        }
        else if (isRunning == false)
        {
            currentSpeed = walkingSpeed;
            direction = inputManager.GetMovementInput();
            playerAnimator.SetBool("IsWalking", true);
        }

        if (direction != new Vector2(0, 0))
        {
            lastKnownDirection = direction;

        }
        if (shootingManager.normalGunOn == true || shootingManager.waterGunOn == true)
        {
            playerAnimator.SetBool("IsShooting", true);
            SetAnimationDirection(facingDirection);
        }
        else
        {
            playerAnimator.SetBool("IsShooting", false);
            SetAnimationDirection(lastKnownDirection);
        }


        Move();

    }

    // Get direction from input by using 0 and 1  for example:(0,1)
    void CheckRunningInput()
    {

        if (inputManager.GetRunningInput())
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

    }
    void Move()
    {
        //transformPlayer.position += new Vector3(direction.x*currentSpeed*Time.deltaTime, direction.y*currentSpeed* Time.deltaTime, 0);

        if (direction != Vector2.zero)
        {
            bool success = TryMove(direction);
            if (success == false)
            {
                success = TryMove(new Vector2(direction.x, 0));
                if (success == false)
                {
                    success = TryMove(new Vector2(0, direction.y));
                }
            }

        }

    }
    private bool TryMove(Vector2 moveDirection)
    {
        int count = rigidBody.Cast(
                moveDirection,
                movementFilter,
                castCollisions,
                currentSpeed * Time.fixedDeltaTime + collisionOffset);
        if (count == 0 || HasWallOrUnwalkableCollision(castCollisions) == false)
        {
            rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetAnimationDirection(Vector2 dir)
    {
        playerAnimator.SetFloat("PosX", dir.x);
        playerAnimator.SetFloat("PosY", dir.y);
    }

    private bool HasWallOrUnwalkableCollision(List<RaycastHit2D> hits)
    {
        foreach (RaycastHit2D hit in hits)
        {

            if (hit.collider.gameObject.CompareTag("Colliders") || hit.collider.gameObject.CompareTag("Unwalkable"))
            {
                //Debug.Log("Collision with: " + hit.collider.gameObject.name);
                return true;
            }
        }
        return false;
    }
    void GetFacingDirection()
    {
        mousePos = Camera.main.ScreenToWorldPoint(inputManager.GetMousePosition());
        Vector3 PlayerPosition = transformPlayer.position;
        mouseInput = new Vector3(mousePos.x - PlayerPosition.x, mousePos.y - PlayerPosition.y, 0);
        rotation = PlayerPosition - mousePos;


        if (mouseInput.x > 0)
        {
            if (mouseInput.y > 0.9)
            {
                facingDirection = new Vector3(0f, 1f, 0);
            }
            else if (mouseInput.y < -0.9)
            {
                facingDirection = new Vector3(0f, -1f, 0);
            }
            else
            {
                facingDirection = new Vector3(1f, 1f, 0);
            }
        }
        else if (mouseInput.x < 0)
        {
            if (mouseInput.y > 0.9)
            {
                facingDirection = new Vector3(0f, 1f, 0);
            }
            else if (mouseInput.y < -0.9)
            {
                facingDirection = new Vector3(0f, -1f, 0);
            }
            else
            {
                facingDirection = new Vector3(-1f, 0f, 0);
            }
        }
    }
}