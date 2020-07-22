using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    Animator transitionAnimator;

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

    public void LoadNextLevel(int level, bool photon)
    {
        StartCoroutine(LoadLevel(level, photon));
    }

    IEnumerator LoadLevel(int level, bool photon)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1.0f);

        if (photon)
            Photon.Pun.PhotonNetwork.LoadLevel("Game");
        else
            SceneManager.LoadScene(level);
    }
}
