using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class PlayerUI
{
    public TextMeshPro Purse;
    public TextMeshPro Name;
    public Animator Light;
}

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;

    [SerializeField]
    private PlayerUI[] playerUIs;

    [SerializeField]
    private TextMeshPro[] BankAmounts;
    [SerializeField]
    private TextMeshPro[] BankNumbers;
    [SerializeField]
    private GameObject[] BankLock;

    [SerializeField]
    private Animator LeftSidePanel;
    [SerializeField]
    private TextMeshProUGUI OperationLeft;
    [SerializeField]
    private Animator RightSidePanel;
    [SerializeField]
    private TextMeshProUGUI OperationRight;
    [SerializeField]
    private Color GreenColor;
    [SerializeField]
    private Color RedColor;

    [SerializeField]
    private Animator Dice1;
    [SerializeField]
    private Animator Dice2;
    [SerializeField]
    private Animator Dice3;
    [SerializeField]
    private Animator EndTurn;

    [SerializeField]
    private Animation Phase;
    [SerializeField]
    private TextMeshProUGUI PhaseText;

    [SerializeField]
    private TextMeshProUGUI Winner;
    [SerializeField]
    private TextMeshProUGUI EndReason;
    [SerializeField]
    private Color WinColor;
    [SerializeField]
    private Color LooseColor;

    [SerializeField]
    private Animation[] Banks;
    [SerializeField]
    private Animation BanksParent;

    [SerializeField]
    private Animation Base9Animation;

    void Start()
    {

    }

    void Update()
    {
        if (GameManager.Player1 != null)
        {
            playerUIs[0].Purse.text = (GameManager.GetPurse(1) < 0 ? "0" : GameManager.GetPurse(1).ToString());
            playerUIs[0].Name.text = GameManager.Player1.PlayerName;
        }
        if (GameManager.Player2 != null)
        {
            playerUIs[1].Purse.text = (GameManager.GetPurse(2) < 0 ? "0" : GameManager.GetPurse(2).ToString());
            playerUIs[1].Name.text = GameManager.Player2.PlayerName;
        }

        //Debug.Log("Side_Panel_LEFT_Pop" + clip.IsName("Side_Panel_LEFT_Pop"));
        //Debug.Log("Side_Panel_LEFT_DePop" + clip.IsName("Side_Panel_LEFT_DePop"));
        //TemporaryUI();
    }

    public void TemporaryUI()
    {
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
        if (playerWinner.IsLocal)
        {
            SoundManager.Instance.PlaySoundCue(SoundName.Jingle_Win, Vector3.zero);
            Winner.color = WinColor;
        }
        else
        {
            SoundManager.Instance.PlaySoundCue(SoundName.Jingle_Loose, Vector3.zero);
            Winner.color = LooseColor;
        }

        Winner.text = playerWinner.PlayerName + " wins";
    }

    public void SetEndReason(string reason)
    {
        EndReason.text = reason;
    }

    public void EnableTwoDice(Player player)
    {
        Dice1.gameObject.SetActive(true);
        Dice1.SetBool("bEnabled", true);
        SoundManager.Instance.PlaySoundCue(SoundName.Dice_Idle, Vector3.zero);

        Dice2.gameObject.SetActive(true);
        Dice2.SetBool("bEnabled", true);
        SoundManager.Instance.PlaySoundCue(SoundName.Dice_Idle, Vector3.zero);
    }

    public void EnableThirdDiceButton(Player player)
    {
        Dice3.gameObject.SetActive(true);
        Dice3.SetBool("bEnabled", true);
        SoundManager.Instance.PlaySoundCue(SoundName.Dice_Idle, Vector3.zero);
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

                StartCoroutine(FadeColorIn(BankNumbers[bankNb], RedColor));
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
                    FlashGreenBank(dice - 1);
                    //StartCoroutine(FadeColorIn(BankNumbers[dice - 1], GreenColor));
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

        Color origin = text.color;
        float f = 0;
        for (f = 0; f < 1.0f; f += Time.deltaTime * 2)
        {
            Color c = Color.Lerp(origin, target, f);
            text.color = c;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);

        for (f = 0; f < 1.0f; f += Time.deltaTime * 2)
        {
            Color c = Color.Lerp(target, origin, f);
            text.color = c;
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
            SoundManager.Instance.PlaySoundCue(SoundName.Panel_Show_Left, Vector3.zero);
        }
        else
        {
            RightSidePanel.SetBool("Enabled", true);
            SoundManager.Instance.PlaySoundCue(SoundName.Panel_Show_Right, Vector3.zero);
        }
    }

    private void HideSidePanel()
    {
        if (GameManager.ActivePlayerNumber == 0)
        {
            LeftSidePanel.SetBool("Enabled", false);
            SoundManager.Instance.PlaySoundCue(SoundName.Panel_Close_Left, Vector3.zero);
        }
        else
        {
            RightSidePanel.SetBool("Enabled", false);
            SoundManager.Instance.PlaySoundCue(SoundName.Panel_Close_Right, Vector3.zero);
        }
    }

    public void ShowLight()
    {
        playerUIs[GameManager.ActivePlayerNumber].Light.SetBool("Enabled", true);
    }

    public void HideLight()
    {
        playerUIs[GameManager.ActivePlayerNumber].Light.SetBool("Enabled", false);
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

    public void EnableBankLock(int bankId)
    {
        Debug.Log("Playing BankLock " + bankId);
        BankLock[bankId].SetActive(true);
        Banks[bankId].Play("TriangleLock");

        SoundManager.Instance.PlaySoundCue(SoundName.Tile_Lock, Vector3.zero);
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

    public void HighlightBank(int bankIndex, bool bHighlighted)
    {
        if (bHighlighted)
            Banks[bankIndex].Play("TriangleHighlight");
        else
            Banks[bankIndex].Play("TriangleHighlightREVERSE");
    }

    public void FlashGreenBank(int bankIndex)
    {
        Banks[bankIndex].Play("TriangleBase9GREEN");
    }

    public void Base9Anim()
    {
        Base9Animation.Play();
        SoundManager.Instance.PlaySoundCue(SoundName.Jingle_Base9, Vector3.zero);
    }

    public void ShowPhase2()
    {
        BanksParent.Play();
        StartCoroutine(GameManager.WaitFor(1.0f, Phase2Anim1));
    }

    private void Phase2Anim1()
    {
        SoundManager.Instance.PlaySoundCue(SoundName.Phase2_Start, Vector3.zero);
        Phase.Play();
        PhaseText.text = "Phase 2";
        //StartCoroutine(GameManager.WaitFor(1.0f, Phase2Anim2));
    }

    private void Phase2Anim2()
    {
        
    }

    public void EnableFBButton()
    {

    }
}
