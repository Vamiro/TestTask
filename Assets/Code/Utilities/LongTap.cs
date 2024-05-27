using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Code.Utilities
{
    public class LongTap : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler
    {
        private const float _delay = 1.0f;

        private float _timer;
        private bool _isPressed;
        private IPressable _object;

        public event UnityAction OnLongTap;
        public event UnityAction OnLongTapEnd;

        private void Update()
        {
            if (_isPressed && Time.time - _timer >= _delay && _object != null)
            {
                _isPressed = false;
                _object?.OnPress();
                OnLongTap?.Invoke();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - _timer >= _delay)
                {
                    _object?.OnRelease();
                    OnLongTapEnd?.Invoke();
                }

                _object = null;
                _isPressed = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isPressed && eventData.hovered.Count != 0 &&
                eventData.hovered.Find(x => x.GetComponent<IPressable>() != null) != null)
            {
                _object = eventData.hovered.Find(x => x.GetComponent<IPressable>() != null).GetComponent<IPressable>();
                _isPressed = true;
                _timer = Time.time;
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (Time.time - _timer < _delay)
            {
                _object = null;
                _isPressed = false;
            }
        }
    }
}