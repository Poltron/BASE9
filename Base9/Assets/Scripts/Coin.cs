using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    Transform coin;
    
    public void Teleport(Vector3 position)
    {
        transform.position = position;
        coin.position = position;
    }

}
