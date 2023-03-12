using UnityEngine;

namespace FroggyDefense.Movement
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform m_TargetTransform;

        private void LateUpdate()
        {
            Vector3 pos = m_TargetTransform.position;
            pos.z = -10;
            transform.position = pos;
        }
    }
}