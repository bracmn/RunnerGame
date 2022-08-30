using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool NewGame = true;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] public int Score = 0;
    public int TotalScore = 0;
    public bool isBoostActive = false;
    private int HpCost = 300;
    private int BoostCost = 200;
    [SerializeField] public int Health = 3;
    [SerializeField] private Text UIScoreText;
    [SerializeField] private Text UILevelText;
    [SerializeField] private Animator UIScoreAnimator;
    [SerializeField] private Animator HP1;
    [SerializeField] private Animator HP2;
    [SerializeField] private Animator HP3;
    [SerializeField] private Animator HP4;
    [Space]
    [SerializeField] private TextMeshProUGUI TotalScoreText;
    [Space]
    [SerializeField] private TextMeshProUGUI HpCostText;
    [SerializeField] private TextMeshProUGUI BoostCostText;
    [SerializeField] private TextMeshProUGUI BuyHpErrorText;
    [SerializeField] private TextMeshProUGUI BuyBoostErrorText;
    [Space]
    [SerializeField] public GameObject RestartButton;
    [SerializeField] public GameObject NextLevelButton;
    [SerializeField] public TextMeshProUGUI StartButtonText;
    private void Awake()
    {
        Instance = this;
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Ready:
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.Victory:
                NewGame = false;
                TotalScore += Score;
                Score = 0;
                TotalScoreText.text = TotalScore.ToString();
                StartButtonText.text = $"Press Here to Start\nLevel: {++LevelManager.Instance.CurrentLevel}";
                UILevelText.text = $"Level: {LevelManager.Instance.CurrentLevel}";
                isBoostActive = false;
                NextLevelButton.SetActive(true);
                LevelManager.Instance.RoadLenght += 5;
                
                break;
            case GameState.Lose:
                NewGame = true;
                RestartButton.SetActive(true);
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
        return;
    }
    public void ReturnToReady()
    {
        ResetAll();
        return;
    }
    public void StartOrContinueGame()
    {
        if(State != GameState.PlayerTurn)
            UpdateGameState(GameState.PlayerTurn);
    }
    public IEnumerator AddScore(int score)
    {
        if (isBoostActive)
            score += score * 2;
        UIScoreAnimator.Play("ScoreTextAddPoints");

        //This loop is not a good solution.
        for (int i = 0; i<score;)
        {
            //This control block is very static. Only
            if(score >= 50)
            {
                Score+=10;
                i+=10;
            }
            else
            {
                Score++;
                i++;
            }
            UIScoreText.text = Score.ToString();
            yield return new WaitForSeconds(.1f);
        }
    }
    public void HPLoss()
    {
        if(Health == 4)
        {
            Health--;
            HP4.Play("HpLoss");
        }
        else if(Health == 3)
        {
            Health--;
            HP3.Play("HpLoss");
        }
        else if(Health == 2)
        {
            Health--;
            HP2.Play("HpLoss");
        }
        else
        {
            Health--;
            HP1.Play("HpLoss");
        }
    }

    public enum GameState
    {
        Ready,
        PlayerTurn,
        Victory,
        Lose
    }

    #region [ UI Buy Section ]
    
    public void BuyHp()
    {
        if(TotalScore < HpCost)
        {
            BuyHpErrorText.text = "You don't have enough points";
            BuyHpErrorText.gameObject.SetActive(true);
            BuyBoostErrorText.gameObject.SetActive(false);
            return;
        }
        if(Health == 4)
        {
            BuyHpErrorText.text = "You have max health possible";
            BuyHpErrorText.gameObject.SetActive(true);
            BuyBoostErrorText.gameObject.SetActive(false);
            return;
        }
        Health++;
        TotalScore -= HpCost;
        TotalScoreText.text = TotalScore.ToString();
        if (Health == 2)
            SetHPBackSingle(HP2.gameObject);
        if (Health == 3)
            SetHPBackSingle(HP3.gameObject);
        if (Health == 4)
            SetHPBackSingle(HP4.gameObject);

        BuyBoostErrorText.gameObject.SetActive(false);
        BuyHpErrorText.gameObject.SetActive(false);
        HpCost += 10;
        HpCostText.text = $"Cost: {HpCost} points";
    }

    public void BuyBoost()
    {
        if (TotalScore < BoostCost)
        {
            BuyBoostErrorText.text = "You don't have enough points";
            BuyBoostErrorText.gameObject.SetActive(true);
            BuyHpErrorText.gameObject.SetActive(false);
            return;
        }
        if (isBoostActive)
        {
            BuyBoostErrorText.text = "You already have boost for next level";
            BuyBoostErrorText.gameObject.SetActive(true);
            BuyHpErrorText.gameObject.SetActive(false);
            return;
        }
        isBoostActive = true;
        TotalScore -= BoostCost;
        TotalScoreText.text = TotalScore.ToString();
        BuyBoostErrorText.gameObject.SetActive(false);
        BuyHpErrorText.gameObject.SetActive(false);
        BoostCost += 5;
        BoostCostText.text = $"Cost: {BoostCost} points";
    }

    #endregion

    #region [ ResetGameState ]

    private void ResetAll()
    {
        Score = 0;
        UIScoreText.text = Score.ToString();
        //TotalScore = 0;
        Health = 3;
        HP1.gameObject.SetActive(true);
        HP2.gameObject.SetActive(true);
        HP3.gameObject.SetActive(true);
        HP4.gameObject.SetActive(false);
        SetHPColors();
        isBoostActive = false;
        //LevelManager.Instance.CurrentLevel = 1;
        StartButtonText.text = $"Press Here to Start\nLevel: {LevelManager.Instance.CurrentLevel}";
        foreach (var pool in ObjectPooler.Instance.poolDictionary)
        {
            foreach (var item in pool.Value)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void SetHPColors()
    {
        Image image = HP1.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        image = HP2.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        image = HP3.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        image = HP4.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }

    private void SetHPBackSingle(GameObject HP)
    {
        Image image = HP.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        HP.SetActive(true);
    }

    #endregion

}