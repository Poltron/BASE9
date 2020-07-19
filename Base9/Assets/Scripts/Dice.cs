using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DiceSide
{
    [SerializeField]
    public Transform transform;
    [SerializeField]
    public int number;
}

public class Dice : MonoBehaviour
{
    [SerializeField]
    private int number;

    [Header("Sides")]
    [SerializeField]
    private DiceSide[] sides;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private GameManager gameManager;

    private bool bThrown = false;
    private float timeStopped = 0;

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (bThrown)
        {
            if (_rigidbody.velocity.magnitude < 0.1f && _rigidbody.angularVelocity.magnitude < 0.1f)
            {
                timeStopped += Time.deltaTime;

                if (timeStopped > 0.75f)
                {
                    bThrown = false;

                    if (IsDiceBroken()) // throw dice again if not perfectly flat
                    {
                        gameManager.RPC_ThrowDice(number);
                    }
                    else
                    {
                        gameManager.DiceResult(number, GetTopFace());
                    }
                }
            }
            else
            {
                timeStopped = 0;
            }

            if (_transform.position.y < -10)
            {
                Throw(gameManager.GetRandomDiceSpawn());
            }
        }
    }

    public void Throw(Transform spawn)
    {
        _transform.position = spawn.position;
        _transform.rotation = UnityEngine.Random.rotation;

        gameObject.SetActive(true);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(spawn.forward * UnityEngine.Random.Range(10, 20), ForceMode.VelocityChange);
        _rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(4, 12), ForceMode.VelocityChange);

        SoundManager.Instance.PlaySoundCue(SoundName.Dice_Launch, Vector3.zero);

        StartCoroutine(WaitFor(0.1f, SetThrown));
    }

    private void SetThrown()
    {
        bThrown = true;
    }

    public IEnumerator WaitFor(float time, Action action)
    {
        yield return new WaitForSeconds(time);

        action();
    }

    public int GetTopFace()
    {

        DiceSide bestSide = sides[0];
        float bestSideDot = -1;
        foreach (DiceSide side in sides)
        {
            float dot = Vector3.Dot(Vector3.up, side.transform.position - _transform.position);
            if (bestSideDot < dot)
            {
                bestSideDot = dot;
                bestSide = side;
            }
        }

        return bestSide.number;
    }

    public bool IsDiceBroken()
    {
        DiceSide bestSide = sides[0];
        float bestSideDot = -1;
        foreach (DiceSide side in sides)
        {
            float dot = Vector3.Dot(Vector3.up, (side.transform.position - _transform.position).normalized);
            if (bestSideDot < dot)
            {
                bestSideDot = dot;
                bestSide = side;
            }
        }
        Debug.Log(bestSideDot);
        return bestSideDot < 0.99f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint point = collision.GetContact(0);
        Debug.Log("OnCollisionEnter " + LayerMask.LayerToName(point.otherCollider.gameObject.layer));

        if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Dice"))
        {
            Debug.Log("Dice");
            SoundManager.Instance.PlaySoundCue(SoundName.Dice_Hit_Dice, transform.position);
        }
        else if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Wood"))
        {
            Debug.Log("Wood");
            SoundManager.Instance.PlaySoundCue(SoundName.Dice_Hit_Wood, transform.position);
        }
        else if (point.otherCollider.gameObject.layer == LayerMask.NameToLayer("Felt"))
        {
            Debug.Log("Felt");
            SoundManager.Instance.PlaySoundCue(SoundName.Dice_Hit_Ground, transform.position);
        }
    }
}
