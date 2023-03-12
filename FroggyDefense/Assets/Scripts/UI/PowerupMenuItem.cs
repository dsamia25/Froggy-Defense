using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.UI
{
    public class PowerupMenuItem : MonoBehaviour
    {
        public Powerup m_Powerup;

        public void Select()
        {
            m_Powerup.Use();
        }
    }
}
