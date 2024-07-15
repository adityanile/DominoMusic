using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAlongTheLine : MonoBehaviour
{
    public GameObject domino;
    private LineRenderer line;

    public float tolorence;

    public float spawnRate = 0.1f;
    private Vector3 lastPos;
    public float dominoOffset = 1;

    public Transform allDominos;
    private Transform currentParent;

    [SerializeField]
    private int count = 0;

    public List<Color32> colors;
    private Vector3[] positions;

    public List<AudioClip> tunes = new();
    int tuneCount = 0;

    GameObject lp;

    public void StartSpawn()
    {
        line = GameObject.Find("ReferenceLine").GetComponent<LineRenderer>();

        line.Simplify(tolorence);

        positions = new Vector3[line.positionCount];
        line.GetPositions(positions);

        GameObject curr = new GameObject(count.ToString());
        curr.transform.parent = allDominos;
        currentParent = curr.transform;

        tuneCount = 0;
        StartCoroutine(SpawnDominos());
    }

    IEnumerator SpawnDominos()
    {
        bool spawning = true;
        int index = 0;

        Vector3 point = positions[index++];
        lastPos = point;

        while (spawning)
        {
            if (index >= positions.Length)
            {
                spawning = false;
                count++;

                line.positionCount = 0;
                break;
            }

            yield return new WaitForSeconds(spawnRate);

            point = positions[index++];
            float dist = Vector3.Distance(lastPos, point);

            if (dist >= 1)
            {
                GetAllPoint(lastPos, point);
            }
            index++;
        }
    }


    void GetAllPoint(Vector3 a, Vector3 b)
    {
        int n = NoOfPoints(a, b);
        Vector3 temp = a;
        lastPos = temp;

        for (int i = 0; i < n; i++)
        {
            temp = GetNextPoint(a, b, n - i);
            a = temp;
            lastPos = temp;
            SpawnDomino(temp);
        }
    }

    void SpawnDomino(Vector3 pos)
    {
        GameObject i = Instantiate(domino, pos, domino.transform.rotation, currentParent);
       
        if(lp)
        i.transform.LookAt(lp.transform);

        GameObject inst = i.transform.GetChild(0).gameObject; 

        int colorIndex = UnityEngine.Random.Range(0, colors.Count);
        inst.GetComponent<MeshRenderer>().material.color = colors[colorIndex];

        int tuneIndex = tuneCount++ % tunes.Count;
        inst.GetComponent<AudioSource>().clip = tunes[tuneIndex];

        lp = inst;

    }

    Vector3 GetNextPoint(Vector3 a, Vector3 b, int n)
    {
        float x1 = ((b.x - a.x) / n) + a.x;
        float y1 = ((b.y - a.y) / n) + a.y;
        float z1 = ((b.z - a.z) / n) + a.z;

        return new Vector3(x1, y1, z1);
    }

    int NoOfPoints(Vector3 a, Vector3 b)
    {
        float dist = Vector3.Distance(a, b);
        dist /= dominoOffset;
        return Convert.ToInt32(dist);
    }

}
