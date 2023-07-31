using UnityEngine;
using FroggyDefense.Core.Spells;
using ShapeDrawer;

namespace FroggyDefense.Core
{
    public class InputController : MonoBehaviour
    {

        [SerializeField] private PolygonDrawer _spellRangePreview;          // Draws spell targeting range.
        [SerializeField] private PolygonDrawer _spellEffectAreaPreview;     // Draws spell targeting shape over cursor.

        private Spell _selectedSpell;                                       // The selected spell being cast.
        private Player _player;

        public delegate void InputCallBack(SpellArgs args);

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
        private bool _targetingAbility = false;
        private Vector2 _moveInput = Vector2.zero;

        private void Awake()
        {
            if (_player == null)
            {
                _player = GetComponent<Player>();
            }    
        }

        private void Update()
        {
            if (GameManager.GameStarted)
            {
                if (_targetingAbility)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;

                    MoveAbilityTargetOverlay(mousePos, _selectedSpell.TargetRange);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Confirmed Spell at (" + _spellEffectAreaPreview.transform.position + ").");
                        _selectedSpell.Cast(new SpellArgs(_player, _spellEffectAreaPreview.transform.position));
                        ClearAbilityTargetingOverlay();
                        _targetingAbility = false;
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        Debug.Log("Cancelled Spell.");
                        ClearAbilityTargetingOverlay();
                        _targetingAbility = false;
                    }
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
                        _player.m_WeaponUser.Deactivate();
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Pressed Attack");
                        if (_player == null)
                        {
                            Debug.LogWarning("PLAYER NULL");
                        }
                        else if (_player.m_WeaponUser == null)
                        {
                            Debug.LogWarning("WEAPON USER NULL");
                        }
                        else
                        {
                            _player.m_WeaponUser.Attack(Input.mousePosition);
                        }
                    }
                }
                _moveInput.x = Input.GetAxisRaw("Horizontal");
                _moveInput.y = Input.GetAxisRaw("Vertical");
                _player.Move(_moveInput);
            }
        }

        /// <summary>
        /// Sets the selected ability to be the spell in the pressed ability slot.
        /// </summary>
        /// <param name="spell"></param>
        private void TargetAbility(Spell spell)
        {
            if (spell == null)
            {
                Debug.Log("No ability selected.");
                return;
            }

            _targetingAbility = true;
            _selectedSpell = spell;

            // Draw background spell range.
            DrawAbilityRangeOverlay(spell);

        }

        /// <summary>
        /// Draws the shapes showing ability range and effect areas.
        /// </summary>
        private void DrawAbilityTargetingOverlay(Spell spell)
        {
            _targetingAbility = true;
            _selectedSpell = spell;

            _spellRangePreview.shape = new Shape(eShape.Circle, new Vector2(spell.TargetRange, spell.TargetRange));
            _spellEffectAreaPreview.shape = spell.EffectShape;

            _spellRangePreview.DrawFilledShape();
            _spellEffectAreaPreview.DrawFilledShape();
        }

        /// <summary>
        /// Draw the spell range background effect.
        /// </summary>
        /// <param name="spell"></param>
        public void DrawAbilityRangeOverlay(Spell spell)
        {
            _targetingAbility = true;
            _selectedSpell = spell;

            _spellRangePreview.shape = new Shape(eShape.Circle, new Vector2(spell.TargetRange, spell.TargetRange));
            _spellRangePreview.DrawFilledShape();
        }

        /// <summary>
        /// Moves the ability effect preview to the input position.
        /// </summary>
        private void MoveAbilityTargetOverlay(Vector3 pos, float maxDistance)
        {
            float currDistance = Vector2.Distance(pos, transform.position);
            if (currDistance > maxDistance) {
                // Outside range area.
                _spellEffectAreaPreview.transform.position = Vector2.Lerp(transform.position, pos, maxDistance / currDistance);
            } else
            {
                // Inside the range bounds.
                _spellEffectAreaPreview.transform.position = pos;
            }
        }

        /// <summary>
        /// Clears all shapes.
        /// </summary>
        private void ClearAbilityTargetingOverlay()
        {
            _spellRangePreview.EraseShape();
            _spellEffectAreaPreview.EraseShape();
        }

        /// <summary>
        /// Uses the ability in the number slot.
        /// </summary>
        /// <param name="num"></param>
        public void UseAbility(int num)
        {
            Spell spell = GameManager.instance.m_Player.SelectedAbilities[num];
            TargetAbility(spell);
        }
    }
}
