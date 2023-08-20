using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= -7)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip,transform.position);
            if(player != null)
            {

                switch (powerupID)
                {
                    case 0:
                        player.TripleAShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        break;
                }
            }
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player2")
        {
            Player2 player2 = other.transform.GetComponent<Player2>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player2 != null)
            {

                switch (powerupID)
                {
                    case 0:
                        player2.TripleAShotActive();
                        break;
                    case 1:
                        player2.SpeedBoostActive();
                        break;
                    case 2:
                        player2.ShieldActive();
                        break;
                    default:
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}