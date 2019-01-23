using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMyMovement : MonoBehaviour {

    //public float speedMove = 4;
    //public float speedRoteate = 0.9f;
    //public float Radius = 4f; 
    //public Vector2 _centre;
    //public float _angle = 45;

    //public float timeManuver = 4;

    //public Transform destination;

    [Header("ForwardToPlayer_Move")]
    public float speedMove = 4;
    public float speedRotate = 0.9f;
    public float Radius = 4f;
    public float _angle = 45;
    public float timeManuver = 4;

    public Transform[] destinations;
      
    private void Start()
    {
        //_centre = transform.position;

        //timeManuver = Time.time + timeManuver;

        StartCoroutine(ForwardMove());
         
    }

    private void Update()
    { 
    }

    IEnumerator ForwardMove()
    {
        float distance1 = Vector3.Distance(this.transform.position, destinations[0].position);
        float distance2 = Vector3.Distance(this.transform.position, destinations[1].position);

        Vector3 targetPoint = distance1 > distance2 ? destinations[0].position : destinations[1].position;
        Vector2 centre = this.transform.position;
        bool circleMoveDirection = distance1 > distance2;

        timeManuver = Time.time + timeManuver;

        while (Time.time <= timeManuver)
        {
            _angle += speedRotate * Time.deltaTime;

            var offset = GetRotateDirectionVector(clockwise: circleMoveDirection) * Radius;

            transform.position = Vector3.MoveTowards(this.transform.position, centre + offset, speedMove * Time.deltaTime);

            yield return new WaitForSeconds(0.05f);
        }
         
        Vector3 heading = targetPoint - transform.position;
        float distance = (heading).magnitude;
        Vector3 direction = heading / distance;
        Debug.Log(direction);

        GetComponent<Rigidbody2D>().AddForce(heading * speedMove * 2);
    }

    private Vector2 GetRotateDirectionVector(bool clockwise)
    {
        if (clockwise)
            return new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle));
        else
            return new Vector2(Mathf.Cos(_angle - 5), Mathf.Sin(_angle - 5));
    }

    //private IEnumerator MoveToRight()
    //{ 
    //    while (Time.time <= timeManuver)
    //    {
    //        _angle += speedRoteate * Time.deltaTime;

    //        var offset = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)) * Radius;

    //        transform.position = Vector3.MoveTowards(this.transform.position, _centre + offset, speedMove * Time.deltaTime);

    //        yield return new WaitForSeconds(0.05f); 
    //    }
    //    while (this.transform.position != destination.position)
    //    {
    //        this.transform.position = Vector3.MoveTowards(this.transform.position, destination.position, speedMove * Time.deltaTime);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //}

    //private IEnumerator MoveToLeft()
    //{

    //}
}
