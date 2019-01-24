using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMax, xMin;
}

public class PlayerController : MonoBehaviour
{ 
    public int helth; 
    public float speed;

    public Boundary boundary;
    public GameObject projectile;
    public Transform[] shootingSpawn;
    public GameObject shield;
    public GameObject playerExplosion;

    public float timeWait;

    private float timeNextFire;

    Rigidbody2D _rb;

    int _countShootingSpawn = 1;
    bool _shieldIsActive; 

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
            Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax), transform.position.y);
    }

    public void Suicide()
    { 
        Destroy(this.gameObject);
        Instantiate(playerExplosion, this.transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyProjectile" && !_shieldIsActive) 
            GameManager.Instance.SetPlayerHelth(--helth);

        if (other.gameObject.tag == "Enemy" && !_shieldIsActive)
            GameManager.Instance.SetPlayerHelth(0);

        Destroy(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        switch (tag)
        { 
            case "AddWeapon":
                AddShootingSpawn(); 
                break;
            case "ExtraLife":
                GameManager.Instance.SetPlayerHelth(++helth); 
                break;
            case "Shield":
                CreateShield(); 
                break;
            default:
                break;
        }

        Destroy(other.gameObject);
    } 
     
    private void AddShootingSpawn()
    {
        if (_countShootingSpawn < shootingSpawn.Length) { 
            _countShootingSpawn += 2;
        }
    } 

    private void CreateShield()
    { 
        GameObject playerShield = Instantiate(shield, this.transform.position, Quaternion.identity);
        playerShield.transform.SetParent(this.transform);

        _shieldIsActive = true;

        StartCoroutine(DeactiveShield(playerShield));
    }

    private IEnumerator DeactiveShield(GameObject shield)
    {
        yield return new WaitForSeconds(10f);

        _shieldIsActive = false;
        Destroy(shield);
    }
}
