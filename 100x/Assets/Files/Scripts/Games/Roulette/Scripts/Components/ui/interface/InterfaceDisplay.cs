using UnityEngine;
using ViewModel;
using UniRx;

namespace Components
{
    public class InterfaceDisplay : MonoBehaviour
    {
        public CharacterTable characterTable;

        // UI Anchor
        public GameObject optionsAnchor;
        public GameObject bottomAnchor;
        public GameObject leftAnchor;

        // Shadow Background
        public Animator shadowGame;

        void Start()
        {
            characterTable.OnRound
                .Subscribe(DisplayInterface)
                .AddTo(this);

            characterTable.characterMoney.currentPayment
                .Subscribe(OnPaymentDisplay)
                .AddTo(this);
        }

        private void OnPaymentDisplay(int value)
        {
            bool display = value > 0 || value < 0 ? false : true;

            optionsAnchor.SetActive(display);
            bottomAnchor.SetActive(display);
            leftAnchor.SetActive(display);
        }

        public void DisplayInterface(bool isRound)
        {
            shadowGame.SetBool("Shadow", isRound);

            if (!isRound) return;

            optionsAnchor.SetActive(!isRound);
            bottomAnchor.SetActive(!isRound);
            leftAnchor.SetActive(!isRound);
        }
    }
}
