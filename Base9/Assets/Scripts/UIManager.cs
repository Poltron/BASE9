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
    private TextMeshPro[] BankAmounts;
    [SerializeField]
    private TextMeshPro[] BankNumbers;

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
    private Animator Dice1;
    [SerializeField]
    private Animator Dice2;
    [SerializeField]
    private Animator Dice3;
    [SerializeField]
    private Animator EndTurn;

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

        for (int i = 1; i <= 5; i++)
        {
            if (GameManager.GetBank(i) != 0)
            {
                BankAmounts[i].text = ShowBankText(i);
                BankAmounts[i].gameObject.SetActive(true);
            }
            else
            {
                BankAmounts[i].gameObject.SetActive(false);
            }
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
        Dice1.SetBool("bEnabled", true);
        Dice2.gameObject.SetActive(true);
        Dice2.SetBool("bEnabled", true);
    }

    public void EnableThirdDiceButton(Player player)
    {
        Dice3.gameObject.SetActive(true);
        Dice3.SetBool("bEnabled", true);
    }
    public void DisableThirdDiceButton(Player player)
    {
        Dice3.SetBool("bEnabled", false);
    }

    public void EnableEndTurnButton(Player player)
    {
        EndTurn.gameObject.SetActive(true);
        EndTurn.SetBool("bEnabled", true);
    }
    public void DisableEndTurnButton(Player player)
    {
        EndTurn.SetBool("bEnabled", false);
    }

    public void EnableOperation(int diceTotal)
    {
        string str = "";

        if (diceTotal != 9)
        {
            int toPay = 0;

            if (diceTotal > 9)
            {
                toPay = diceTotal - 9;
                str = "<color=#FFFF00>" + diceTotal + "</color>\n-\n9\n=\n<color=#FF0000>" + Mathf.Abs(toPay).ToString() + "</color>";
            }
            else if (diceTotal < 9)
            {
                toPay = 9 - diceTotal;
                str = "9\n-\n<color=#FFFF00>" + diceTotal + "</color>\n=\n<color=#FF0000>" + Mathf.Abs(toPay).ToString() + "</color>";
            }

            while (toPay > 0)
            {
                int bankNb = 0;
                if (toPay > 5)
                {
                    bankNb = 4;
                }
                else
                {
                    bankNb = toPay - 1;
                }

                StartCoroutine(FadeColorIn(BankNumbers[bankNb], Color.red));
                toPay -= 5;
            }
        }
        else
        {
            str = "9\n-\n9\n=\n<color=#00FF00>0</color>";

            for (int i = 1; i <= 3; i++)
            {
                int dice = GameManager.GetDice(i);
                if (dice != 0 && dice != 6)
                {
                    StartCoroutine(FadeColorIn(BankNumbers[dice - 1], Color.green));
                }
            }
        }

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

    IEnumerator FadeColorIn(TextMeshPro text, Color target)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("cou");
        Color origin = text.color;
        float f = 0;
        for (f = 0; f < 1.0f; f += Time.deltaTime * 2)
        {
            Color c = Color.Lerp(origin, target, f);
            text.color = c;
            Debug.Log(c);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);

        for (f = 0; f < 1.0f; f += Time.deltaTime * 2)
        {
            Color c = Color.Lerp(target, origin, f);
            text.color = c;
            Debug.Log(c);
            yield return new WaitForEndOfFrame();
        }

        text.color = origin;
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
