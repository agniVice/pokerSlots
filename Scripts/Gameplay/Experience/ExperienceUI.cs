using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    public static ExperienceUI Instance { get; private set; }

    [SerializeField] private Image _experienceBar;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _rewardText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    public void UpdateInfo()
    {
        _experienceBar.fillAmount = PlayerExperience.Instance.Experience / PlayerExperience.Instance.MaxExperience;
        _levelText.text = "LEVEL " + PlayerExperience.Instance.Level.ToString();
        _rewardText.text = "+" + PlayerExperience.Instance.Reward.ToString();
    }
}
