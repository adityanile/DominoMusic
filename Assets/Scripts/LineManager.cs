using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class LineManager : MonoBehaviour
{
    private LineRenderer line;
    Vector3 prevPos;

    public float yOffset = 0.5f;
    private Vector3 lp;

    private bool dominoHit = false;
    public SpawnAlongTheLine spawner;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lp = touch.position;
                prevPos = GetWorldPosition(lp);

                Ray ray = Camera.main.ScreenPointToRay(lp);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Domino"))
                    {
                        dominoHit = true;
                        DominoManager dm = hit.collider.GetComponent<DominoManager>();
                        dm.MakeItFall();
                        return;
                    }
                }

                line.positionCount = 1;
                line.SetPosition(0, prevPos);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
                prevPos = GetWorldPosition(lp);

                line.positionCount++;
                line.SetPosition(line.positionCount - 1, prevPos);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lp = touch.position;

                if (line.positionCount < 2)
                    return;

                if (!dominoHit)
                {
                    spawner.StartSpawn();
                }

                dominoHit = false;
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

        return new Vector3(ret.x, yOffset, ret.z);
    }


}
