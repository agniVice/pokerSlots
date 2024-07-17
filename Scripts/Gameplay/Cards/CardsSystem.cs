using UnityEngine;

public class CardsSystem : MonoBehaviour
{
    public static CardsSystem Instance { get; private set; }

    public int[] _cards { get; private set; }
    public ElementType ElementType { get; private set; }

    [SerializeField] private int[] _maxCards;
    [SerializeField] private int[] _rewards;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;

        Initialize();
    }
    private void Initialize()
    {
        _cards = new int[System.Enum.GetNames(typeof(ElementType)).Length];
        for (int i = 0; i < _cards.Length; i++)
            _cards[i] = PlayerPrefs.GetInt("Card" + i, 0);
    }
    public void OnCardWin(ElementType type)
    {
        ElementType = type;

        int id = (int)type;

        if (_cards[id] >= _maxCards[id])
            return;
        _cards[id]++;

        Save();
        CheckCardForComplete(id);
    }
    private void CheckCardForComplete(int id)
    {
        if (_cards[id] >= _maxCards[id])
        {
            PlayerBalance.Instance.AddBalance(this, _rewards[id]);
            AudioManager.Instance.PlaySound(AudioManager.Instance.LevelUp, 1, 0.8f);
        }
        CardsUI.Instance.UpdateElements();
    }
    private void Save()
    {
        for (int i = 0; i < _cards.Length; i++)
            PlayerPrefs.SetInt("Card" + i, _cards[i]);
    }
    public int GetReward(int id) => _rewards[id];
    public string GetProgress(int id) => _cards[id] + "/" + _maxCards[id];
    public float GetProgressPercent(int id) => ((float)_cards[id] / _maxCards[id]);
}
