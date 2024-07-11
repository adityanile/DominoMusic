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

    [SerializeField]
    private float dominoOffset = 0.7f;

    public AudioClip[] babySharkTune;

    public Transform allDominos;
    private Transform currentParent;
    private int count = 0;

    public List<Color32> colors;

    private void Start()
    {
        babySharkTune = Resources.LoadAll<AudioClip>("BabyShark/");
    }

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

                Ray ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit) )
                {
                    if (hit.collider.CompareTag("Domino"))
                    {
                        DominoManager dm = hit.collider.GetComponent<DominoManager>();
                        dm.MakeItFall();
                        return;
                    }
                }
                
                spawning = true;
                
                GameObject curr = new GameObject(count.ToString());
                curr.transform.parent = allDominos;
                currentParent = curr.transform;

                StartCoroutine(SpawnDominos(count));
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                if (spawning)
                {
                    StopCoroutine(SpawnDominos(count));
                    spawning = false;
                    
                    count++;
                }

            }
          
        }
    }

    IEnumerator SpawnDominos(int currentIndex)
    {
        int tuneCount = 0;

        while(spawning)
        {
            yield return new WaitForSeconds(spawnRate);

            cp = new Vector3(cp.x + dominoOffset ,cp.y,cp.z);
            GameObject inst = Instantiate(domino, cp, Quaternion.identity, currentParent);

            int colorIndex = Random.Range(0, colors.Count);
            inst.GetComponent<MeshRenderer>().material.color = colors[colorIndex];

            int tuneIndex = tuneCount++ % babySharkTune.Length;
            inst.GetComponent<AudioSource>().clip = babySharkTune[tuneIndex];
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

        return new Vector3(ret.x, yOffset, ret.z);
    }
}
