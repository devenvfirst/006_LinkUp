using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

    private LineRenderer line1, line2, line3;

    public void CreatLine()
    {
        GameObject line = new GameObject("line1");
        line1 = line.AddComponent<LineRenderer>();
        line1.startWidth = 0.1f;
        line1.endWidth = 0.1f;
        line1.positionCount = 2;

        line = new GameObject("line2");
        line2 = line.AddComponent<LineRenderer>();
        line2.startWidth = 0.1f;
        line2.endWidth = 0.1f;
        line2.positionCount = 2;

        line = new GameObject("line3");
        line3 = line.AddComponent<LineRenderer>();
        line3.startWidth = 0.1f;
        line3.endWidth = 0.1f;
        line3.positionCount = 2;

        line1.enabled = false;
        line2.enabled = false;
        line3.enabled = false;
    }

    public void DrawLinkLine(GameObject g1, GameObject g2, int linkType, Vector3 z1, Vector3 z2)
    {
        if (linkType == 0)
        {
            line1.enabled = true;
            line1.SetPosition(0, g1.transform.position);
            line1.SetPosition(1, g2.transform.position);
        }
        else if (linkType == 1)
        {
            line1.enabled = true;
            line2.enabled = true;

            line1.SetPosition(0, g1.transform.position);
            line1.SetPosition(1, z1);

            line2.SetPosition(0,z1);
            line2.SetPosition(1, g2.transform.position);
        }
        else if (linkType == 2)
        {
            line1.enabled = true;
            line2.enabled = true;
            line3.enabled = true;

            line1.SetPosition(0, g1.transform.position);
            line1.SetPosition(1, z2);

            line2.SetPosition(0, z2);
            line2.SetPosition(1, z1);

            line3.SetPosition(0, z1);
            line3.SetPosition(1, g2.transform.position);
        }

        StartCoroutine(HideLines());
    }

    IEnumerator HideLines()
    {
        yield return new WaitForSeconds(0.2f);

        line1.enabled = false;
        line2.enabled = false;
        line3.enabled = false;
    }
}
