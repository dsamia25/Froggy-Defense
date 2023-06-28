using UnityEngine;

namespace FroggyDefense.Core.Enemies
{
    public class Enemy_Alert_State : StateMachineBehaviour
    {
        private Enemy _enemy;
        private float _currCheckCooldown;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemy = animator.gameObject.GetComponent<Enemy>();
            _enemy.ResetFocus();

            _currCheckCooldown = _enemy.TargetCheckFrequency;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_enemy.TargetsPlayer)
            {
                if (_currCheckCooldown <= 0)
                {
                    // Look for players to attack.
                    var target = _enemy.DetectTargets();
                    if (target != null)
                    {
                        // If player detected, set focus to closest player.
                        _enemy.Focus = target.gameObject;

                        // Change state to chase to move towards the new focus.
                        animator.SetBool("DetectedPlayer", true);
                    }
                    _currCheckCooldown = _enemy.TargetCheckFrequency;
                }
                else
                {
                    _currCheckCooldown -= Time.deltaTime;
                }
            }
        }
    }
}
