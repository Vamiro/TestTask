using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : BaseWindow
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _iconImage;
    
    protected override void OnShow(object[] args)
    {
        gameObject.SetActive(true);
        _titleText.text = (string)args[0];
        _descriptionText.text = (string)args[1];
        _iconImage.sprite = args[2] as Sprite;
    }
    
    protected override void OnHide()
    {
        gameObject.SetActive(false);
    }
}