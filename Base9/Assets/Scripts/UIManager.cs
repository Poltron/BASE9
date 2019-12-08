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
    private Image Player1Image;
    [SerializeField]
    private Image Player2Image;
    [SerializeField]
    private Text Player1Name;
    [SerializeField]
    private Text Player2Name;

    [SerializeField]
    private Text Player1Purse;
    [SerializeField]
    private Text Player2Purse;

    [SerializeField]
    private Text Dice1Text;
    [SerializeField]
    private Text Dice2Text;
    [SerializeField]
    private Text Dice3Text;

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
        if (GameManager.ActivePlayerNumber == 1)
        {
            Player1Image.color = new Color(Player1Image.color.r, Player1Image.color.g, Player1Image.color.b, 1);
            Player2Image.color = new Color(Player2Image.color.r, Player2Image.color.g, Player2Image.color.b, 0);
        }
        else
        {
            Player1Image.color = new Color(Player1Image.color.r, Player1Image.color.g, Player1Image.color.b, 0);
            Player2Image.color = new Color(Player2Image.color.r, Player2Image.color.g, Player2Image.color.b, 1);
        }

        if (GameManager.Player1 != null)
            Player1Name.text = GameManager.Player1.PlayerName;
        if (GameManager.Player2 != null)
            Player2Name.text = GameManager.Player2.PlayerName;

        if (GameManager.Player1 != null)
            Player1Purse.text = GameManager.GetPurse(1).ToString();
        if (GameManager.Player2 != null)
            Player2Purse.text = GameManager.GetPurse(2).ToString();

        Bank1.text = GameManager.GetBank(1).ToString();
        Bank2.text = GameManager.GetBank(2).ToString();
        Bank3.text = GameManager.GetBank(3).ToString();
        Bank4.text = GameManager.GetBank(4).ToString();
        Bank5.text = GameManager.GetBank(5).ToString();

        int dice = GameManager.GetDice(1);
        if (dice != 0)
            Dice1Text.text = GameManager.GetDice(1).ToString();
        else
            Dice1Text.text = "";

        dice = GameManager.GetDice(2);
        if (dice != 0)
            Dice2Text.text = GameManager.GetDice(2).ToString();
        else
            Dice2Text.text = "";

        dice = GameManager.GetDice(3);
        if (dice != 0)
            Dice3Text.text = GameManager.GetDice(3).ToString();
        else
            Dice3Text.text = "";
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

    public void EnableInputUI(Player player)
    {
        PlayUI.SetActive(true);
        
        PlayDiceButton.onClick.AddListener(player.PlayDice);
        PlayBonusDiceButton.onClick.AddListener(player.PlayBonusDice);
        EndTurnButton.onClick.AddListener(player.EndTurn);
    }

    public void DisableInputUI(Player player)
    {
        PlayUI.SetActive(false);

        PlayDiceButton.onClick.RemoveListener(player.PlayDice);
        PlayBonusDiceButton.onClick.RemoveListener(player.PlayBonusDice);
        EndTurnButton.onClick.RemoveListener(player.EndTurn);
    }
}
