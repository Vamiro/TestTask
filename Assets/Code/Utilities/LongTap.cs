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
        private IReceiveLongTap _object;

        public event UnityAction OnLongTap;
        public event UnityAction OnLongTapEnd;

        private void Update()
        {
            if (_isPressed && Time.time - _timer >= _delay)
            {
                _isPressed = false;
                _object?.OnReceiveLongTap();
                OnLongTap?.Invoke();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - _timer >= _delay)
                {
                    _object?.OnReleaseLongTap();
                    OnLongTapEnd?.Invoke();
                }

                _object = null;
                _isPressed = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isPressed && eventData.hovered.Count != 0)
            {
                _object = eventData.hovered.Find(x => x.GetComponent<IReceiveLongTap>() != null).GetComponent<IReceiveLongTap>();
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