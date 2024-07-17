using UnityEngine;

public class SortTimer : MonoBehaviour
{
    public static SortTimer Instance { get; private set; }

    public float CurrentTime { get; private set; }

    private bool _isTimerOn;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    private void FixedUpdate()
    {
        if (_isTimerOn)
        {
            if (CurrentTime > 0)
                CurrentTime -= Time.fixedDeltaTime;
            else
            {
                StopTimer();
                AudioManager.Instance.PlaySound(AudioManager.Instance.LevelUp, 0.6f);
                LevelManager.Instance.OnGameFailed();
            }
        }
    }
    public void StartTimer(float time)
    {
        CurrentTime = time + PlayerPrefs.GetInt("BonusTimeUpgrade", 0);
        _isTimerOn = true;
    }
    public void StopTimer()
    { 
        _isTimerOn = false;
    }
}
