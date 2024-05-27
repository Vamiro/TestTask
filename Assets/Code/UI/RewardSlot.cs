using Code.Inventory;
using Code.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class RewardSlot : MonoBehaviour, IDataView<RewardSlot.Data>, IPressable
    {
        public class Data
        {
            public InventoryItem Item;
        }

        [SerializeField] private Image _itemImage;
        [SerializeField] private Tooltip tooltip;

        private Data _data;

        public void OnPress()
        {
            if (_data != null && _data.Item != null)
                tooltip.Show(_data.Item.Title, _data.Item.Description, _data.Item.Icon);
        }

        public void OnRelease()
        {
            tooltip.Hide();
        }

        public void SetData(Data data)
        {
            _data = data;
            if (_itemImage) _itemImage.sprite = data.Item.Icon;
        }
    }
}