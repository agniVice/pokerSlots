using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public static PlayerExperience Instance { get; private set; }

    public float Experience { get; private set; }
    public float MaxExperience {  get; private set; }
    public int Level { get; private set; }
    public int Reward { get; private set; }

    [SerializeField] private float _defaultExperience;
    [SerializeField] private float _winExperience;

    [SerializeField] private float _experienceForLevel;
    [SerializeField] private int _rewardForLevel;
    [SerializeField] private int _defaultReward;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        Level = PlayerPrefs.GetInt("Level", 1);
        Experience = PlayerPrefs.GetFloat("Experience", 0);
        Reward = _defaultReward + (_rewardForLevel * Level);
        MaxExperience = _defaultExperience + (_experienceForLevel * Level);

        ExperienceUI.Instance.UpdateInfo();
    }
    public void AddExperience()
    {
        Experience += _winExperience + PlayerPrefs.GetInt("ExperienceUpgrade", 0) * (_winExperience/2);
        if (Experience >= _defaultExperience + (_experienceForLevel * Level))
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.LevelUp, 1f, 0.6f);
            Level++;
            Experience = 0;
            GiveReward();
        }
        Save();
        ExperienceUI.Instance.UpdateInfo();
    }
    private void GiveReward()
    {
        Reward = _defaultReward + (_rewardForLevel * Level);
        PlayerBalance.Instance.AddBalance(this, Reward);
    }
    private void Save()
    {
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetFloat("Experience", Experience);
    }
}
