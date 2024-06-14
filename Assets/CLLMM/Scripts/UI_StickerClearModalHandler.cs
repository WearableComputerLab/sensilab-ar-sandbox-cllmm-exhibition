using System;
using UnityEngine;

namespace CLLMM.Scripts
{
    public class UI_StickerClearModalHandler : MonoBehaviour
    {
        [SerializeField] private UI_ModalChoice _modal;
        [SerializeField] private MapboxPinManager _pinManager;

        private void Start()
        {
            if (_modal == null)
            {
                _modal = FindObjectOfType<UI_ModalChoice>();
            }
        }

        public void DoStickerClearModal()
        {
            _modal.DoModal(
                "Clear Stickers",
                "Are you sure you want to clear all stickers?",
                new UI_ModalChoice.ModalChoiceOption()
                {
                    ButtonText = "Back",
                    OnClick = () =>
                    {
                    }
                },
                new UI_ModalChoice.ModalChoiceOption()
                {
                    ButtonText = "Clear",
                    OnClick = () =>
                    {
                        _pinManager.DestroyAllMapPins();
                    }
                });
        }
    }
}