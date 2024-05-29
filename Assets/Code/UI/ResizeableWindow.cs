using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ResizeableWindow: MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Vector2 _smallSize = new(100, 100);
        [SerializeField] private Vector2 _bigSize = new(200, 200);
        
        private void Awake()
        {
            _text.OnPreRenderText += AdjustRectTransform;
        }

        private void AdjustRectTransform(TMP_TextInfo text)
        {
            Canvas.ForceUpdateCanvases();
            var rect = GetComponent<RectTransform>();
            
            var newSize = new Vector2(
                Mathf.Clamp(_text.preferredWidth, _smallSize.x, _bigSize.x),
                Mathf.Clamp(_text.preferredHeight, _smallSize.y, _bigSize.y)
            );
            rect.sizeDelta = newSize;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            _text.RecalculateMasking();
            
            Canvas.ForceUpdateCanvases();
        }
    }
}