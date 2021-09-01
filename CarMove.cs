using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarMove : MonoBehaviour
{
    private float speed;
    private Vector3 roat;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        roat = Vector3.zero;
        speed = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Jump"))
        {
            speed += (10 - speed) * Time.deltaTime;
        }
        else
        {
            speed += -speed * Time.deltaTime;
        }
        float handle = Input.GetAxis("Horizontal");
        roat.y += handle * 50 * Time.deltaTime;
        if (handle > 360)
            handle -= 360;
        else if (handle < 0)
            handle += 360;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up * -1, out hit, 1.5f))
        {
            Quaternion look = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward,
      hit.normal), hit.normal);
            look = Quaternion.RotateTowards(transform.rotation, look, 90 * Time.deltaTime);
            roat.x = look.eulerAngles.x;
            roat.z = look.eulerAngles.z;
        }
        rb.velocity = transform.forward * speed;
        rb.rotation = Quaternion.Euler(roat.x, roat.y, roat.z);
    }
}