using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    private int number;

    [Header("Sides")]
    [SerializeField]
    private Transform Side1;
    [SerializeField]
    private Transform Side2;
    [SerializeField]
    private Transform Side3;
    [SerializeField]
    private Transform Side4;
    [SerializeField]
    private Transform Side5;
    [SerializeField]
    private Transform Side6;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private bool bThrown;
    private GameManager gameManager;

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log("dice awake");
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (bThrown)
        {
            if (_rigidbody.velocity.magnitude < 0.1f && _rigidbody.angularVelocity.magnitude < 0.1f)
            {
                bThrown = false;
                gameManager.DiceResult(number);
            }
        }
    }

    public void Throw(Transform spawn)
    {
        _transform.position = spawn.position;
        _transform.rotation = UnityEngine.Random.rotation;

        gameObject.SetActive(true);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(spawn.forward * UnityEngine.Random.Range(5, 15), ForceMode.VelocityChange);
        _rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 15), ForceMode.VelocityChange);

        StartCoroutine(WaitFor(0.5f, SetThrown));
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
