using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astriod : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _expelosionPrefab;
    private SpawnManager _spawnManager;
    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime );
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_expelosionPrefab,transform.position,Quaternion.identity);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            _spawnManager.StartSpawning();
            Destroy(this.gameObject,0.5f);
        }
    }
}