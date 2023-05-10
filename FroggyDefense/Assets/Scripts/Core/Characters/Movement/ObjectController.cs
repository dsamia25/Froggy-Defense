using UnityEngine;

namespace FroggyDefense.Movement
{
    public class ObjectController : MonoBehaviour
    {
        [Space]
        [Header("Movement Settings")]
        [Space]

        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _moveSpeedModifer = 1f;
        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
        public float MoveSpeedModifier { get => _moveSpeedModifer; set => _moveSpeedModifer = value; }

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 moveDir)
        {
            rb.velocity = _moveSpeedModifer * _moveSpeed * moveDir;
        }
    }
}