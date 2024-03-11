using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _text;

    [SerializeField] private UnityEvent _onClick;
    [SerializeField] private float _animationDuration = 0.1f;
    [SerializeField] private float _animationScale = 0.9f;
    [SerializeField] private float _animationFade = 0.5f;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        transform.DOScale(_animationScale, _animationDuration).SetEase(Ease.InOutBack).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        bool isHovered = false;

        foreach (GameObject gameObject in eventData.hovered)
        {
            if (gameObject == this.gameObject)
            {
                isHovered = true;
                break;
            }
        }

        transform.DOScale(1, _animationDuration).SetEase(Ease.InOutBack).SetUpdate(true);

        if (!isHovered) return;

        _onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.DOFade(1, _animationDuration).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.DOFade(_animationFade, _animationDuration).SetUpdate(true);
    }


    public void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.alpha = _animationFade;
    }
}
