using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField] private int hintsCount = 5;
        [SerializeField] private TMP_Text hintAmountText;
        [SerializeField] private Button hintButton;
        public Action onHintUsed;

        private void Start()
        {
            UpdateHintButtonState();
            hintButton.onClick.AddListener(UseHint);
        }

        private void UseHint()
        {
            hintsCount--;
            onHintUsed?.Invoke();

            UpdateHintButtonState();
        }

        private void UpdateHintButtonState()
        {
            hintButton.interactable = hintsCount > 0;
            SetHintAmountText();
        }

        private void SetHintAmountText()
        {
            hintAmountText.text = $"{hintsCount}";
        }

        private void OnDestroy()
        {
            hintButton.onClick.RemoveListener(UseHint);
        }
    }
}
