using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public GameObject tile;

    public int rowNum = 14;
    public int columNum = 18;

    public static int[,] temp_map;
    public static int[,] test_map;

    public GameObject[,] tileGos;

    //贴图数组
    public Sprite[] tiles;

    public static float xMove = 0.71f;
    public static float yMove = 0.71f;

    public Transform tileParent;

    private void Awake()
    {
        FindObjectOfType<DrawLine>().CreatLine();
        Init();
        ChangeMap();
        SaveNewMap();
        BuildMap();
    }

    // Use this for initialization
    void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// 地图初始化
    /// </summary>
    private void Init()
    {
        temp_map = new int[columNum, rowNum];
        test_map = new int[columNum + 2, rowNum + 2];
        Camera.main.transform.position = new Vector3(6.73f,5.31f,-10f);
        Camera.main.orthographicSize = 6.5f;

        for (int i = 0; i < rowNum; i++)

        {
            for (int j = 0; j < columNum; j += 2)
            {
                int temp = Random.Range(0, tiles.Length);
                temp_map[j, i] = temp;
                temp_map[j + 1, i] = temp;
            }
        }
    }

    /// <summary>
    /// 打乱地图数组
    /// </summary>
    public void ChangeMap()
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < columNum; j++)
            {
                int temp = temp_map[j, i];
                int rowRandom = Random.Range(0,rowNum);
                int colRandom = Random.Range(0,columNum);
                temp_map[j, i] = temp_map[colRandom, rowRandom];
                temp_map[colRandom, rowRandom] = temp;
            }
        }
    }

    /// <summary>
    /// 更新为周围带一圈0的地图
    /// </summary>
    private void SaveNewMap()
    {
        for (int i = 0; i < rowNum + 2; i++)
        {
            for (int j = 0; j < columNum + 2; j++)
            {
                if (i == 0 || i == rowNum + 1 || j == 0 || j == columNum + 1)
                {
                    test_map[j, i] = 0;
                }
                else
                {
                    test_map[j, i] = temp_map[j - 1, i - 1];
                }
            }
        }
    }
    /// <summary>
    /// 实例化地图
    /// </summary>
    private void BuildMap()
    {
        GameObject g;
        for (int i = 0; i < rowNum + 2; i++)
        {
            for (int j = 0; j < columNum + 2; j++)
            {
                g = Instantiate(tile);

                g.transform.position = new Vector2(j * xMove, i * yMove);
                g.transform.SetParent(tileParent);

                if (i == 0 || i == rowNum + 1 || j == 0 || j == columNum + 1)
                {
                    g.GetComponent<SpriteRenderer>().enabled = false;
                    g.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    Sprite icon = tiles[test_map[j, i]];
                    g.GetComponent<SpriteRenderer>().sprite = icon;
                }
                g.GetComponent<Tile>().x = j;
                g.GetComponent<Tile>().y = i;
                g.GetComponent<Tile>().value = test_map[j, i];
            }
        }
    }
}
