using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceCheck : MonoBehaviour {

    /// <summary>
    /// Definition for direction player is facing
    /// </summary>
    public enum Direction
    {
        LEFT = -1,
        RIGHT = 1,
        DOWN = 2
    };

    /// <summary>
    /// Holds information about the current frame to drive movement and animations
    /// </summary>
    public struct State
    {
        public Direction direction;
        public Direction oppDirection;

        public GameObject objRight; // The object the player is facing 
        public GameObject playerRight; // The object the player is facing
        public GameObject objLeft; // The object opposite the side the player is facing
        public GameObject playerLeft; // The object the player is facing
        public GameObject objGround; // The game object on the ground

        public Vector2 velocity; // Stores the player's velocity at the end of the frame
    };

    public float maxSlideSpeed;
    public float minSlideSpeed;

    private bool initialBounce = true;

    [HideInInspector]
    public bool touchingLeftWall = false;

    [HideInInspector]
    public bool touchingRightWall = false;

    private State currentState;
    private State previousState;

    private Rigidbody2D body;
 
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        SetCurrentSurroundings();
        currentState.direction = GetDirection(body.velocity.x) ?? previousState.direction;
        currentState.oppDirection = GetDirection(-1 * body.velocity.x) ?? previousState.oppDirection;

        touchingLeftWall = ObjectExists(currentState.objLeft);
        touchingRightWall = ObjectExists(currentState.objRight);

        GameController.material? groundType = GetMaterial(currentState.objGround);
        GameController.material? prevGroundType = GetMaterial(previousState.objGround);

        float slideSpeed = body.velocity.x;

        if (groundType != null && groundType != GameController.material.BOUNCE)
        {
            initialBounce = true;
        }
        
        if (groundType == GameController.material.BOUNCE && !(prevGroundType == GameController.material.BOUNCE) && 
            Mathf.Abs(body.velocity.y) > 2f)
        {
            float initialBounceBonus = 0;

            if (initialBounce)
            {
                initialBounceBonus = 1f;
                initialBounce = false;
            }

            // .79 because guestimation said so 
            body.velocity = new Vector2(previousState.velocity.x, Mathf.Abs(previousState.velocity.y) + .79f + initialBounceBonus);
        }

        if (groundType == GameController.material.SLIP &&
            currentState.playerLeft == null && currentState.playerRight == null)
        {
            GameController.SurfaceSpeeds surfaceSpeeds = currentState.objGround.GetComponent<SurfaceMaterial>().surfaceSpeeds;

            if (Mathf.Abs(body.velocity.x) > surfaceSpeeds.pushSpeed / 2f)
            {
                if (Mathf.Abs(body.velocity.x) < minSlideSpeed)
                {
                    slideSpeed = minSlideSpeed;
                }
                else if (Mathf.Abs(body.velocity.x) > maxSlideSpeed)
                {
                    slideSpeed = maxSlideSpeed;
                }

                body.velocity = new Vector2(slideSpeed, body.velocity.y);
            }  
        }

        previousState = currentState;
        previousState.velocity = body.velocity;
        
    }

    /// <summary>
    /// Performs raycasts to check surfaces player is contacting, and stores those GameObjects in currentState
    /// </summary>
    private void SetCurrentSurroundings()
    {
        float normalLeniency = .05f;

        // Check for left/right collisions on Objects layer
        currentState.objLeft = RayCheck(currentState.oppDirection, null, normalLeniency, false);
        currentState.objRight = RayCheck(currentState.direction, null, normalLeniency, false);

        currentState.playerLeft = RayCheck(currentState.oppDirection, null, normalLeniency, true);
        currentState.playerRight = RayCheck(currentState.oppDirection, null, normalLeniency, true);

        // Check surface below player on both layers
        currentState.objGround = RayCheck(Direction.DOWN, null, normalLeniency, false);
    }

    private bool ObjectExists(GameObject gameObject)
    {
        if (gameObject != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private GameController.material? GetMaterial(GameObject gameObject)
    {
        if (gameObject)
        {
            return gameObject.GetComponent<SurfaceMaterial>().type;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Performs raycast alongside player collider
    /// </summary>
    /// <param name="direction">Direction of raycast</param>
    /// <param name="layerMaskName">Layer to check for collision</param>
    /// <param name="leniency">Leniency applied for ground/object raycasts (zero unless direction = down or pressing Grab)</param>
    /// <returns>Returns: Any GameObject it collides with</returns>
    private GameObject RayCheck(Direction direction, string layerMaskName, float leniency, bool playerCast)
    {
        // Raycast parameters
        float rayDistance;
        Vector2 rayOrigin;
        Vector2 rayDirection;
        RaycastHit2D raycast;

        // X,Y points used to calculate raycast origin
        float playerBottom;
        float originXPos;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        if (direction == Direction.DOWN)
        {
            // Calculate bottom of player:
            // Bottom of BoxCollider + edgeRadius around collider + leniency (subtraction because in downward direction)
            playerBottom = collider.bounds.min.y - collider.edgeRadius - leniency;

            // Calculate left edge of player (don't need leniency here):
            // Edge of BoxCollider + 1/2 of edgeRadius
            // Starts on the side opposite the direction the player is facing
            if (currentState.direction == Direction.RIGHT)
            {
                originXPos = collider.bounds.min.x - (collider.edgeRadius / 2);
                rayDirection = Vector2.right;
            }
            else
            {
                originXPos = collider.bounds.max.x + (collider.edgeRadius / 2);
                rayDirection = Vector2.left;
            }

            // Distance = width + edge radius
            rayDistance = collider.bounds.size.x + collider.edgeRadius;
        }
        else
        {
            // Calculate bottom of player (don't need leniency here):
            // Bottom of BoxCollider
            playerBottom = collider.bounds.min.y - (collider.edgeRadius / 2f);

            // Calculate distance to left edge of player:
            // Half the collider + the radius + leniency
            originXPos = (collider.bounds.size.x / 2) + (collider.edgeRadius) + leniency;

            // Left or Right determines the side of the player the ray is being shot from
            if (direction == Direction.LEFT)
            {
                originXPos = collider.bounds.center.x - originXPos;
            }
            else
            {
                originXPos = collider.bounds.center.x + originXPos;
            }

            rayDirection = Vector2.up;

            // Distance = height + edge radius
            rayDistance = collider.bounds.size.y + collider.edgeRadius;
        }

        // Raycast origin
        rayOrigin = new Vector2(originXPos, playerBottom);

        // Perform raycast (leave out layermask if null)
        if (layerMaskName != null)
        {
            raycast = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, LayerMask.GetMask(layerMaskName));
        }
        else
        {
            raycast = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
        }

        // Check for collision
        if (raycast.collider != null)
        {
            if((raycast.collider.tag == "Player" && playerCast) ||
                (raycast.collider.tag != "Player" && !playerCast))
            {
                return raycast.collider.gameObject;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns direction of given movement
    /// </summary>
    /// <param name="movement">Player movement</param>
    private Direction? GetDirection(float movement)
    {
        if (movement > 0)
        {
            return Direction.RIGHT;
        }
        else if (movement < 0)
        {
            return Direction.LEFT;
        }
        else return null;
    }
}
