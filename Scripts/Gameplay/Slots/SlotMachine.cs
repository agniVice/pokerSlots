using System;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    public static SlotMachine Instance;
    public int FinalBet { get; private set; }
    public int PlayBet { get; private set; }
    public bool AutoSpinnig { get; private set; }

    [SerializeField] private int _wildChance;
    [SerializeField] private int _defaultWinChance;
    [SerializeField] private int _minBalanceForWin;

    [SerializeField] private int[] _bets;
    [SerializeField] private int _defaultWin;
    [SerializeField] private int _wildWin;

    [SerializeField] private SlotDrum[] _drums;

    private int _currentBet;

    private int _winChance;

    private bool _isSpinning;
    private bool[] _canSpin = { true, true, true };


    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(Instance);
        else
            Instance = this;

        FinalBet = _bets[_currentBet];
    }
    private void FixedUpdate()
    {
        CheckForSpin();
    }
    private void CheckForSpin()
    {
        if (AutoSpinnig)
        {
            if (!_isSpinning)
            {
                if (_canSpin[0] && _canSpin[1] && _canSpin[2])
                {
                    if (PlayerBalance.Instance.Balance >= _bets[_currentBet])
                        Spin();
                    else
                        ToggleAutoSpin();
                }
            }
        }
        if (_isSpinning)
        {
            if (_canSpin[0] && _canSpin[1] && _canSpin[2])
                OnEndSpin();
        }
    }
    private void CheckForWin()
    {
        ElementType type = _drums[0].GetMainElement();
        bool elementsSame = true;
        for (int i = 0; i < _drums.Length; i++)
        {
            if (_drums[i].GetMainElement() != type)
            {
                elementsSame = false;
            }
        }
        if (elementsSame)
        {
            int reward = 0;
            if (type == ElementType.Wild)
                reward = PlayBet * _wildWin;
            else
                reward = PlayBet * _defaultWin;

            CardsSystem.Instance.OnCardWin(type);
            PlayerExperience.Instance.AddExperience();
            PlayerBalance.Instance.AddBalance(this, reward);
            SlotUI.Instance.OnPlayerWin(reward);
        }
    }
    private void UpdateWinChance()
    {
        _winChance = _defaultWinChance + PlayerPrefs.GetInt("WinChanceUpgrade", 0);
    }
    public void SetMaxBet()
    {
        FinalBet = _bets[_bets.Length-1];
        SlotUI.Instance.UpdateBet();
    }
    private void OnEndSpin()
    {
        CheckForWin();
        _isSpinning = false;
        SlotUI.Instance.OnEndSpin();
    }
    public void IncreaseBet()
    {
        if (_currentBet != _bets.Length - 1)
            _currentBet++;
        FinalBet = _bets[_currentBet];
    }
    public void DecreaseBet()
    {
        if (_currentBet != 0)
            _currentBet--;
        FinalBet = _bets[_currentBet];
    }
    public void ToggleAutoSpin()
    {
        AutoSpinnig = !AutoSpinnig;
        SlotUI.Instance.UpdateAutoSpin();
    }
    public void Spin()
    {
        if (_isSpinning)
            return;
        if (PlayerBalance.Instance.Balance < FinalBet)
        {
            if (AutoSpinnig)
                ToggleAutoSpin();
            return;
        }
        if (_canSpin[0] && _canSpin[1] && _canSpin[2])
        {
            PlayBet = FinalBet;

            UpdateWinChance();

            for (int i = 0; i < _canSpin.Length; i++)
                _canSpin[i] = false;

            if (IsWinGame())
            {
                ElementType type = (ElementType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(ElementType)).Length);
                float spinDelay = 0f;
                for (int i = 0; i < _drums.Length; i++)
                {
                    _drums[i].OnDrumStartWinSpin(spinDelay, type);
                    spinDelay += 0.15f;
                }
            }
            else
            {
                float spinDelay = 0f;
                for (int i = 0; i < _drums.Length; i++)
                {
                    _drums[i].OnDrumStartSpin(spinDelay);
                    spinDelay += 0.15f;
                }
            }

            AudioManager.Instance.PlaySound(AudioManager.Instance.Spin, 1f, 1f);
            PlayerBalance.Instance.RemoveBalance(this, FinalBet);
            SlotUI.Instance.UpdateBalance();

            _isSpinning = true;

            SlotUI.Instance.OnStartSpin();
        }
    }
    public void DisableAutoSpin()
    {
        if (AutoSpinnig)
            ToggleAutoSpin();
    }
    private bool IsWinGame() => (UnityEngine.Random.Range(0, 100) <= _winChance || PlayerBalance.Instance.Balance <= _minBalanceForWin);
    public bool IsCurrentBetMax() => _currentBet == (_bets.Length - 1);
    public bool IsCurrentBetMin() => _currentBet == 0;
    public float GetWildChance() => _wildChance;
    public void ReadyForSpin(int id) => _canSpin[id] = true;
    public bool CanOpenMenu() => (_canSpin[0] && _canSpin[1] && _canSpin[2]);
    public int GetDrumIndex(SlotDrum drum) => Array.IndexOf(_drums, drum);
    
}
