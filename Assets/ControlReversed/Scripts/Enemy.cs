using UnityEngine;
namespace ControlReversed
{

    public class Enemy : MonoBehaviour
    {
        float delay = 0.5f;
        float nextAtackTime = 0;
        private float playerDistance = 6;
        public LayerMask playerLayer;
        private bool isAttacking = false;
        Bow bow;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            bow = GetComponentInChildren<Bow>();
        }
        private void Update()
        {
            RaycastHit2D playerHit = Physics2D.CircleCast(transform.position, playerDistance, Vector2.zero, 0, playerLayer);
            if (playerHit.collider != null)
            {
                if (Time.time > nextAtackTime && !isAttacking)
                {
                    isAttacking = true;
                    nextAtackTime = Time.time + delay;
                    Attack(playerHit.transform.position);
                }
            }
        }
        void Attack(Vector3 playerPos)
        {
            bow.Shoot(playerPos);
            rb.velocity = Vector2.zero;
            StopAttack();
        }
        void StopAttack()
        {
            isAttacking = false;
        }
    }

}