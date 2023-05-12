using System.Collections;
using UnityEngine;

namespace FroggyDefense.Movement
{
    public class ObjectController : MonoBehaviour
    {
        [Space]
        [Header("Movement Settings")]
        [Space]
        [SerializeField] private bool _movementLocked = false;
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _moveSpeedModifer = 1f;
        public bool MovementLocked => _movementLocked;
        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
        public float MoveSpeedModifier { get => _moveSpeedModifer; set => _moveSpeedModifer = value; }

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Sets the rigidbody velocity to move in the set direction.
        /// </summary>
        /// <param name="moveDir"></param>
        public void Move(Vector2 moveDir)
        {
            if (!_movementLocked)
            {
                rb.velocity = _moveSpeedModifer * _moveSpeed * moveDir;
            }
        }

        /// <summary>
        /// Makes the rigidbody lunge in the set direction.
        /// Locks movement for a set time.
        /// </summary>
        public void Lunge(Vector2 moveDir, float lungeStrength, float slowTime, float moveLockTime)
        {
            Freeze();
            StartCoroutine(SetMovementLockTimer(moveLockTime));
            StartCoroutine(DecreaseLungeVelocity(moveDir, lungeStrength, slowTime));
        }

        /// <summary>
        /// Freezes movement
        /// </summary>
        public void Freeze()
        {
            rb.velocity = Vector2.zero;
        }

        /// <summary>
        /// Decreases lunge velocity to zero over a set time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator DecreaseLungeVelocity(Vector2 moveDir, float lungeStrength, float slowTime)
        {
            rb.velocity = lungeStrength * moveDir;
            Vector2 startingVelocity = rb.velocity;
            for (float i = 0; i <= 1; i += (Time.deltaTime / slowTime))
            {
                rb.velocity = Vector2.Lerp(startingVelocity, Vector2.zero, i);
                yield return null;
            }
        }

        /// <summary>
        /// Locks movement and unlocks after a set time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator SetMovementLockTimer(float time)
        {
            _movementLocked = true;
            yield return new WaitForSeconds(time);
            _movementLocked = false;
        }
    }
}