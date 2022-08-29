using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCreate : NetworkBehaviour
{

    public static itemCreate Instance;
    public static int destroyNum;

    public GameObject[] spawnArea;
    public int itemPrefabNumber;
    public bool itemch = true;

    // Start is called before the first frame update
    void Start()
    {
        destroyNum = itemPrefabNumber;
        if(RoomPlayer.MyRoomPlayer.isServer) CmdItemCreate();
        if(Instance == null) Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itemch == false)
        {
            Invoke("CmdItemCreate", 7);
            itemch = true;

        }
    }

    
    private void CmdItemCreate()
    {
        Debug.Log(itemPrefabNumber+"에서 생성");
        GameObject itemCre = Instantiate(RoomManager.singleton.spawnPrefabs[destroyNum]);
        itemCre.transform.position = new Vector3(spawnArea[destroyNum-1].transform.position.x, 0.5f, spawnArea[destroyNum - 1].transform.position.z);
        NetworkServer.Spawn(itemCre);
    }

}
