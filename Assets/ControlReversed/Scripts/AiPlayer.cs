﻿using UnityEngine;

namespace ControlReversed
{
    public class AiPlayer : MonoBehaviour
    {
        public float speed = 5f;
        public float jumpForce = 10f;
        public float groundDistance = 0.1f;
        public float obstacleDistance = 1f;
        public float enemyDistance = 2f;
        public LayerMask groundLayer;
        public LayerMask obstacleLayer;
        public LayerMask enemyLayer;
        private Rigidbody2D rb;
        private Animator anim;
        private SpriteRenderer sr;
        private int direction = 1;
        private bool isGrounded = false;
        private bool isAttacking = false;
        float delay = 0.2f;
        float nextAtackTime = 0;
        private enum State { Idle, Running, Jumping, Falling }
        private State state = State.Idle;
        Bow bow;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            bow = GetComponentInChildren<Bow>();
        }
        void Update()
        {
            if (state == State.Running)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);
                if (hit.collider != null)
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }

                RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, Vector2.right * direction, obstacleDistance, obstacleLayer);
                if (obstacleHit.collider != null)
                {
                    if (isGrounded)
                    {
                        Jump();
                    }
                }

                RaycastHit2D enemyHit = Physics2D.CircleCast(transform.position, enemyDistance, Vector2.zero, 0, enemyLayer);
                if (enemyHit.collider != null)
                {

                    Debug.Log("enemyHit.collider:" + enemyHit.collider.name);
                    if (Time.time > nextAtackTime && !isAttacking)
                    {
                        isAttacking = true;
                        nextAtackTime = Time.time + delay;
                        Attack(enemyHit.transform.position);
                    }
                }
                else
                {
                    if (isAttacking)
                    {
                        StopAttack();
                    }
                    Move();
                }

                Debug.Log("isGrounded:" + isGrounded + "\nisAttacking:" + isAttacking + "\nspeed:" + speed);
                anim.SetBool("isGrounded", isGrounded);
                anim.SetBool("isAttacking", isAttacking);
                anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
            }

        }
        public void StartGame()
        {
            state = State.Running;
        }

        void Move()
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            sr.flipX = direction < 0;
        }

        void Jump()
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        void Attack(Vector3 EnemyPos)
        {
            bow.Shoot(EnemyPos);
            rb.velocity = Vector2.zero;
            StopAttack();
        }

        void StopAttack()
        {
            isAttacking = false;
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDistance);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * obstacleDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * enemyDistance);
        }
    }
}
