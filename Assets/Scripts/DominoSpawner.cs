using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DominoSpawner : MonoBehaviour
{
    private Vector3 cp;
    public GameObject domino;

    private bool spawning = false;

    [SerializeField]
    private float spawnRate = 1f;
    [SerializeField]
    private float yOffset = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
           
            if(touch.phase == TouchPhase.Began)
            {
                Vector3 pos = touch.position;
                cp = GetWorldPosition(pos);
                
                spawning = true;
                StartCoroutine(SpawnDominos());
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                Vector3 pos = touch.position;
                cp = GetWorldPosition(pos);

            }
            else if(touch.phase == TouchPhase.Ended)
            {
                StopCoroutine(SpawnDominos());
                spawning = false;
            }
          
        }
    }

    Vector3 GetWorldPosition(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        
        RaycastHit hit;
        Vector3 ret = new Vector3();
        
        if (Physics.Raycast(ray, out hit))
        {
            ret = hit.point;
        }

        return new Vector3(ret.x,yOffset, ret.z);
    }

    IEnumerator SpawnDominos()
    {
        while(spawning)
        {
            yield return new WaitForSeconds(spawnRate);

            Instantiate(domino, cp, Quaternion.identity);
        }
    }
}
