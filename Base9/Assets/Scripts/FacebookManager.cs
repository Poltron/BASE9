using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button fbConnect;
    [SerializeField]
    private UnityEngine.UI.Button fbDisconnect;
    [SerializeField]
    private UnityEngine.UI.Image profilePicImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI userName;

    private Sprite profilePicSprite;

    // Start is called before the first frame update
    void Start()
    {
        FB.Init(OnInitCompleted);
    }

    void OnInitCompleted()
    {
        if (FB.IsLoggedIn)
        {
            LoginResult(null);
        }
        else
        {
            EnableConnectedUI(false);
        }
    }

    public void FBLogin()
    {
        FB.LogInWithReadPermissions(null, LoginResult);
    }

    public void FBLogout()
    {
        FB.LogOut();
        EnableConnectedUI(false);
    }

    void LoginResult(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            FB.API("/me?fields=first_name", HttpMethod.GET, NameCallback);
            FB.API("/me/picture?type=normal", HttpMethod.GET, ProfilePhotoCallback);

            EnableConnectedUI(true);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void NameCallback(IGraphResult graphResult)
    {
        Debug.Log(graphResult.Error);

        // Add error handling here
        if (graphResult.ResultDictionary != null)
        {
            foreach (string key in graphResult.ResultDictionary.Keys)
            {
                Debug.Log(key + " : " + graphResult.ResultDictionary[key].ToString());
                if (key == "first_name")
                {
                    userName.text = graphResult.ResultDictionary[key].ToString();
                    userName.gameObject.SetActive(true);
                }
            }
        }
    }

    private void ProfilePhotoCallback(IGraphResult graphResult)
    {
        if (graphResult.Error != null)
            Debug.Log(graphResult.Error);

        profilePicSprite = Sprite.Create(graphResult.Texture, new Rect(0, 0, graphResult.Texture.width, graphResult.Texture.height), Vector2.zero);
        profilePicImage.sprite = profilePicSprite;
        profilePicImage.gameObject.SetActive(true);
    }

    private void EnableConnectedUI(bool enabled)
    {
        if (enabled)
        {
            fbConnect.gameObject.SetActive(false);
            fbDisconnect.gameObject.SetActive(true);
        }
        else
        {
            fbConnect.gameObject.SetActive(true);
            fbDisconnect.gameObject.SetActive(false);
            profilePicImage.gameObject.SetActive(false);
            userName.gameObject.SetActive(false);
        }
    }
}
