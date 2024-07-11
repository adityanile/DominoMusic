using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoManager : MonoBehaviour
{
    public float force = 1;
    private Rigidbody rb;

    Vector3 hitPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 trans = transform.position;
        float yoff = transform.localScale.y - 0.1f;

        hitPos = new Vector3(trans.x, yoff, trans.z);
    }

    public void MakeItFall()
    {
        rb.AddForceAtPosition(new Vector3(force,0,0), hitPos,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Domino"))
        {
            DominoManager dm = collision.gameObject.GetComponent<DominoManager>();
            dm.MakeItFall();
        }

        // Make the Music Play Here
    }
}
