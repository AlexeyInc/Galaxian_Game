using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMax, xMin, yMax, yMin;
}

public class PlayerController : MonoBehaviour
{ 
    public int helth; 
    public float speed;

    public Boundary boundary;
    public GameObject projectile;
    public Transform[] shootingSpawn;
    public GameObject shield;//make other, not image

    public float timeWait;
    public float timeNextFire;

    Rigidbody2D _rb;
    int _countShootingSpawn = 1;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        GameManager.Instance.SetPlayerHelth(helth);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > timeNextFire)
        {
            timeNextFire = Time.time + timeWait;

            for (int i = 0; i < _countShootingSpawn; i++)
            { 
                Instantiate(projectile, shootingSpawn[i].transform.position, Quaternion.identity);
            }
        } 
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        _rb.velocity = movement * speed;

        _rb.position = new Vector2(
            Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(transform.position.y, boundary.yMin, boundary.yMax));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyProjectile") 
            GameManager.Instance.SetPlayerHelth(--helth);

        if (other.gameObject.tag == "Enemy")
            GameManager.Instance.SetPlayerHelth(0);

        Destroy(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        switch (tag)
        { 
            case "AddFire":
                AddShootingSpawn();
                Debug.Log("Edd new fire");
                break;
            case "ExtraLife":
                GameManager.Instance.SetPlayerHelth(++helth);
                Debug.Log("Add extra life");
                break;
            case "Shield":
                CreateShield();
                Debug.Log("Shield");
                break;
            default:
                break;
        }

        Destroy(other.gameObject);
    } 
     
    private void AddShootingSpawn()
    {
        if (_countShootingSpawn < shootingSpawn.Length)
        { 
            _countShootingSpawn += 2;
        }
    } 

    private void CreateShield()
    {
        GameObject playerShield = Instantiate(shield, this.transform.position, Quaternion.identity);
        playerShield.transform.SetParent(this.transform);
    }
}
