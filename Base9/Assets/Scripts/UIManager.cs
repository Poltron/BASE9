using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;

    [SerializeField]
    private TextMeshPro Player1Purse;
    [SerializeField]
    private TextMeshPro Player1Name;
    [SerializeField]
    private Animator Player1Light;

    [SerializeField]
    private TextMeshPro Player2Purse;
    [SerializeField]
    private TextMeshPro Player2Name;
    [SerializeField]
    private Animator Player2Light;

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


    [SerializeField]
    private Animator LeftSidePanel;
    [SerializeField]
    private TextMeshProUGUI OperationLeft;
    [SerializeField]
    private Animator RightSidePanel;
    [SerializeField]
    private TextMeshProUGUI OperationRight;

    [SerializeField]
    private TextMeshPro Dice1Text;
    [SerializeField]
    private TextMeshPro Dice2Text;
    [SerializeField]
    private TextMeshPro Dice3Text;

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

        if (GameManager.Player1 != null)
            Player1Purse.text = GameManager.GetPurse(1).ToString();
        if (GameManager.Player2 != null)
            Player2Purse.text = GameManager.GetPurse(2).ToString();

        //TemporaryUI();
    }

    public void TemporaryUI()
    {
        // dice

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

        // banks

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

    public void EnableTwoDice(Player player)
    {
        Dice1.gameObject.SetActive(true);
        Dice2.gameObject.SetActive(true);
    }

    public void EnableThirdDiceButton(Player player)
    {
        Dice3.gameObject.SetActive(true);
    }
    public void DisableThirdDiceButton(Player player)
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
        string str;
        if (diceTotal > 9)
            str = diceTotal + "\n-\n9\n=\n<color=#FF0000>" + Mathf.Abs(diceTotal - 9).ToString() + "</color>";
        else if (diceTotal < 9)
            str = "9\n-\n" + diceTotal + "\n=\n<color=#FF0000>" + Mathf.Abs(9 - diceTotal).ToString() + "</color>";
        else
            str = "9\n-\n9\n=\n<color=#00FF00>0</color>";

        if (GameManager.ActivePlayerNumber == 0)
        {
            OperationLeft.gameObject.SetActive(true);
            OperationLeft.text = str;
        }
        else
        {
            OperationRight.gameObject.SetActive(true);
            OperationRight.text = str;
        }
    }

    public void DisableOperation()
    {
        if (GameManager.ActivePlayerNumber == 0)
        {
            OperationLeft.gameObject.SetActive(false);
        }
        else
        {
            OperationRight.gameObject.SetActive(false);
        }
    }

    public void ShowSidePanel()
    {
        if (GameManager.ActivePlayerNumber == 0)
        {
            LeftSidePanel.SetBool("Enabled", true);
        }
        else
        {
            RightSidePanel.SetBool("Enabled", true);
        }
    }

    private void HideSidePanel()
    {
        if (GameManager.ActivePlayerNumber == 0)
        {
            LeftSidePanel.SetBool("Enabled", false);
        }
        else
        {
            RightSidePanel.SetBool("Enabled", false);
        }
    }

    public void ShowLight()
    {
        if (GameManager.ActivePlayerNumber == 0)
        {
            Player1Light.SetBool("Enabled", true);
        }
        else
        {
            Player2Light.SetBool("Enabled", true);
        }
    }

    public void HideLight()
    {
        if (GameManager.ActivePlayerNumber == 0)
        {
            Player1Light.SetBool("Enabled", false);
        }
        else
        {
            Player2Light.SetBool("Enabled", false);
        }
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

    public void ShowEndTurn()
    {
        DisableOperation();
        HideSidePanel();
        HideLight();
    }

    public void ShowStartTurn()
    {
        ShowLight();
    }
}
