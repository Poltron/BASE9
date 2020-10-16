using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameState gameState = default;

    [Tooltip("The Ui Text to inform the user about the connection progress")]
    [SerializeField]
    private TextMeshProUGUI feedbackText = default;

    [Tooltip("The Back button to stop the opponent search")]
    [SerializeField]
    private Button backButton = default;

    [SerializeField]
    private float minimumTimeToFindOpponent = default;
    
    [SerializeField]
    private float maximumTimeToFindOpponent = default;

    private Coroutine lookForOpponent;

    public void PlayOnline()
    {
        LogFeedback("Playing online");
        gameState.gameMode = GameMode.Online;

        lookForOpponent = StartCoroutine(LookForOpponent());
    }

    public void PlayLocal()
    {
        LogFeedback("Playing local");
        gameState.gameMode = GameMode.Local;

        SoundManager.Instance.PlaySoundCue(SoundName.Game_Start, Vector3.zero);

        DG.Tweening.DOVirtual.DelayedCall(1.0f, ChangeSoundtracks);
        DG.Tweening.DOVirtual.DelayedCall(2.0f, LoadGameScene);
    }

    public void PlayAI()
    {
        LogFeedback("Playing against AI");
        gameState.gameMode = GameMode.AI;

        SoundManager.Instance.PlaySoundCue(SoundName.Game_Start, Vector3.zero);

        DG.Tweening.DOVirtual.DelayedCall(1.0f, ChangeSoundtracks);
        DG.Tweening.DOVirtual.DelayedCall(2.0f, LoadGameScene);
    }

    public void LoadGameScene()
    {
        LogFeedback("Loading game scene...");
        LevelLoader.Instance.LoadNextLevel(1);
    }

    private IEnumerator LookForOpponent()
    {
        ScreenLogs("Looking for opponent...");
        SoundManager.Instance.PlaySoundCue(SoundName.Connecting, Vector3.zero);

        yield return new WaitForSeconds(Random.Range(minimumTimeToFindOpponent, maximumTimeToFindOpponent));
        
        backButton.interactable = false;

        SoundManager.Instance.RemoveCue(SoundName.Connecting);
        ScreenLogs("Found an opponent !");

        PlayAI();
    }

    public void StopLookingForOpponent()
    {
        StopCoroutine(lookForOpponent);
    }

    /// <summary>
    /// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
    /// </summary>
    /// <param name="message">Message.</param>
    void LogFeedback(string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
    /// </summary>
    /// <param name="message">Message.</param>
    void ScreenLogs(string message)
    {
        // we do not assume there is a feedbackText defined.
        if (feedbackText == null)
        {
            return;
        }

        // add new messages as a new line and at the bottom of the log.
        feedbackText.text += System.Environment.NewLine + message;
    }

    private void ChangeSoundtracks()
    {
        Debug.Log("ChangeSoundtracks");
        SoundManager.Instance.PlayAmbient(SoundName.Ambiance_Loop);
        SoundManager.Instance.PlayMusic(SoundName.Musique_Loop_Phase1);
    }
}
