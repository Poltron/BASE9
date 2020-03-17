using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
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

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void Throw(Transform spawn)
    {
        _transform.position = spawn.position;
        _transform.rotation = Random.rotation;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(spawn.forward * 25, ForceMode.VelocityChange);
        _rigidbody.AddTorque(Random.onUnitSphere * Random.Range(0, 10), ForceMode.VelocityChange);
        gameObject.SetActive(true);
    }
}
