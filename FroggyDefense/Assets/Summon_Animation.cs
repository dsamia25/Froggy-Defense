using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense
{
    public class Summon_Animation : StateMachineBehaviour
    {
        private Character character = null;
        private SpriteRenderer renderer = null;
        public float delay = 0;
        private float animationTime = 1f;
        private float currTime = 0f;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            character = animator.gameObject.GetComponentInParent<Character>();
            character.m_Invincible = true;
            animationTime = SpawnManager.instance.SpawnAnimationTime;
            delay = SpawnManager.instance.SpawnDelay;
            renderer = animator.gameObject.GetComponent<SpriteRenderer>();
            renderer.material = new Material(SpawnManager.instance.SpawnMaterial);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // First counts down delay.
            if (delay < 0)
            {
                // Then count up currTime.
                if (currTime <= animationTime)
                {
                    renderer.material.SetFloat("_Fill", currTime / animationTime);
                }
                else
                {
                    animator.SetTrigger("FinishedSummon");
                }
                currTime += Time.deltaTime;
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            renderer.material = new Material(character.DefaultMaterial);
            character.m_Invincible = false;
            animator.ResetTrigger("FinishedSummon");
        }
    }
}
