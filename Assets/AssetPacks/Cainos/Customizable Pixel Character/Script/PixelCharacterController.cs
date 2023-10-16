using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cainos.CustomizablePixelCharacter
{
    public class PixelCharacterController : MonoBehaviour
    {
        const float GROUND_CHECK_RADIUS = 0.1f;                 // radius of the overlap circle to determine if the character is on ground

        public MovementType defaultMovement = MovementType.Walk;

        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode crouchKey = KeyCode.S;
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode moveModifierKey = KeyCode.LeftShift;

        public KeyCode attackKey = KeyCode.Mouse0;

        public float walkSpeedMax = 2.5f;                       // max walk speed, ideally should be half of runSpeedMax
        public float walkAcc = 10.0f;                           // walking acceleration

        public float runSpeedMax = 5.0f;                        // max run speed
        public float runAcc = 10.0f;                            // running acceleration

        public float crouchSpeedMax = 1.0f;                     // max move speed while crouching
        public float crouchAcc = 8.0f;                          // crouching acceleration

        public float airSpeedMax = 2.0f;                        // max move speed while in air
        public float airAcc = 8.0f;                             // air acceleration

        public float groundBrakeAcc = 6.0f;                     // braking acceleration (from movement to still) while on ground
        public float airBrakeAcc = 1.0f;                        // braking acceleration (from movement to still) while in air

        public float jumpSpeed = 5.0f;                          // speed applied to character when jump
        public float jumpCooldown = 0.55f;                      // time to be able to jump again after landing
        public float jumpGravityMutiplier = 0.6f;               // gravity multiplier when character is jumping, should be within [0.0,1.0], set it to lower value so that the longer you press the jump button, the higher the character can jump    
        public float fallGravityMutiplier = 1.3f;               // gravity multiplier when character is falling, should be equal or greater than 1.0

        public float groundCheckRadius = 0.17f;                 // radius of the circle at the character's bottom to determine whether the character is on ground

        [ExposeProperty]                                        // is the character dead? if dead, plays dead animation and disable control
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
                fx.IsDead = isDead;
                fx.DropWeapon();
            }
        }                    
        private bool isDead;


        private PixelCharacter fx;                              // the FXCharacter script attached the character
        private CapsuleCollider2D collider2d;                   // Collider compoent on the character
        private Rigidbody2D rb2d;                               // Rigidbody2D component on the character


        private bool isGrounded;                                // is the character on ground
        private Vector2 curVel;                                 // current velocity
        private float jumpTimer;                                // timer for jump cooldown
        private Vector2 posBot;                                 // local position of the character's middle bottom
        private Vector2 posTop;                                 // local position of the character's middle top

        private void Awake()
        {
            fx = GetComponent<PixelCharacter>();
            collider2d = GetComponent<CapsuleCollider2D>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            posBot = collider2d.offset - new Vector2 ( 0.0f , collider2d.size.y * 0.5f );
            posTop = collider2d.offset + new Vector2( 0.0f, collider2d.size.y * 0.5f );
        }

        private void Update()
        {
            if (jumpTimer < jumpCooldown) jumpTimer += Time.deltaTime;

            //RECEIVE INPUT
            bool inputCrounch = false;
            bool inputMoveModifier = false;
            bool inputJump = false;
            float inputH = 0.0f;
            bool inputAttack = false;
            bool inputAttackContinuous = false;

            bool pointerOverUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();
            if (!pointerOverUI)
            {
                inputCrounch = Input.GetKey(crouchKey);
                inputMoveModifier = Input.GetKey(moveModifierKey);
                inputJump = Input.GetKey(jumpKey);
                inputAttack = Input.GetKeyDown(attackKey);
                inputAttackContinuous = Input.GetKey(attackKey);
            }

            if (Input.GetKey(leftKey)) inputH = -1.0f;
            else
            if (Input.GetKey(rightKey)) inputH = 1.0f;
            else inputH = 0.0f;

            bool inputRun = false;
            if (defaultMovement == MovementType.Walk && inputMoveModifier) inputRun = true;
            if (defaultMovement == MovementType.Run && !inputMoveModifier) inputRun = true;


            //PERFORM MOVE OR ATTACK BASED ON INPUT
            Move(inputH, inputCrounch, inputRun, inputJump);
            Attack(inputAttack, inputAttackContinuous);


            //CHECK IF THE CHARACTER IS ON GROUND
            isGrounded = false;
            Vector2 worldPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos + posBot, groundCheckRadius);
            for (int i = 0; i < colliders.Length; i++ )
            {
                if ( colliders[i].isTrigger ) continue;
                if ( colliders[i].gameObject != gameObject ) isGrounded = true;
            }
        }

        public void Attack( bool inputAttack , bool inputAttackContinuous)
        {
            if ( inputAttack ) fx.Attack();
            fx.IsAttacking = inputAttackContinuous;
        }

        public void Move(float inputH, bool inputCrouch, bool inputRunning, bool inputJump)
        {
            if (isDead) return;

            //GET CURRENT SPEED FROM RIGIDBODY
            curVel = rb2d.velocity;

            //SET ACCELERATION AND MAX SPEED BASE ON CONDITION
            float acc = 0.0f;
            float max = 0.0f;
            float brakeAcc = 0.0f;

            if (isGrounded)
            {
                acc = inputRunning ? runAcc : walkAcc;
                max = inputRunning ? runSpeedMax : walkSpeedMax;
                brakeAcc = groundBrakeAcc;

                if (inputCrouch)
                {
                    acc = crouchAcc;
                    max = crouchSpeedMax;
                }
            }
            else
            {
                acc = airAcc;
                max = airSpeedMax;
                brakeAcc = airBrakeAcc;
            }


            //HANDLE HORIZONTAL MOVEMENT
            //has horizontal movement input
            if (Mathf.Abs(inputH) > 0.01f)
            {
                //if current horizontal speed is out of allowed range, let it fall to the allowed range
                bool shouldMove = true;
                if (inputH > 0 && curVel.x >= max)
                {
                    curVel.x = Mathf.MoveTowards(curVel.x, max, brakeAcc * Time.deltaTime);
                    shouldMove = false;
                }
                if (inputH < 0 && curVel.x <= -max)
                {
                    curVel.x = Mathf.MoveTowards(curVel.x, -max, brakeAcc * Time.deltaTime);
                    shouldMove = false;
                }

                //otherwise, add movement acceleration to cureent velocity
                if (shouldMove) curVel.x += acc * Time.deltaTime * inputH;
            }
            //no horizontal movement input, brake to speed zero
            else
            {
                curVel.x = Mathf.MoveTowards(curVel.x, 0.0f, brakeAcc * Time.deltaTime);
            }

            //JUMP
            if (isGrounded && inputJump && jumpTimer >= jumpCooldown)
            {
                isGrounded = false;
                jumpTimer = 0.0f;
                curVel.y += jumpSpeed;
            }
            if ( inputJump && curVel.y > 0)
            {
                curVel.y += Physics.gravity.y * (jumpGravityMutiplier -1.0f) * Time.deltaTime;
            }
            else if (curVel.y > 0)
            {
                curVel.y += Physics.gravity.y * ( fallGravityMutiplier - 1.0f) * Time.deltaTime;
            }


            rb2d.velocity = curVel;

            float movingBlend = Mathf.Abs(curVel.x) / runSpeedMax;
            fx.MovingBlend = Mathf.Abs(curVel.x) / runSpeedMax;

            if (isGrounded) fx.IsCrouching = inputCrouch;

            fx.SpeedVertical = curVel.y;
            fx.Facing = Mathf.RoundToInt(inputH);
            fx.IsGrounded = isGrounded;
        }

        public enum MovementType
        {
            Walk,
            Run
        }

        private void OnDrawGizmosSelected()
        {
            //Draw the ground detection circle
            Gizmos.color = Color.white;
            Vector2 worldPos = transform.position;
            Gizmos.DrawWireSphere(worldPos + posBot, groundCheckRadius);
        }

    }
}



