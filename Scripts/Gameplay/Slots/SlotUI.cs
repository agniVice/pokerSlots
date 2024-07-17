using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public static SlotUI Instance;

    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private TextMeshProUGUI _betText;
    [SerializeField] private TextMeshProUGUI _winText;

    [SerializeField] private Image _spinImage;
    [SerializeField] private Image _autoPlayImage;
    [SerializeField] private Image _maxBetImage;

    [SerializeField] private Button _increaseBetButton;
    [SerializeField] private Button _decreaseBetButton;
    [SerializeField] private Button _sortButton;

    [SerializeField] private Sprite _activeButton;
    [SerializeField] private Sprite _inactiveButton;

    [SerializeField] private Sprite _activeSpin;
    [SerializeField] private Sprite _inactiveSpin;

    [SerializeField] private CanvasGroup _winPanel;
    [SerializeField] private Transform _winTextStartPos;
    [SerializeField] private Transform _winTextEndPos;

    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private Transform _cardButton;
    [SerializeField] private Sprite[] _elementSprites;

    [SerializeField] private float _spinButtonTime;

    private int _lastBalance = 0;
    private int _currentBalance;
    private int _currentWin;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        else
            Instance = this;
    }
    private void Start()
    {
        UpdateBalance();
        UpdateBet();
    }
    public void OnPlayerWin(int value)
    {
        var card = Instantiate(_cardPrefab, _winPanel.transform.position, Quaternion.identity, _cardParent);
        card.GetComponent<SlotCardElement>().Initialize(_cardButton, _elementSprites[(int)CardsSystem.Instance.ElementType]);

        _currentWin = 0;
        _winPanel.alpha = 0;
        _winText.transform.localPosition = _winTextStartPos.localPosition;

        _winPanel.DOFade(1, 0.4f).SetLink(_winPanel.gameObject);
        _winPanel.DOFade(0, 0.4f).SetLink(_winPanel.gameObject).SetDelay(1.5f);

        DOTween.To(() => _currentWin, x => _currentWin = x, value, 0.5f)
            .OnUpdate(UpdateWinText)
            .OnKill(() => {
                UpdateBalance(0.3f);
                _winText.transform
                .DOLocalMove(_winTextEndPos.localPosition, 0.3f)
                .SetLink(_winText.gameObject); });

        AudioManager.Instance.PlaySound(AudioManager.Instance.SlotPostWin, 1f, 0.4f);
    }
    private void UpdateWinText()
    {
        _winText.text = "+" + _currentWin.ToString();
    }
    public void OpenSlot()
    {
        _panel.gameObject.SetActive(true);
    }
    public void CloseSlot()
    {
        _panel.gameObject.SetActive(false);
    }
    public void UpdateBet()
    {
        _increaseBetButton.interactable = true;
        _decreaseBetButton.interactable = true;

        if (SlotMachine.Instance.IsCurrentBetMax())
            _increaseBetButton.interactable = false;
        else
            _increaseBetButton.interactable = true;

        if (SlotMachine.Instance.IsCurrentBetMin())
            _decreaseBetButton.interactable = false;
        else
            _decreaseBetButton.interactable = true;

        _betText.text = "BET: " + SlotMachine.Instance.FinalBet.ToString();
    }
    public void UpdateBalance(float delay = 0f)
    {
        _currentBalance = _lastBalance;
        DOTween.To(() => _currentBalance, x => _currentBalance = x, PlayerBalance.Instance.Balance, 1f).SetEase(Ease.Linear).OnUpdate(UpdateBalanceText).SetDelay(delay);
    }
    private void UpdateBalanceText()
    { 
        _balanceText.text = _currentBalance.ToString();
        _lastBalance = PlayerBalance.Instance.Balance;
    }
    public void UpdateAutoSpin()
    {
        if (SlotMachine.Instance.AutoSpinnig)
            _autoPlayImage.sprite = _inactiveButton;
        else
            _autoPlayImage.sprite = _activeButton;
    }
    public void OnAutoSpinButtonClicked()
    {
        SlotMachine.Instance.ToggleAutoSpin();
        UpdateAutoSpin();
    }
    public void OnIncreaseBetButtonClicked()
    {
        SlotMachine.Instance.IncreaseBet();
        UpdateBet();
    }
    public void OnDecreaseBetButtonClicked()
    {
        SlotMachine.Instance.DecreaseBet();
        UpdateBet();
    }
    public void OnStartSpin()
    {
        _sortButton.interactable = false;
        _spinImage.sprite = _inactiveSpin;
        _spinImage.transform.DORotate(new Vector3(0, 0, 540f), _spinButtonTime, RotateMode.FastBeyond360).SetEase(Ease.InOutBack);
    }
    public void OnEndSpin()
    {
        _sortButton.interactable = true;
        _spinImage.sprite = _activeSpin;
    }
    public void OnMenuButtonClicked()
    {
        SlotMachine.Instance.DisableAutoSpin();
        if (SlotMachine.Instance.CanOpenMenu())
            FindObjectOfType<MenuUserInterface>().OpenMenu();
    }
    public void OnSpinButtonClicked() => SlotMachine.Instance.Spin();
    public void OnMaxBetButtonClicked()
    {
        SlotMachine.Instance.SetMaxBet();
        UpdateBet();
    }
}
