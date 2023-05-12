using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Enemies
{
    public class Enemy_Chase_State : StateMachineBehaviour
    {
        private Enemy _enemy;
        private float _currTimeOutsideLeashRadius;
        private float _currLeashTime;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemy = animator.gameObject.GetComponent<Enemy>();
            _currTimeOutsideLeashRadius = _enemy.LeashBreakTime;
            _currLeashTime = 0f;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var distance = Vector2.Distance(animator.transform.position, _enemy.Focus.transform.position);
            var dT = Time.deltaTime;

            // If the player is within attack range, attack.
            if (distance <= _enemy.WeaponRange)
            {
                animator.SetBool("Attacking", true);
            } else if (distance > _enemy.LeashRadius)            // If the player is outside of leash range, count down the leash break time.
            {
                _currTimeOutsideLeashRadius -= dT;
                if (_currTimeOutsideLeashRadius <= 0)
                {
                    // Break leash.
                    animator.SetBool("DetectedPlayer", false);
                }
            } else // If the player is in normal leashing range, reset the reset countdown.
            {
                _currTimeOutsideLeashRadius = _enemy.LeashBreakTime;
            }

            _currLeashTime += dT;
            if (_enemy.HasMaxLeashTime && _currLeashTime >= _enemy.MaxLeashTime)
            {
                // Break leash.
                animator.SetBool("DetectedPlayer", false);
            }
        }
    }
}
