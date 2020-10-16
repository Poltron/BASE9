using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transitionAnimator = default;

    private static LevelLoader instance;
    public static LevelLoader Instance { get { return instance; } }

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void LoadNextLevel(int level)
    {
        StartCoroutine(LoadLevel(level));
    }

    IEnumerator LoadLevel(int level)
    {
        transitionAnimator.SetTrigger("Start");

        if (level == 0)
        {
            SoundManager.Instance.StopAmbient();
            SoundManager.Instance.PlayMusic(SoundName.Musique_Loop_Menu);
        }

        yield return new WaitForSeconds(1.0f);

        if (level == -1)
            Application.Quit();
        else
            SceneManager.LoadScene(level);
    }

    public void QuitApplication()
    {
        LoadNextLevel(-1);
    }
}
