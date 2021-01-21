using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private GameObject m_White_Tile;
    private GameObject m_Grey_Tile;
    private GameObject m_Wall2;
    private GameObject m_Moveing_Spike;
    private GameObject m_Smashing_Spike;
    private GameObject m_Regard;
    private GameObject m_Arrow_Box;

    private int pr_Empty_Tile = 0;
    private int pr_Moving_Spike = 0;
    private int pr_Smashing_Spike = 0;
    private int pr_Regard = 2;
    private int num_Arrow_Box = 0;

    private PlayerControl m_PlayerControl;

    public List<GameObject[]> mapData = new List<GameObject[]>();

    private Color color_White_Tile;
    private Color color_Grey_Tile;
    private Color color_Wall;

    private Color color_White_Tile_1;
    private Color color_Grey_Tile_1;
    private Color color_Wall_1;

    public Vector3 setPoint = Vector3.zero;
    private Transform m_Transform;

    public int index = 0;

    public bool canCreateBox = false;

    bool[] oddRow = new bool[6];
    bool[] evenRow = new bool[5];
    // Start is called before the first frame update
    void Start()
    {
        m_White_Tile = Resources.Load("tile_white") as GameObject;
        m_Grey_Tile = Resources.Load("tile_grey") as GameObject;
        m_Wall2 = Resources.Load("wall2") as GameObject;
        m_Moveing_Spike = Resources.Load("moving_spikes") as GameObject;
        m_Smashing_Spike = Resources.Load("smashing_spikes") as GameObject;
        m_Regard = Resources.Load("gem 2") as GameObject;
        m_Arrow_Box = Resources.Load("ArrowBox") as GameObject;

        m_Transform = gameObject.GetComponent<Transform>();

        m_PlayerControl = GameObject.Find("Player").GetComponent<PlayerControl>();

        color_White_Tile = new Color(22 / 255f, 201 / 255f, 1);
        color_Grey_Tile = new Color(0, 246 / 255f, 239 / 255f);
        color_Wall = new Color(83 / 255f, 93 / 255f, 169 / 255f);

        color_White_Tile_1 = new Color(246 / 255f, 231 / 255f, 0);
        color_Grey_Tile_1 = new Color(1, 250 / 255f, 22 / 255f);
        color_Wall_1 = new Color(98 / 255f, 251 / 255f, 17 / 255f);

        ResetBoolRow();
        CreateMapElement(setPoint);
        

    }

    private void ResetBoolRow()
    {
        oddRow[0] = oddRow[5] = false;
        oddRow[1] = oddRow[2] = oddRow[3] = oddRow[4] = true;
        evenRow[0] = evenRow[1] = evenRow[2] = evenRow[3] = evenRow[4] = true;
    }
    private void EvenToOdd()
    {
        for (int i = 1; i < 5; i++)
            oddRow[i] = evenRow[i - 1] || evenRow[i];
    }
    private void OddToEven()
    {
        for (int i = 0; i < 5; i++)
            evenRow[i] = oddRow[i] || oddRow[i + 1];
    }
    public void CreateMapElement(Vector3 SetPoint)
    {
        float times = 0.254f * Mathf.Sqrt(2);
        num_Arrow_Box = 0;
        for (int i = 0; i < 10; i++)
        {
            EvenToOdd();
            GameObject[] temp_Array1 = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                Vector3 pos = new Vector3(j * times, 0, i * times)+SetPoint;
                Vector3 vecRot = new Vector3(-90, 45, 0);
                GameObject singleTile = null;
                if (j == 0 || j == 5)
                {
                    if (CreateArrowBox()&&canCreateBox)
                    {
                        int vc = Random.Range(5, 15) / 5;
                        if(j == 5)
                            vecRot += new Vector3(0, 90 * vc, 0);
                        else
                            vecRot += new Vector3(0, -90 * (vc-1), 0);
                        singleTile = GameObject.Instantiate(m_Arrow_Box, pos+new Vector3(0,0.125f,0), Quaternion.Euler(vecRot));

                    }
                    else
                    {
                        singleTile = GameObject.Instantiate(m_Wall2, pos, Quaternion.Euler(vecRot));
                        singleTile.GetComponent<MeshRenderer>().material.color = color_Wall;

                    }
                    oddRow[j] = false;
                }
                else
                {
                    int pr = CalcPR();
                    if (pr == 0)
                    {
                        singleTile = GameObject.Instantiate(m_White_Tile, pos, Quaternion.Euler(vecRot));
                        singleTile.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = color_White_Tile;
                        singleTile.GetComponent<MeshRenderer>().material.color = color_White_Tile;
                        if (CalcGemPR() == 1)
                        {
                            CreateGem(pos + new Vector3(0, 0.06f, 0), singleTile.GetComponent<Transform>());
                        }
                    }
                    else if (pr == 1)
                    {
                        singleTile = new GameObject();
                        singleTile.GetComponent<Transform>().position = pos;
                        singleTile.GetComponent<Transform>().rotation = Quaternion.Euler(vecRot);
                        oddRow[j] = false;
                    }
                    else if (pr == 2)
                    {
                        singleTile = GameObject.Instantiate(m_Moveing_Spike, pos, Quaternion.Euler(vecRot));
                    }
                    else if (pr == 3)
                    {
                        singleTile = GameObject.Instantiate(m_Smashing_Spike, pos, Quaternion.Euler(vecRot));
                    }
                }
                singleTile.GetComponent<Transform>().SetParent(m_Transform);
                temp_Array1[j] = singleTile;
            }
            if(!(oddRow[1]|| oddRow[2] || oddRow[3] || oddRow[4]))
            {
                int m = 0;
                while (!evenRow[m] && m <= 4)
                {
                    m++;
                    Debug.Log(m);
                }
                    int n = m + Random.Range(0, 2);
                if (n == 0)
                    n = 1;
                if (n == 5)
                    n = 4;
                oddRow[n] = true;
                Vector3 pos = new Vector3(n * times, 0, i * times) + SetPoint;
                Vector3 vecRot = new Vector3(-90, 45, 0);
                GameObject singleTile = GameObject.Instantiate(m_White_Tile, pos, Quaternion.Euler(vecRot));
                singleTile.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = color_White_Tile;
                singleTile.GetComponent<MeshRenderer>().material.color = color_White_Tile;
                temp_Array1[n] = singleTile;
            }
            mapData.Add(temp_Array1);
            color_White_Tile = ChangeColor_1(color_White_Tile);
            color_Wall = ChangeColor_3(color_Wall);

            OddToEven();
            GameObject[] temp_Array2 = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(times / 2 + j * times, 0, times / 2 + i * times)+SetPoint;
                Vector3 vecRot = new Vector3(-90, 45, 0);
                GameObject singleTile = null;
                int pr = CalcPR();
                if (pr == 0)
                {
                    singleTile = GameObject.Instantiate(m_White_Tile, pos, Quaternion.Euler(vecRot));
                    singleTile.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = color_Grey_Tile;
                    singleTile.GetComponent<MeshRenderer>().material.color = color_Grey_Tile;
                }
                else if (pr == 1)
                {
                    singleTile = new GameObject();
                    singleTile.GetComponent<Transform>().position = pos;
                    singleTile.GetComponent<Transform>().rotation = Quaternion.Euler(vecRot);
                    evenRow[j] = false;
                }
                else if (pr == 2)
                {
                    singleTile = GameObject.Instantiate(m_Moveing_Spike, pos, Quaternion.Euler(vecRot));
                }
                else if (pr == 3)
                {
                    singleTile = GameObject.Instantiate(m_Smashing_Spike, pos, Quaternion.Euler(vecRot));
                }
                singleTile.GetComponent<Transform>().SetParent(m_Transform);
                temp_Array2[j] = singleTile;
            }
            if (!(evenRow[0] || evenRow[1] || evenRow[2] || evenRow[3]|| evenRow[4]))
            {
                int m = 0;
                while (!oddRow[m])
                    m++;
                int n = m - Random.Range(0, 2);
                oddRow[n] = true;
                Vector3 pos = new Vector3(times / 2 + n * times, 0, times / 2 + i * times) + SetPoint;
                Vector3 vecRot = new Vector3(-90, 45, 0);
                GameObject singleTile = GameObject.Instantiate(m_White_Tile, pos, Quaternion.Euler(vecRot));
                singleTile.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = color_Grey_Tile;
                singleTile.GetComponent<MeshRenderer>().material.color = color_Grey_Tile;
                temp_Array1[n] = singleTile;
            }
            mapData.Add(temp_Array2);
            color_Grey_Tile = ChangeColor_2(color_Grey_Tile);
            setPoint = new Vector3(0, 0, mapData.Count / 2 * 0.254f * Mathf.Sqrt(2));
        }
       }
    private Color ChangeColor_1(Color sor)
    {
        Color tar = Color.Lerp(sor, color_White_Tile_1, Time.deltaTime*0.5f);
        return tar;
    }
    private Color ChangeColor_2(Color sor)
    {
        Color tar = Color.Lerp(sor, color_Grey_Tile_1, Time.deltaTime*0.5f);
        return tar;
    }
    private Color ChangeColor_3(Color sor)
    {
        Color tar = Color.Lerp(sor, color_Wall_1, Time.deltaTime*0.5f);
        return tar;
    }

    private bool CreateArrowBox()
    {
        if (num_Arrow_Box < 2)
        {
            if (Random.Range(0, 100) < 10)
            {
                num_Arrow_Box++;
                return true;
            }
            else return false;
        }
        else return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowMapInfo();
    }

    void ShowMapInfo()
    {
        for(int i =0;i<mapData.Count;i++)
            for(int j =0;j<mapData[i].Length;j++)
            {
                mapData[i][j].name = i + "-" + j;
            }
    }

    public void StartDown()
    {
        StartCoroutine("TillDown");
    }

    public void StopDown()
    {
        StopCoroutine("TillDown");
    }

    private IEnumerator TillDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (GameObject item in mapData[index])
            {
                Rigidbody temp = item.AddComponent<Rigidbody>();
                temp.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f));
                GameObject.Destroy(item, 1.5f);
            }
            if (index == m_PlayerControl.x1)
            {
                StopDown();
                m_PlayerControl.gameObject.AddComponent<Rigidbody>();
                m_PlayerControl.StartCoroutine("GameOver", true);
            }
            index++;
        }
    }

    private int CalcPR()
    {
        int i = Random.Range(0, 100);
        if (i <= 30 && i <= pr_Empty_Tile && i > 0)
            return 1;
        else if (i > 30 && i <= 30 + pr_Moving_Spike && i <= 60)
            return 2;
        else if (i > 60 && i <= 90 && i <= 60 + pr_Smashing_Spike)
            return 3;
        else
            return 0;
    }
    public void AddPr()
    {
        pr_Empty_Tile += 2;
        pr_Moving_Spike++;
        pr_Smashing_Spike++;
    }
    
    private int CalcGemPR()
    {
        int i = Random.Range(1, 100);
        if (i <= pr_Regard)
            return 1;
        else
            return 0;
    }
    private void CreateGem(Vector3 pos,Transform tran)
    {
        GameObject temp = GameObject.Instantiate(m_Regard, pos+new Vector3(0,0.06f,0), Quaternion.identity);
        temp.GetComponent<Transform>().SetParent(tran);
    }
    public void ResetMap()
    {
        Transform[] sonTransform = m_Transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < sonTransform.Length; i++)
            GameObject.Destroy(sonTransform[i].gameObject);
        pr_Empty_Tile = 0;
        pr_Moving_Spike = 0;
        pr_Smashing_Spike = 0;
        index = 0;
        canCreateBox = false;
        mapData.Clear();
        setPoint = Vector3.zero;
        color_White_Tile = new Color(22 / 255f, 201 / 255f, 1);
        color_Grey_Tile = new Color(0, 246 / 255f, 239 / 255f);
        color_Wall = new Color(83 / 255f, 93 / 255f, 169 / 255f);
        CreateMapElement(setPoint);
        ResetBoolRow();
    }
}
