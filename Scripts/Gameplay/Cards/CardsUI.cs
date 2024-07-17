using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsUI : MonoBehaviour
{
    public static CardsUI Instance { get; private set; }

    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private Transform _window;
    [SerializeField] private Transform _endPos;
    [SerializeField] private Transform _startPos;

    [SerializeField] private TextMeshProUGUI[] _rewardsText;
    [SerializeField] private TextMeshProUGUI[] _progressText;
    [SerializeField] private Image[] _cardBars;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    public void UpdateElements()
    {
        for (int i = 0; i < _rewardsText.Length; i++)
        {
            _cardBars[i].fillAmount = CardsSystem.Instance.GetProgressPercent(i);
            _progressText[i].text = CardsSystem.Instance.GetProgress(i);
            int price = CardsSystem.Instance.GetReward(i);
            _rewardsText[i].text = "REWARD: " + price.ToString();

            if (_cardBars[i].fillAmount == 1)
                _rewardsText[i].text = "COMPLETED!";
        }
    }
    public void OpenCardWindow()
    {
        _panel.alpha = 0f;
        _window.transform.localPosition = _startPos.localPosition;
        _panel.blocksRaycasts = true;
        _panel.ignoreParentGroups = true;

        _panel.DOFade(1, 0.3f).SetLink(_panel.gameObject);
        _window.DOLocalMove(_endPos.localPosition, 0.4f).SetLink(_window.gameObject).SetEase(Ease.OutBack);

        UpdateElements();
    }
    public void CloseCardWindow()
    {
        _panel.blocksRaycasts = false;
        _panel.ignoreParentGroups = false;

        _panel.DOFade(0, 0.3f).SetLink(_panel.gameObject);
        _window.DOLocalMove(_startPos.localPosition, 0.4f).SetLink(_window.gameObject).SetEase(Ease.OutBack);
    }
}
