using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    Transform coin;

    [SerializeField]
    Rigidbody rigidbody;

    [SerializeField]
    BoxCollider collider;

    [SerializeField]
    Animator animator;

    [SerializeField]
    GameObject disappear;

    [SerializeField]
    GameObject appear;

    public void Teleport(Vector3 position, bool destroy)
    {
        StartCoroutine(TP(0.5f, position, destroy));
    }

    IEnumerator TP(float time, Vector3 position, bool destroy)
    {
        Instantiate(disappear, coin.position + new Vector3(0, 2, 0), Quaternion.identity);
        SoundManager.Instance.PlaySoundCue(SoundName.Coin_Swap, transform.position);
        yield return new WaitForSeconds(time);

        transform.position = position;
        coin.position = position;
        coin.rotation = Quaternion.identity;
        Instantiate(appear, coin.position + new Vector3(0, 2, 0), Quaternion.identity);

        if (destroy)
        {
            rigidbody.isKinematic = true;
            collider.isTrigger = true;

            animator.SetBool("bDestroy", true);

            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Destroy, transform.position);
            yield return new WaitForSeconds(1.0f);
            Destroy(gameObject);
        }
    }

}
