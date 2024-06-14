using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CLLMM.Scripts
{
    public class UI_ModalChoice : MonoBehaviour
    {
        public class ModalChoiceOption
        {
            public string ButtonText;
            public Action OnClick;
        }

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private RectTransform _modalTransform;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _contentText;
        [SerializeField] private RectTransform _buttonTransform;
        [SerializeField] private Button _buttonPrototype;

        private void Start()
        {
            _buttonPrototype.gameObject.SetActive(false);
            
            HideModal();
        }
        
        public void DoModal(string title, string content, params ModalChoiceOption[] options)
        {
            _titleText.text = title;
            _contentText.text = content;

            foreach (Transform child in _buttonTransform)
            {
                Destroy(child.gameObject);
            }

            foreach (var option in options)
            {
                var button = Instantiate(_buttonPrototype, _buttonTransform);
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TMP_Text>().text = option.ButtonText;
                button.onClick.AddListener(() =>
                {
                    option.OnClick?.Invoke();
                    HideModal();
                });
            }
            
            ShowModal();
        }

        private void ShowModal()
        {
            gameObject.SetActive(true);
            _backgroundImage.DOFade(0.8f, 0.25f).SetEase(Ease.OutBack);
            _modalTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        }

        private void HideModal()
        {
            _backgroundImage.DOFade(0f, 0.25f).SetEase(Ease.InBack);
            _modalTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}