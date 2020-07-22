﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision coll)
    {
        ContactPoint point = coll.GetContact(0);
        Debug.Log("OnCollisionEnter " + LayerMask.LayerToName(point.otherCollider.gameObject.layer));

        if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            Debug.Log("Dice");
            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Hit_Coin, transform.position);
        }
        else if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Wood"))
        {
            Debug.Log("Wood");
            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Hit_Ground, transform.position);
        }
        else if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Felt"))
        {
            Debug.Log("Felt");
            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Hit_Ground, transform.position);
        }
    }
}
