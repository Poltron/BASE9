using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private Transform _coin = default;

    [SerializeField]
    private Rigidbody _rigidbody = default;

    [SerializeField]
    private BoxCollider _collider = default;

    [SerializeField]
    private Animator _animator = default;

    [SerializeField]
    private GameObject _disappear = default;

    [SerializeField]
    private GameObject _appear = default;

    private Vector3 spawnPos;

    public void Teleport(Vector3 position, bool destroy)
    {
        spawnPos = position;
        StartCoroutine(TP(0.5f, position, destroy));
    }

    public void Respawn()
    {
        transform.position = spawnPos;
        _coin.position = spawnPos;
        _coin.rotation = Quaternion.identity;
        Instantiate(_appear, spawnPos + new Vector3(0, 2, 0), Quaternion.identity);
    }

    IEnumerator TP(float time, Vector3 position, bool destroy)
    {
        Instantiate(_disappear, _coin.position + new Vector3(0, 2, 0), Quaternion.identity);
        SoundManager.Instance.PlaySoundCue(SoundName.Coin_Swap, transform.position);
        yield return new WaitForSeconds(time);

        transform.position = position;
        _coin.position = position;
        _coin.rotation = Quaternion.identity;
        Instantiate(_appear, _coin.position + new Vector3(0, 2, 0), Quaternion.identity);

        if (destroy)
        {
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;

            _animator.SetBool("bDestroy", true);

            SoundManager.Instance.PlaySoundCue(SoundName.Coin_Destroy, transform.position);
            yield return new WaitForSeconds(1.0f);
            Destroy(gameObject);
        }
    }
}