using System.Collections;
using UnityEngine;

public class PlayerBalance : MonoBehaviour
{
    public static PlayerBalance Instance;
    public int Balance { get; private set; }
    [SerializeField] private int _startBalance;

    [SerializeField] private int _rewardFromBonus;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        Balance = PlayerPrefs.GetInt("Balance", _startBalance);
    }
    public void AddBalance(object sender, int count)
    {
        if (sender.GetType() == typeof(SlotMachine) 
            || sender.GetType() == typeof(PlayerExperience)
            || sender.GetType() == typeof(CardsSystem)
            || sender.GetType() == typeof(LevelManager))
        {
            Balance += count;
            Save();
        }
    }
    public void RemoveBalance(object sender, int count)
    {
        if (sender.GetType() == typeof(SlotMachine) 
            || sender.GetType() == typeof(UpgradeSystem))
        {
            Balance -= count;
            Save();
        }
    }
    private void Save()
    {
        PlayerPrefs.SetInt("Balance", Balance);
    }
    public int BonusReward => _rewardFromBonus;
}