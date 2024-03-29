using UnityEngine;
using FroggyDefense.Core;

public class Player_Damaged : StateMachineBehaviour
{
    private Character character = null;
    public SpriteRenderer renderer = null;
    public float DamagedAnimationTime = 1f;
    public float currTime = 0f;
    private float animationPercent = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character = animator.gameObject.GetComponentInParent<Character>();
        DamagedAnimationTime = character.m_DamagedAnimationTime;
        currTime = 0f;
        animationPercent = 0f;

        renderer = animator.gameObject.GetComponent<SpriteRenderer>();
        renderer.material = new Material(character.DamagedMaterial);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        try
        {
            if (currTime <= DamagedAnimationTime)
            {
                renderer.material.SetFloat("_Offset", animationPercent / DamagedAnimationTime);
            }
            else
            {
                animator.SetBool("DamagedAnimation", false);
            }
        } catch
        {
            Debug.LogWarning("Abandoning Player_Damaged animation.");
            animator.SetBool("DamagedAnimation", false);
        }

        currTime += Time.deltaTime;
        animationPercent += (currTime >= (DamagedAnimationTime / 2)  ? -1 : 1) * Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        renderer.material = new Material(character.DefaultMaterial);
    }
}
