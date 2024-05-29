using System.Collections.Generic;
using System.Linq;
using Code.Inventory;
using Code.UI;
using Code.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsWindow : BaseWindow
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _rewardsText;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private SimpleList<RewardSlot, RewardSlot.Data> _rewardSlotList;

    private LongTap _longTap;

    protected override void Awake()
    {
        base.Awake();
        _closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        if (GetComponent<LongTap>() == null)
        {
            _longTap = gameObject.AddComponent<LongTap>();
            _longTap.OnLongTap += () => _scrollRect.enabled = false;
            _longTap.OnLongTapEnd += () => _scrollRect.enabled = true;
        }
    }

    protected override void OnShow(object[] args)
    {
        _titleText.text = (string)args[0];

        var items = (List<InventoryItem>)args[1];
        _rewardSlotList.SetData(items.Select(s => new RewardSlot.Data { Item = s }).ToList());

        _scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    protected override void OnHide()
    {
        _rewardSlotList.Clear();
    }
}