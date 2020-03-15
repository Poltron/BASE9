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
    private TextMeshProUGUI Player1Name;
    [SerializeField]
    private TextMeshProUGUI Player1Purse;
    [SerializeField]
    private GameObject Player1Light;

    [SerializeField]
    private TextMeshProUGUI Player2Purse;
    [SerializeField]
    private Image Player2Image;
    [SerializeField]
    private TextMeshProUGUI Player2Name;
    [SerializeField]
    private GameObject Player2Light;

    [SerializeField]
    private TextMeshPro Bank1;
    [SerializeField]
    private TextMeshPro Bank2;
    [SerializeField]
    private TextMeshPro Bank3;
    [SerializeField]
    private TextMeshPro Bank4;
    [SerializeField]
    private TextMeshPro Bank5;

    /*[SerializeField]
    private RectTransform Dice1Position;
    [SerializeField]
    private RectTransform Dice2Position;*/

    [SerializeField]
    private GameObject Operation;
    [SerializeField]
    private TextMeshProUGUI DiceTotal;
    [SerializeField]
    private TextMeshProUGUI OperationTotal;

    [SerializeField]
    private TextMeshProUGUI Dice1Text;
    [SerializeField]
    private TextMeshProUGUI Dice2Text;
    [SerializeField]
    private TextMeshProUGUI Dice3Text;

    [SerializeField]
    private Transform Dice1;
    [SerializeField]
    private Transform Dice2;
    [SerializeField]
    private Transform Dice3;
    [SerializeField]
    private Transform EndTurn;

    [SerializeField]
    private TextMeshProUGUI PhaseText;

    [SerializeField]
    private TextMeshProUGUI Winner;
    [SerializeField]
    private Color WinColor;
    [SerializeField]
    private Color LooseColor;

    void Start()
    {

    }

    void Update()
    {
        PhaseText.text = GameManager.IsPhase2() ? "Phase 2" : "Phase 1";

        if (GameManager.ActivePlayerNumber == 1)
        {
            Player1Light.SetActive(true);
            Player2Light.SetActive(false);
            Player1Image.color = new Color(Player1Image.color.r, Player1Image.color.g, Player1Image.color.b, 1);
            Player2Image.color = new Color(Player2Image.color.r, Player2Image.color.g, Player2Image.color.b, 0);
        }
        else
        {
            Player1Light.SetActive(false);
            Player2Light.SetActive(true);
            Player1Image.color = new Color(Player1Image.color.r, Player1Image.color.g, Player1Image.color.b, 0);
            Player2Image.color = new Color(Player2Image.color.r, Player2Image.color.g, Player2Image.color.b, 1);
        }

        if (GameManager.Player1 != null)
            Player1Purse.text = GameManager.GetPurse(1).ToString();
        if (GameManager.Player2 != null)
            Player2Purse.text = GameManager.GetPurse(2).ToString();

        TemporaryUI();
    }

    public void TemporaryUI()
    {
        if (GameManager.GetDice(1) != 0)
        {
            Dice1Text.text = GameManager.GetDice(1).ToString();
            Dice1Text.gameObject.SetActive(true);
        }
        else
        {
            Dice1Text.gameObject.SetActive(false);
        }

        if (GameManager.GetDice(2) != 0)
        {
            Dice2Text.text = GameManager.GetDice(2).ToString();
            Dice2Text.gameObject.SetActive(true);
        }
        else
        {
            Dice2Text.gameObject.SetActive(false);
        }

        if (GameManager.GetDice(3) != 0)
        {
            Dice3Text.text = GameManager.GetDice(3).ToString();
            Dice3Text.gameObject.SetActive(true);
        }
        else
        {
            Dice3Text.gameObject.SetActive(false);
        }

        if (GameManager.GetBank(1) != 0)
        {
            Bank1.text = ShowBankText(1);
            Bank1.gameObject.SetActive(true);
        }
        else
        {
            Bank1.gameObject.SetActive(false);
        }

        if (GameManager.GetBank(2) != 0)
        {
            Bank2.text = ShowBankText(2);
            Bank2.gameObject.SetActive(true);
        }
        else
        {
            Bank2.gameObject.SetActive(false);
        }

        if (GameManager.GetBank(3) != 0)
        {
            Bank3.text = ShowBankText(3);
            Bank3.gameObject.SetActive(true);
        }
        else
        {
            Bank3.gameObject.SetActive(false);
        }

        if (GameManager.GetBank(4) != 0)
        {
            Bank4.text = ShowBankText(4);
            Bank4.gameObject.SetActive(true);
        }
        else
        {
            Bank4.gameObject.SetActive(false);
        }

        if (GameManager.GetBank(5) != 0)
        {
            Bank5.text = ShowBankText(5);
            Bank5.gameObject.SetActive(true);
        }
        else
        {
            Bank5.gameObject.SetActive(false);
        }
    }

    public void SetWinnerLooser(Player playerWinner, Player playerLooser)
    {
        Winner.text = playerWinner.PlayerName + " wins";

        if (playerWinner.IsLocal)
            Winner.color = WinColor;
        else
            Winner.color = LooseColor;
    }

    public void EnableDice(Player player)
    {
        //Camera.main.ScreenToWorldPoint(,);
        Dice1.gameObject.SetActive(true);
        Dice2.gameObject.SetActive(true);
    }

    public void EnablePlayBonusDiceButton(Player player)
    {
        Dice3.gameObject.SetActive(true);
    }
    public void DisablePlayBonusDiceButton(Player player)
    {
        Dice3.gameObject.SetActive(false);
    }

    public void EnableEndTurnButton(Player player)
    {
        EndTurn.gameObject.SetActive(true);
    }
    public void DisableEndTurnButton(Player player)
    {
        EndTurn.gameObject.SetActive(false);
    }

    public void EnableOperation(int diceTotal)
    {
        Operation.SetActive(true);

        DiceTotal.text = diceTotal.ToString();
        OperationTotal.text = Mathf.Abs(diceTotal - 9).ToString();
    }
    public void DisableOperation()
    {
        Operation.SetActive(false);
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
}
