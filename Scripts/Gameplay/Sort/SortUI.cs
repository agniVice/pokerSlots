using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SortUI : MonoBehaviour
{
    public static SortUI Instance { get; private set; }

    public Transform ElementsStartPosition;
 
    [SerializeField] private CanvasGroup _winPanel;
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private TextMeshProUGUI _timerText;

    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private TextMeshProUGUI _rewardText;

    [SerializeField] private Image _timerBackground;
    [SerializeField] private Transform _timerObject;

    [SerializeField] private Transform _timerStartPos;
    [SerializeField] private Transform _timerEndPos;


    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(Instance);
        else
            Instance = this;
    }
    private void FixedUpdate()
    {
        if (SortTimer.Instance != null)
            _timerText.text = Mathf.RoundToInt(SortTimer.Instance.CurrentTime) + " SEC";
    }
    public void UpdateUI()
    {
        _balanceText.text = "BALANCE: " + PlayerBalance.Instance.Balance.ToString();
        _rewardText.text = "+" + LevelManager.Instance.LevelReward.ToString();
        _levelText.text = "LEVEL " + LevelManager.Instance.Level.ToString();
    }
    public void OpenSort()
    {
        _panel.alpha = 0f;
        _panel.DOFade(1f, 0.2f).SetLink(_panel.gameObject);
        _panel.gameObject.SetActive(true);

        _timerObject.position = _timerStartPos.position;
        _timerBackground.color = new Color32(0, 0, 0, 119);
        _timerBackground.DOFade(0, 0.3f).SetDelay(1f).SetLink(_timerBackground.gameObject);
        _timerObject.localScale = new Vector3(2,2,2);
        _timerObject.DOScale(1, 0.3f).SetDelay(1f).SetLink(_timerObject.gameObject);
        _timerObject.DOMove(_timerEndPos.position, 0.3f).SetDelay(1f).SetLink(_timerEndPos.gameObject);
        SlotUI.Instance.CloseSlot();
        LevelManager.Instance.InitializeLevel();
        UpdateUI();
    }
    public void CloseSort() 
    {
        SortTimer.Instance.StopTimer();
        SlotUI.Instance.UpdateBalance();
        _panel.DOFade(0f, 0.2f).SetLink(_panel.gameObject);
        //_panel.gameObject.SetActive(false);
        UpdateUI();
    }
    public void OnGameCompleted()
    {
        _winPanel.alpha = 0f;
        _winPanel.DOFade(1, 0.5f).SetLink(_winPanel.gameObject);
        _winPanel.DOFade(0, 0.5f).SetLink(_winPanel.gameObject).SetDelay(2.5f).OnKill(() => 
        {
            SlotUI.Instance.OpenSlot();
            CloseSort();
        });
        _infoText.text = "LEVEL COMPLETED";
        _winText.text = "+" + LevelManager.Instance.LevelReward.ToString();
    }
    public void OnGameOver()
    {
        _winPanel.alpha = 0f;
        _winPanel.DOFade(1, 0.5f).SetLink(_winPanel.gameObject);
        _winPanel.DOFade(0, 0.5f).SetLink(_winPanel.gameObject).SetDelay(2.5f).OnKill(() =>
        {
            SlotUI.Instance.OpenSlot();
            CloseSort();
        });
        _infoText.text = "LEVEL FAILED";
        _winText.text = "";
    }
}
