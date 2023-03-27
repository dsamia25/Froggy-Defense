using System;
using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Items;

namespace FroggyDefense.UI
{
    public class CharacterSheetInfoUI : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private GameObject _equipmentSlotPrefab;
        [SerializeField] private Transform _uiParent;

        private EquipmentSlotUI[] _equipmentSlots;

        private void Start()
        {
            if (_uiParent == null) _uiParent = transform;

            GenerateEquipmentSlots();

            // Subscribe to events.
            GameManager.instance.m_Player.CharacterStatsChanged.AddListener(UpdateUI);
        }

        /// <summary>
        /// Creates a new UI representation of this inventory by instantiating new object buttons.
        /// </summary>
        public void GenerateEquipmentSlots()
        {
            int slotAmount = Enum.GetValues(typeof(EquipmentSlot)).Length;
            Debug.Log("There are " + slotAmount + " EquipmentSlot values.");
            _equipmentSlots = new EquipmentSlotUI[slotAmount];

            for (int i = 0; i < slotAmount; i++)
            {
                var slot = Instantiate(_equipmentSlotPrefab, _uiParent).GetComponent<EquipmentSlotUI>();
                slot.SlotType = (EquipmentSlot)i;   // int cast as slot.
                slot.HeadCharacterSheetInfoUi = this;
                _equipmentSlots[i] = slot;
                slot.UpdateUI();
            }
        }

        public void UpdateUI()
        {
            for (int i = 0; i < _equipmentSlots.Length; i++)
            {
                var slot = _equipmentSlots[i];
                slot.equipment = _character.GetEquipment(i);
                slot.UpdateUI();
            }
        }
    }
}
