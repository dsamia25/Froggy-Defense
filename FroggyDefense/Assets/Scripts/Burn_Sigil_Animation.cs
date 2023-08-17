using UnityEngine;

namespace FroggyDefense.Dungeons
{
    public class Burn_Sigil_Animation : StateMachineBehaviour
    {
        private SpawnSigil sigil = null;
        public SpriteRenderer renderer = null;
        public float delay = 0;
        public float animationTime = 0;
        public float currTime = 0;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            sigil = animator.gameObject.GetComponent<SpawnSigil>();
            renderer = animator.gameObject.GetComponent<SpriteRenderer>();

            delay = sigil.burnAnimationDelay;
            animationTime = sigil.burnAnimationTime;

            renderer.material = new Material(sigil.burnMaterial);
            renderer.material.SetFloat("_ChalkScale", Random.Range(sigil.chalkScaleRange.x, sigil.chalkScaleRange.y));
            renderer.material.SetFloat("_BurnScale", Random.Range(sigil.burnScaleRange.x, sigil.burnScaleRange.y));
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // First counts down delay.
            if (delay < 0)
            {
                // Then counts up currTime
                if (currTime <= animationTime)
                {
                    renderer.material.SetFloat("_BurnAmount", currTime / animationTime);
                } else
                {
                    sigil.CleanUp();
                }
                currTime += Time.deltaTime;
            } else
            {
                delay -= Time.deltaTime;
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            sigil.CleanUp();
        }
    }
}
