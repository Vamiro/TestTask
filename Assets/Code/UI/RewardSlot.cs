using Code.Inventory;
using Code.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.UI
{
    public class RewardSlot : MonoBehaviour, IDataView<RewardSlot.Data>, IReceiveLongTap
    {
        public class Data
        {
            public InventoryItem Item;
        }

        [SerializeField] private Image _itemImage;
        [SerializeField] private Tooltip _tooltip;
        private Data _data;

        public void OnReceiveLongTap()
        {
            if (_data != null && _data.Item != null)
                _tooltip.Show(_data.Item.Title, _data.Item.Description, _data.Item.Icon);
        }

        public void OnReleaseLongTap()
        {
            _tooltip.Hide();
        }

        public void SetData(Data data)
        {
            _data = data;
            
            if (_itemImage)
                _itemImage.sprite = data.Item.Icon;
        }
    }
}