using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;

    [SerializeField]
    private GameObject PlayUI;
    [SerializeField]
    private GameObject PauseUI;
    [SerializeField]
    private GameObject EndUI;

    [SerializeField]
    private Text ActivePlayer;
    [SerializeField]
    private Text Dice1Text;
    [SerializeField]
    private Text Dice2Text;
    [SerializeField]
    private Text Dice3Text;
    [SerializeField]
    private Text Player1Purse;
    [SerializeField]
    private Text Player2Purse;
    [SerializeField]
    private Text Bank1;
    [SerializeField]
    private Text Bank2;
    [SerializeField]
    private Text Bank3;
    [SerializeField]
    private Text Bank4;
    [SerializeField]
    private Text Bank5;

    [SerializeField]
    private Text Winner;
    [SerializeField]
    private Text Looser;

    public Button PlayDiceButton;
    public Button PlayBonusDiceButton;
    public Button EndTurnButton;

    void Start()
    {

    }

    void Update()
    {
        ActivePlayer.text = GameManager.ActivePlayer.PlayerName;
        Player1Purse.text = GameManager.Player1.Purse.ToString();
        Player2Purse.text = GameManager.Player2.Purse.ToString();
        Bank1.text = GameManager.GetBank(1).ToString();
        Bank2.text = GameManager.GetBank(2).ToString();
        Bank3.text = GameManager.GetBank(3).ToString();
        Bank4.text = GameManager.GetBank(4).ToString();
        Bank5.text = GameManager.GetBank(5).ToString();
        Dice1Text.text = GameManager.GetDice(1).ToString();
        Dice2Text.text = GameManager.GetDice(2).ToString();
        Dice3Text.text = GameManager.GetDice(3).ToString();
    }

    public void TogglePausePanel()
    {
        PauseUI.SetActive(!PauseUI.activeInHierarchy);
    }

    public void ShowEndPanel(bool on)
    {
        EndUI.SetActive(on);
    }

    public void SetWinnerLooser(Player winner, Player looser)
    {
        Winner.text = winner.PlayerName + "wins";
        Looser.text = looser.PlayerName + "looses";
    }

    public void HideDices()
    {
        Dice1Text.gameObject.SetActive(false);
        Dice2Text.gameObject.SetActive(false);
        Dice3Text.gameObject.SetActive(false);
    }

    public void EnableInputUI(Player player)
    {
        PlayUI.SetActive(true);
        
        PlayDiceButton.onClick.AddListener(GameManager.ActivePlayerThrowDice);
        PlayBonusDiceButton.onClick.AddListener(GameManager.ActivePlayerThrowBonusDice);
        EndTurnButton.onClick.AddListener(GameManager.ActivePlayerEndTurn);
    }

    public void DisableInputUI(Player player)
    {
        PlayUI.SetActive(false);

        PlayDiceButton.onClick.RemoveListener(GameManager.ActivePlayerThrowDice);
        PlayBonusDiceButton.onClick.RemoveListener(GameManager.ActivePlayerThrowBonusDice);
        EndTurnButton.onClick.RemoveListener(GameManager.ActivePlayerEndTurn);
    }

    public void ShowDice(int number, int value)
    {
        switch (number)
        {
            case 1:
                Dice1Text.gameObject.SetActive(true);
                Dice1Text.text = value.ToString();
                break;
            case 2:
                Dice2Text.gameObject.SetActive(true);
                Dice2Text.text = value.ToString();
                break;
            case 3:
                Dice3Text.gameObject.SetActive(true);
                Dice3Text.text = value.ToString();
                break;
        }
    }
}
