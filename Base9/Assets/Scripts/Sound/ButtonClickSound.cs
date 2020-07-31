using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionButton
{
    Play,
    Stop
}

public class ButtonClickSound : MonoBehaviour
{
    [SerializeField]
    private SoundName name;
    [SerializeField]
    private ActionButton action;

    [SerializeField]
    private float randomMinPitch = 1f;
    [SerializeField]
    private float randomMaxPitch = 1f;


    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (action == ActionButton.Play)
        {
            float pitch = UnityEngine.Random.Range(randomMinPitch, randomMaxPitch);
            SoundManager.Instance.PlaySoundCue(name, Vector3.zero, pitch);
        }
        else if (action == ActionButton.Stop)
        {
            SoundManager.Instance.RemoveCue(name);
        }
    }
}
