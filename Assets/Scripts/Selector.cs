using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public GameObject selectorPrefab;


    public List<Selectable> selectables = new List<Selectable>();
    public Selectable selected;

    private GameObject selectorObject;

    public Vector2Int selectorGridPos;
    public Vector2Int selectedGridPos;

    private GridManager gridManager;

    void Start()
    {
        selectorObject = Instantiate(selectorPrefab);

        gridManager = gameObject.GetComponent<GridManager>();

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Selectable");
        foreach (GameObject o in objs)
        {
            Selectable s = o.GetComponent<Selectable>();
            if (s)
            {
                selectables.Add(s);
            }
        }
    }

    void Update()
    {
        float t = Time.deltaTime;
        selectorObject.transform.Rotate(15f * t, 25f * t, 35f * t);

        if (Input.GetMouseButtonUp(0))
        {
            selected = gridManager.GetCellAt(selectorGridPos).selectable;
        }
        if(Input.GetMouseButtonUp(1) && selected)
        {
            selectedGridPos = selectorGridPos;
            selected.gridTarget = selectedGridPos;
        }
    }

    public void MoveSelector(Vector3 pos)
    {
        selectorObject.transform.position = new Vector3(pos.x + gridManager.cellDims * .1f, pos.y, pos.z + gridManager.cellDims * .1f);
        selectorGridPos = new Vector2Int(Mathf.RoundToInt(pos.x / gridManager.cellDims), Mathf.RoundToInt(pos.z / gridManager.cellDims));
    }

    private void OnDrawGizmos()
    {
        if (gridManager)
        {
            Vector2Int gPos = gridManager.GetCellAt(selectedGridPos).pos;
            Vector3 gizPos = new Vector3(gPos.x + gridManager.cellDims / 2, 1.5f, gPos.y + gridManager.cellDims / 2);
            Gizmos.DrawSphere(gizPos, 0.3f);
        }
    }
}
