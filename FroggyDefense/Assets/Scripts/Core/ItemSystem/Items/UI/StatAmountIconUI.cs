using System;
using UnityEngine;
using UnityEngine.UI;

namespace FroggyDefense
{
    public class StatAmountIconUI : MonoBehaviour
    {

        [SerializeField] private Image Icon;

        [SerializeField] private Color color0;
        [SerializeField] private Color color1;
        [SerializeField] private Color color5;
        [SerializeField] private Color color10;
        [SerializeField] private Color color50;
        [SerializeField] private Color color100;

        /// <summary>
        /// Changes the icon color to match amount.
        /// </summary>
        /// <param name="num"></param>
        public int SetIcon(int num)
        {
            if (num >= 100)
            {
                Icon.color = color100;
                return 100;
            }
            else if (num >= 50)
            {
                Icon.color = color50;
                return 50;
            }
            else if (num >= 10)
            {
                Icon.color = color10;
                return 10;
            }
            else if (num >= 5)
            {
                Icon.color = color5;
                return 5;
            }
            else if (num >= 1)
            {
                Icon.color = color1;
                return 1;
            } else
            {
                Icon.color = color0;
                return 0;
            }
        }
    }
}
