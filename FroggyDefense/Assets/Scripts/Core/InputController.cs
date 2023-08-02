using System;
using UnityEngine;
using FroggyDefense.Core.Spells;
using FroggyDefense.Core.Actions.Inputs;

namespace FroggyDefense.Core
{
    public class InputController : MonoBehaviour
    {
        // Input listeners.
        public AreaInput AreaListener;              // Component to listen for player click inputs for spells.
        public ProjectileInput ProjectileListener;  // Component to listen for player projectile inputs for spells.
        public DragInput DragListener;              // Component to listen for player drag inputs for spells.

        private Spell SelectedSpell;                                       // The selected spell being cast.
        private Player m_Player;

        public delegate void InputCallBack(InputArgs args);

        /*
         * Inputs:
         * 
         * Weapon:
         *  - Attack (Left Click)
         *  * Note: Disabled while targetting spell.
         *  
         * Spell Hotkeys:
         *  - Spell1 (1)
         *  - Spell2 (2)
         *  - Spell3 (3)
         *  - Spell4 (4)
         *  
         *  Movement:
         *  - Up (W, Up Arrow)
         *  - Left (A, Left Arrow)
         *  - Down (S, Down Arrow)
         *  - Right (D, Right Arrow)
         *  
         * 
         */
        private bool TargetingAbility => ActionInput.InputListenerActive;
        private Vector2 _moveInput = Vector2.zero;

        /// <summary>
        /// Enum showing how the user input the action.
        /// </summary>
        public enum ActionInputType
        {
            NULL,
            Keyboard,
            UIButton
        }

        private void Awake()
        {
            if (m_Player == null)
            {
                m_Player = GetComponent<Player>();
            }    
        }

        private void Update()
        {
            if (GameManager.GameStarted)
            {
                if (TargetingAbility)
                {
                    //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //mousePos.z = 0;

                    //MoveAbilityTargetOverlay(mousePos, SelectedSpell.TargetRange);
                    //if (Input.GetMouseButtonDown(0))
                    //{
                    //    Debug.Log("Confirmed Spell at (" + _spellEffectAreaPreview.transform.position + ").");
                    //    SelectedSpell.Cast(new SpellArgs(m_Player, _spellEffectAreaPreview.transform.position));
                    //    ClearAbilityTargetingOverlay();
                    //    _targetingAbility = false;
                    //}
                    //else if (Input.GetMouseButtonDown(1))
                    //{
                    //    Debug.Log("Cancelled Spell.");
                    //    ClearAbilityTargetingOverlay();
                    //    _targetingAbility = false;
                    //}
                    Debug.Log($"Already targeting an ability.");
                }
                else
                {
                    if (Input.GetButtonDown("Ability1"))
                    {
                        UseAbility(0);
                    }
                    else if (Input.GetButtonDown("Ability2"))
                    {
                        UseAbility(1);
                    }
                    else if (Input.GetButtonDown("Ability3"))
                    {
                        UseAbility(2);
                    }
                    else if (Input.GetButtonDown("Ability4"))
                    {
                        UseAbility(3);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("Released Attack");
                        m_Player.m_WeaponUser.Deactivate();
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Pressed Attack");
                        if (m_Player == null)
                        {
                            Debug.LogWarning("PLAYER NULL");
                        }
                        else if (m_Player.m_WeaponUser == null)
                        {
                            Debug.LogWarning("WEAPON USER NULL");
                        }
                        else
                        {
                            m_Player.m_WeaponUser.Attack(Input.mousePosition);
                        }
                    }
                }
                _moveInput.x = Input.GetAxisRaw("Horizontal");
                _moveInput.y = Input.GetAxisRaw("Vertical");
                m_Player.Move(_moveInput);
            }
        }

        /// <summary>
        /// Uses the ability in the number slot.
        /// </summary>
        /// <param name="num"></param>
        public void UseAbility(int num)
        {
            Spell spell = GameManager.instance.m_Player.SelectedAbilities[num];
            if (spell == null)
            {
                Debug.Log("No ability selected.");
                return;
            }
            
            SelectedSpell = spell;

            switch (spell.TargetMode)
            {
                case InputMode.AreaInput:
                    AreaListener.Activate(spell, ConfirmInput);
                    break;
                case InputMode.ProjectileInput:
                    ProjectileListener.Activate(spell, ConfirmInput);
                    break;
                case InputMode.DragInput:
                    DragListener.Activate(spell, ConfirmInput);
                    break;
                default:
                    Debug.LogWarning($"Unknown input type.");
                    break;
            }
        }

        public void ConfirmInput(InputArgs args)
        {
            SelectedSpell.Cast(new SpellArgs(m_Player, args));
        }
    }
}
