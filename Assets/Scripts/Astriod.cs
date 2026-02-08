using UnityEngine;

public class Astriod : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 20.0f;
    [SerializeField] private GameObject _expelosionPrefab;
    
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
            GetComponent<CircleCollider2D>().enabled = false;
            GameManager.Instance.OnGameStart?.Invoke(true);
            Destroy(this.gameObject,0.2f);
        }
    }
}