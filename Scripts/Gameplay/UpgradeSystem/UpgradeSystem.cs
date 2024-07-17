using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;
    public int[] _upgrades { get; private set; }

    [SerializeField] private int[] _maxUpgrades;
    [SerializeField] private int[] _prices;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;

        Initialize();
    }
    private void Initialize()
    {
        _upgrades = new int[3];

        _upgrades[0] = PlayerPrefs.GetInt("WinChanceUpgrade", 0);
        _upgrades[1] = PlayerPrefs.GetInt("ExperienceUpgrade", 0);
        _upgrades[2] = PlayerPrefs.GetInt("BonusTimeUpgrade", 0);
    }
    private void Save()
    {
        PlayerPrefs.SetInt("WinChanceUpgrade", _upgrades[0]);
        PlayerPrefs.SetInt("ExperienceUpgrade", _upgrades[1]);
        PlayerPrefs.SetInt("BonusTimeUpgrade", _upgrades[2]);
    }
    public void UpgradeSomething(int upgradeId)
    {
        if (_upgrades[upgradeId] >= _maxUpgrades[upgradeId])
            return;
        if (_prices[_upgrades[upgradeId]] > PlayerBalance.Instance.Balance)
            return;

        AudioManager.Instance.PlaySound(AudioManager.Instance.LevelUp, 1, 0.8f);
        PlayerBalance.Instance.RemoveBalance(this, _prices[_upgrades[upgradeId]]);
        SlotUI.Instance.UpdateBalance();
        _upgrades[upgradeId]++;

        Save();
        UpgradeUI.Instance.UpdateElements();
    }
    public int GetPrice(int id)
    {
        if (_upgrades[id] >= _maxUpgrades[id])
            return -1;
        else
            return _prices[_upgrades[id]];
    }   
    public float GetUpgradePercent(int id) => ((float)_upgrades[id] / _maxUpgrades[id]);
}
