using System;
using UnityEngine;

namespace Factory
{
    public class Pusher : MonoBehaviour
    {
        public float pushForce; // 푸셔가 칩을 미는 힘

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Chip"))
            {
                Rigidbody chipRb = collision.gameObject.GetComponent<Rigidbody>();
                if (chipRb != null)
                {
                    Vector3 pushDirection = transform.forward * pushForce * Time.deltaTime;
                    chipRb.MovePosition(chipRb.position + pushDirection);
                }
            }
        }
    }    
}

