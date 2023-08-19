using UnityEngine;

namespace FroggyDefense.Core
{
    public class Enemy_Summoned : StateMachineBehaviour
    {
        private Character character = null;
        private float animationTime = 1f;
        private float currTime = 0f;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log($"Enemy_Summoned enter.");
            animator.ResetTrigger("SummonAnimation");

            character = animator.gameObject.GetComponentInParent<Character>();
            character.BeingSummoned = true;

            animationTime = SpawnManager.instance.SpawnAnimationTime + SpawnManager.instance.SpawnDelay;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log($"Enemy_Summoned time: {currTime}.");
            if (currTime > animationTime)
            {
                animator.SetTrigger("FinishedSummon");
            }
            currTime += Time.deltaTime;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            character.BeingSummoned = false;
            animator.ResetTrigger("FinishedSummon");
        }
    }
}
