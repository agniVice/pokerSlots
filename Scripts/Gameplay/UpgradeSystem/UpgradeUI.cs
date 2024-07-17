using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private Transform _window;
    [SerializeField] private Transform _endPos;
    [SerializeField] private Transform _startPos;

    [SerializeField] private Button[] _upgradeButtons;
    [SerializeField] private TextMeshProUGUI[] _upgradePriceTexts;
    [SerializeField] private Image[] _upgradeBars;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        Instance = this;
    }
    public void UpdateElements()
    {
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeBars[i].fillAmount = UpgradeSystem.Instance.GetUpgradePercent(i);

            int price = UpgradeSystem.Instance.GetPrice(i);
            _upgradePriceTexts[i].text = price.ToString();

            if (price >= PlayerBalance.Instance.Balance)
                _upgradeButtons[i].interactable = true;
            else if (price == -1)
            {
                _upgradeButtons[i].interactable = false;
                _upgradePriceTexts[i].text = "MAX";
            }
        }
    }
    public void OpenUpgradeWindow()
    {
        _panel.alpha = 0f;
        _window.transform.localPosition = _startPos.localPosition;
        _panel.blocksRaycasts = true;
        _panel.ignoreParentGroups = true;

        _panel.DOFade(1, 0.3f).SetLink(_panel.gameObject);
        _window.DOLocalMove(_endPos.localPosition, 0.4f).SetLink(_window.gameObject).SetEase(Ease.OutBack);

        UpdateElements();
    }
    public void CloseUpgradeWindow() 
    {
        _panel.blocksRaycasts = false;
        _panel.ignoreParentGroups = false;

        _panel.DOFade(0, 0.3f).SetLink(_panel.gameObject);
        _window.DOLocalMove(_startPos.localPosition, 0.4f).SetLink(_window.gameObject).SetEase(Ease.OutBack);
    }
}
