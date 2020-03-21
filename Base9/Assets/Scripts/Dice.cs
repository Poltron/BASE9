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

                    _rigidbody.isKinematic = true;
                    gameManager.DiceResult(number, bestSide.number);
                }
            }
            else
            {
                timeStopped = 0;
            }
        }
    }

    public void Throw(Transform spawn)
    {
        _transform.position = spawn.position;
        _transform.rotation = UnityEngine.Random.rotation;

        gameObject.SetActive(true);

        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(spawn.forward * UnityEngine.Random.Range(5, 15), ForceMode.VelocityChange);
        _rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(4, 12), ForceMode.VelocityChange);

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
}
