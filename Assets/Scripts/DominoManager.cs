using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoManager : MonoBehaviour
{
    public float force = 1;
    private Rigidbody rb;

    Vector3 hitPos;
    AudioSource audioSource;

    private bool played = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        Vector3 trans = transform.position;
        float yoff = transform.localScale.y - 0.1f;

        hitPos = new Vector3(trans.x, yoff, trans.z);
    }

    public void MakeItFall()
    {
        Vector3 dir = -transform.parent.forward;
        rb.AddForceAtPosition(dir * force, hitPos ,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Domino"))
        {
            DominoManager dm = collision.gameObject.GetComponent<DominoManager>();
            dm.MakeItFall();
            
            if (!played)
            {
                played = true;

                // Make the Music Play Here
                audioSource.Play();
            }
        }
    }
}
