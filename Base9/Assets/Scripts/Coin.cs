using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    Transform coin;

    [SerializeField]
    GameObject disappear;

    [SerializeField]
    GameObject appear;

    public void Teleport(Vector3 position)
    {
        StartCoroutine(TP(0.5f, position));
    }

    IEnumerator TP(float time, Vector3 position)
    {
        Instantiate(disappear, coin.position + new Vector3(0, 2, 0), Quaternion.identity);
        transform.position = position;
        yield return new WaitForSeconds(time);
        coin.position = position;
        Instantiate(appear, coin.position + new Vector3(0, 2, 0), Quaternion.identity);
    }

}
