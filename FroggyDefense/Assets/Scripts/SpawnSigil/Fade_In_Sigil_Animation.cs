using UnityEngine;

namespace FroggyDefense.Dungeons
{
    public class Fade_In_Sigil_Animation : StateMachineBehaviour
    {
        SpawnSigil sigil = null;
        SpriteRenderer renderer = null;
        float animationTime = 0;
        float currTime = 0;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            sigil = animator.gameObject.GetComponent<SpawnSigil>();
            renderer = animator.gameObject.GetComponent<SpriteRenderer>();

            animationTime = sigil.fadeTime;

            renderer.material = new Material(sigil.randomName);
            renderer.material.SetFloat("_ChalkScale", Random.Range(sigil.chalkScaleRange.x, sigil.chalkScaleRange.y));
            renderer.material.SetFloat("_BurnScale", Random.Range(sigil.burnScaleRange.x, sigil.burnScaleRange.y));
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (currTime <= animationTime)
            {
                renderer.material.SetFloat("_FadeAmount", currTime / animationTime);
            }
            else
            {
                animator.SetTrigger("FinishedStep");
            }
            currTime += Time.deltaTime;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetTrigger("FinishedStep");
        }
    }
}
