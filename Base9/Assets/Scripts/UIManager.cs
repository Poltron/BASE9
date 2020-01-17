using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;
    
    [SerializeField]
    private Image Player1Image;
    [SerializeField]
    private Image Player2Image;
    [SerializeField]
    private TextMeshProUGUI Player1Name;
    [SerializeField]
    private TextMeshProUGUI Player2Name;

    [SerializeField]
    private TextMeshProUGUI Player1Purse;
    [SerializeField]
    private TextMeshProUGUI Player2Purse;

    [SerializeField]
    private TextMeshProUGUI Dice1Text;
    [SerializeField]
    private TextMeshProUGUI Dice2Text;
    [SerializeField]
    private TextMeshProUGUI Dice3Text;

    [SerializeField]
    private Image Dice1Image;
    [SerializeField]
    private Image Dice2Image;
    [SerializeField]
    private Image Dice3Image;

    [SerializeField]
    private Sprite Dice1Sprite;
    [SerializeField]
    private Sprite Dice2Sprite;
    [SerializeField]
    private Sprite Dice3Sprite;
    [SerializeField]
    private Sprite Dice4Sprite;
    [SerializeField]
    private Sprite Dice5Sprite;
    [SerializeField]
    private Sprite Dice6Sprite;

    [SerializeField]
    private TextMeshProUGUI Bank1;
    [SerializeField]
    private TextMeshProUGUI Bank2;
    [SerializeField]
    private TextMeshProUGUI Bank3;
    [SerializeField]
    private TextMeshProUGUI Bank4;
    [SerializeField]
    private TextMeshProUGUI Bank5;

    [SerializeField]
    private TextMeshProUGUI PhaseText;

    [SerializeField]
    private TextMeshProUGUI Winner;
    [SerializeField]
    private Color WinColor;
    [SerializeField]
    private Color LooseColor;

    public Button PlayDiceButton;
    public Button PlayBonusDiceButton;
    public Button EndTurnButton;

    void Start()
    {

    }

    void Update()
    {
        PhaseText.text = GameManager.IsPhase2() ? "Phase 2" : "Phase 1";

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

        Bank1.text = ShowBankText(1);
        Bank2.text = ShowBankText(2);
        Bank3.text = ShowBankText(3);
        Bank4.text = ShowBankText(4);
        Bank5.text = ShowBankText(5);

        int dice = GameManager.GetDice(1);
        if (dice != 0)
        {
            Dice1Image.gameObject.SetActive(true);
            Dice1Text.text = GameManager.GetDice(1).ToString();
            Dice1Image.sprite = ShowDiceSprite(dice);
        }
        else
        {
            Dice1Image.gameObject.SetActive(false);
            Dice1Text.text = "";
        }

        dice = GameManager.GetDice(2);
        if (dice != 0)
        {
            Dice2Image.gameObject.SetActive(true);
            Dice2Text.text = GameManager.GetDice(2).ToString();
            Dice2Image.sprite = ShowDiceSprite(dice);
        }
        else
        {
            Dice2Image.gameObject.SetActive(false);
            Dice2Text.text = "";
        }

        dice = GameManager.GetDice(3);
        if (dice != 0)
        {
            Dice3Image.gameObject.SetActive(true);
            Dice3Text.text = GameManager.GetDice(3).ToString();
            Dice3Image.sprite = ShowDiceSprite(dice);
        }
        else
        {
            Dice3Image.gameObject.SetActive(false);
            Dice3Text.text = "";
        }
    }

    public void SetWinnerLooser(Player playerWinner, Player playerLooser)
    {
        Winner.text = playerWinner.PlayerName + "wins";

        if (playerWinner.IsLocal)
            Winner.color = WinColor;
        else
            Winner.color = LooseColor;
    }

    public void EnablePlayDiceButton(Player player, bool value)
    {
        PlayDiceButton.gameObject.SetActive(value);

        if (value)
            PlayDiceButton.onClick.AddListener(player.PlayDice);
        else
            PlayDiceButton.onClick.RemoveListener(player.PlayDice);
    }

    public void EnablePlayBonusDiceButton(Player player, bool value)
    {
        PlayBonusDiceButton.gameObject.SetActive(value);

        if (value)
            PlayBonusDiceButton.onClick.AddListener(player.PlayBonusDice);
        else
            PlayBonusDiceButton.onClick.RemoveListener(player.PlayBonusDice);
    }

    public void EnableEndTurnButton(Player player, bool value)
    {
        EndTurnButton.gameObject.SetActive(value);

        if (value)
            EndTurnButton.onClick.AddListener(player.EndTurn);
        else
            EndTurnButton.onClick.RemoveListener(player.EndTurn);
    }

    private string ShowBankText(int BankIndex)
    {
        if (GameManager.IsPhase2())
        {
            if (GameManager.GetBank(BankIndex) == 0)
            {
                return "X";
            }
        }

        return GameManager.GetBank(BankIndex).ToString();
    }

    private Sprite ShowDiceSprite(int diceNumber)
    {
        switch(diceNumber)
        {
            case 1:
                return Dice1Sprite;
            case 2:
                return Dice2Sprite;
            case 3:
                return Dice3Sprite;
            case 4:
                return Dice4Sprite;
            case 5:
                return Dice5Sprite;
            case 6:
                return Dice6Sprite;
            default:
                return null;
        }
    }
}
