using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

    public GameObject upGradePfb;

    public static GameObject g1, g2;

    public static int x1, x2, y1, y2, value1, value2;

    public bool select = false;

    private int linkType;

    public Vector3 z1, z2;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        //检测鼠标按下
        if (Input.GetButtonDown("Fire1"))
        {
            IsSelect();
        }
    }

    /// <summary>
    /// 是否选中游戏物体
    /// </summary>
    private void IsSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (g1 != null && g2 != null)
            {
                g1.GetComponent<SpriteRenderer>().color = Color.white;
                g2.GetComponent<SpriteRenderer>().color = Color.white;
                g1 = g2 = null;
            }
            if (select == false)
            {
                g1 = hit.transform.gameObject;
                g1.GetComponent<SpriteRenderer>().color = Color.red;
                x1 = g1.GetComponent<Tile>().x;
                y1 = g1.GetComponent<Tile>().y;
                value1 = g1.GetComponent<Tile>().value;
                select = true;
            }
            else
            {
                g2 = hit.transform.gameObject;
                g2.GetComponent<SpriteRenderer>().color = Color.red;
                x2 = g2.GetComponent<Tile>().x;
                y2 = g2.GetComponent<Tile>().y;
                value2 = g2.GetComponent<Tile>().value;
                select = false;
                IsSame();
            }
        }
    }

    /// <summary>
    /// 判断俩个点击的物体是否相同
    /// </summary>
    public void IsSame()
    {
        if (value1 == value2 && g1.transform.position != g2.transform.position)
        {
            IsLink(x1, y1, x2, y2);

            //类型相同位置错误则修改颜色
            if (g1 != null && g2 != null)
            {
                StartCoroutine(ChangeNoneState(g1, g2));
            }
        }
        else
        {
            //修改类型不同的颜色
            StartCoroutine(ChangeNoneState(g1, g2));
        }
    }

    /// <summary>
    /// 判断是否可以连成直线消除
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    private bool IsLink(int x1, int y1, int x2, int y2)
    {
        if (y1 == y2)
        {
            if (X_Link(x1, x2, y1, y2))
            {
                linkType = 0;
                StartCoroutine(DestroySelf(x1, x2, y1, y2));
                return true;
            }
        }
        else if (x1 == x2)
        {
            if (Y_Link(x1, x2, y1, y2))
            {
                linkType = 0;
                StartCoroutine(DestroySelf(x1, x2, y1, y2));
                return true;
            }
        }

        if (IsOneCornerLink(x1, y1, x2, y2))
        {
            linkType = 1;
            StartCoroutine(DestroySelf(x1, x2, y1, y2));
            return true;
        }

        if (IsTwoCornerLink(x1, y1, x2, y2))
        {
            linkType = 2;
            StartCoroutine(DestroySelf(x1, x2, y1, y2));
            return true;
        }

        return false;
    }

    //判断X轴并排
    private bool X_Link(int x1, int x2, int y1, int y2)
    {
        if (x1 > x2)
        {
            int tmp = x1;
            x1 = x2;
            x2 = tmp;
        }

        for (int i = x1 + 1; i <= x2; i++)
        {
            if (i == x2)
                return true;
            if (MapController.test_map[i, y1] != 0)
                break;
        }
        return false;
    }

    //判断Y轴并排
    private bool Y_Link(int x1, int x2, int y1, int y2)
    {
        if (y1 > y2)
        {
            int tmp = y1;
            y1 = y2;
            y2 = tmp;
        }

        for (int i = y1 + 1; i <= y2; i++)
        {
            if (i == y2)
                return true;
            if (MapController.test_map[x1, i] != 0)
                break;
        }
        return false;
    }

    /// <summary>
    /// 判断一折
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    private bool IsOneCornerLink(int x1,int y1,int x2,int y2)
    {
        if (MapController.test_map[x1, y2] == 0)
        {
            if (X_Link(x1, x2, y1, y2) && Y_Link(x1, x2, y1, y2))
            {
                z1 = new Vector3(x1*MapController.xMove,y2*MapController.yMove,0);
                return true;
            }
        }

        if (MapController.test_map[x2, y1] == 0)
        {
            if (X_Link(x1, x2, y1, y2) && Y_Link(x1, x2, y1, y2))
            {
                z1 = new Vector3(x2 * MapController.xMove, y1 * MapController.yMove, 0);
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 判断二折
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    private bool IsTwoCornerLink(int x1,int y1,int x2,int y2)
    {
        MapController mc = FindObjectOfType<MapController>();
        //右探
        for (int i = x1+1; i < mc.columNum+2; i++)
        {
            if(MapController.test_map[i,y1] == 0)
            {
                if(IsOneCornerLink(i,y1,x2,y2))
                {
                    z2 = new Vector3(i*MapController.xMove,y1*MapController.yMove);
                    return true;
                }
            }
            if(MapController.test_map[i, y1] != 0)
            {
                break;
            }
        }

        //左探
        for(int i = x1-1;i>=0;i--)
        {
            if (MapController.test_map[i, y1] == 0)
            {
                if (IsOneCornerLink(i, y1, x2, y2))
                {
                    z2 = new Vector3(i * MapController.xMove, y1 * MapController.yMove);
                    return true;
                }
            }
            if (MapController.test_map[i, y1] != 0)
            {
                break;
            }
        }

        //下探
        for (int i = y1 - 1; i >= 0; i--)
        {
            if (MapController.test_map[x1, i] == 0)
            {
                if (IsOneCornerLink(x1, i, x2, y2))
                {
                    z2 = new Vector3(x1 * MapController.xMove, i * MapController.yMove);
                    return true;
                }
            }
            if (MapController.test_map[x1, i] != 0)
            {
                break;
            }
        }

        //上探
        for (int i = y1 + 1; i < mc.rowNum + 2; i++)
        {
            if (MapController.test_map[x1, i] == 0)
            {
                if (IsOneCornerLink(x1, i, x2, y2))
                {
                    z2 = new Vector3(x1 * MapController.xMove, i * MapController.yMove);
                    return true;
                }
            }
            if (MapController.test_map[x1, i] != 0)
            {
                break;
            }
        }

        return false;
     } 

    /// <summary>
    /// 消除给定位置的图像
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="y1"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    IEnumerator DestroySelf(int x1,int x2, int y1,int y2)
    {
        FindObjectOfType<DrawLine>().DrawLinkLine(g1,g2,linkType,z1,z2);

        //随机奖励物品
        if(Random.value > 0.7f)
        {
            GameObject upgradeGo = Instantiate(upGradePfb,new Vector3(6.5f,6,0),Quaternion.identity);
            upgradeGo.GetComponent<SpriteRenderer>().sortingOrder = 1;
            string name = upgradeGo.GetComponent<UpGrade>().upgradeName;
            switch (name)
            {
                case "stop":
                    Stop();
                    break;
                case "time":
                    Time();
                    break;
                default:
                    break;
            }
        }

        yield return new WaitForSeconds(0.2f);

        Destroy(g1);
        Destroy(g2);
        MapController.test_map[x1, y1] = 0;
        MapController.test_map[x2, y2] = 0;
        x1 = x2 = y1 = y2 = value1 = value2 = 0;
    }

    /// <summary>
    /// 改变给定物体的图像颜色
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
    /// <returns></returns>
    IEnumerator ChangeNoneState(GameObject g1,GameObject g2)
    {
        yield return new WaitForSeconds(0.2f);

        g1.GetComponent<SpriteRenderer>().color = Color.white;
        g2.GetComponent<SpriteRenderer>().color = Color.white;
        g1 = g2 = null;
        x1 = x2 = y1 = y2 = value1 = value2 = 0;
    }

    private void Stop()
    {
        Debug.Log("stop do something");
    }

    private void Time()
    {
        Debug.Log("add some time");
    }
}
