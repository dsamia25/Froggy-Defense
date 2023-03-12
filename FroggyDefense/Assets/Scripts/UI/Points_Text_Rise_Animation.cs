using UnityEngine;

namespace FroggyDefense.UI
{
    public class Points_Text_Rise_Animation : StateMachineBehaviour
    {
        PointsText m_Text;
        float m_RiseTime;
        float m_FadeTime;

        float _startingHeight;
        float _targetHeight;
        float _time;
        float _fadeDelay;
        float _fade;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_Text = animator.gameObject.GetComponent<PointsText>();

            m_RiseTime = m_Text.m_RaiseTextTime;
            m_FadeTime = m_Text.m_FadeTextTime;
            _fadeDelay = m_Text.m_FadeTextDelay;

            _startingHeight = animator.transform.position.y;
            _targetHeight = _startingHeight + m_Text.m_RaiseTextHeight;
            _time = 0;
            _fade = 1;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_time > m_RiseTime)
            {
                animator.SetTrigger("Delete");    // Done with state.
            }

            Vector2 pos = animator.transform.position;
            pos.y = Mathf.Lerp(_startingHeight, _targetHeight, _time / m_RiseTime);
            animator.transform.position = pos;

            if (_time > _fadeDelay)
            {
                m_Text.SetFadePercent(_fade);
                _fade -= Time.deltaTime / m_FadeTime;
            }

            _time += Time.deltaTime;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject);
        }
    }
}
