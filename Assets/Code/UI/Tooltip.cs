using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : BaseWindow
{
    [SerializeField] private RectTransform _tooltipRect;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Vector2 _smallSize = new(200, 100);
    [SerializeField] private Vector2 _bigSize = new(400, 300);

    protected override void OnShow(object[] args)
    {
        gameObject.SetActive(true);
        _titleText.text = (string)args[0];
        _descriptionText.text = (string)args[1];
        _iconImage.sprite = args[2] as Sprite;

        AdjustRectTransform();
    }

    private void AdjustRectTransform()
    {
        var newSize = new Vector2(
            Mathf.Clamp(_descriptionText.preferredWidth, _smallSize.x, _bigSize.x),
            Mathf.Clamp(_descriptionText.preferredHeight, _smallSize.y, _bigSize.y)
        );
        _tooltipRect.sizeDelta = newSize;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_tooltipRect);
        _descriptionText.RecalculateMasking();
    }

    protected override void OnHide()
    {
        gameObject.SetActive(false);
    }
}