using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Interactables;
using FroggyDefense.Economy;
using FroggyDefense.UI;

namespace FroggyDefense.Core
{
    public class GemManager : MonoBehaviour
    {
        [SerializeField] private GameObject GemDropperPrefab;
        [SerializeField] private UICounter GemText;

        public CurrencyObject GemCurrencyObject;
        public GemValueChartObject GemChart;

        public int Gems { get => m_PlayerWallet.GetAmount(GemCurrencyObject); }

        private CurrencyWallet m_PlayerWallet;

        private void Start()
        {
            m_PlayerWallet = GameManager.instance.m_Player.GetComponent<CurrencyWallet>();

            // Subscribe to events.
            DropGems.DropGemsEvent += OnDropGemsEvent;
            m_PlayerWallet.CurrencyAmountChangedEvent += OnGemPickedUpEvent;
        }

        private void OnGemPickedUpEvent(WalletEventArgs args)
        {
            if (args.Currency == GemCurrencyObject)
            {
                GemText.UpdateText(m_PlayerWallet.GetAmount(GemCurrencyObject));
            }
        }

        private void OnDropGemsEvent(DropGemsEventArgs args)
        {
            Debug.Log("Spawning GemDropper.");
            GameObject dropper = Instantiate(GemDropperPrefab, args.pos, Quaternion.identity);
            dropper.transform.SetParent(transform);
            dropper.GetComponent<GemDropper>().Drop(args.gems);
        }
    }
}