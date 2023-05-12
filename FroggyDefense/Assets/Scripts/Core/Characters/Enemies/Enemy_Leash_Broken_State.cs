using UnityEngine;

namespace FroggyDefense.Core.Enemies
{
    public class Enemy_Leash_Broken_State : StateMachineBehaviour
    {
        private Enemy _enemy;
        private float _currLeashResetTime;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemy = animator.gameObject.GetComponent<Enemy>();
            _enemy.ResetFocus();

            _currLeashResetTime = _enemy.LeashResetTime;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_currLeashResetTime <= 0f)
            {
                animator.SetTrigger("LeashReset");
            }
            _currLeashResetTime -= Time.deltaTime;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("LeashReset");
        }
    }
}
