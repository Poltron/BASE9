using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollisions : MonoBehaviour
{
    [SerializeField]
    private Coin coin;

    void OnCollisionEnter(Collision coll)
    {
        ContactPoint point = coll.GetContact(0);

        if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Hit_Coin, transform.position);
        }
        else if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Wood"))
        {
            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Hit_Ground, transform.position);
        }
        else if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Felt"))
        {
            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Hit_Ground, transform.position);
        }
        else if (point.otherCollider.gameObject.tag == "Killzone")
        {
            Debug.Log(coin.gameObject.name + " touched killzone");
            coin.Respawn();
        }
    }
}
