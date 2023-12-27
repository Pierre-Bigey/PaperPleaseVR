using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventObjectFall : MonoBehaviour
{
    [SerializeField] private float minAltitude = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody _rigidbody;
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (transform.position.y <= minAltitude)
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
}
