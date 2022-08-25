using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCreate : NetworkBehaviour
{
    public GameObject spawnArea;
    public GameObject itemPrefab;
    public static bool itemch = true;

    // Start is called before the first frame update
    void Start()
    {
        CmdItemCreate();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemch == false)
        {
            Invoke("CmdItemCreate", 5);
            itemch = true;

        }
    }

    
    [Command]
    private void CmdItemCreate()
    {
        GameObject itemCre = Instantiate(itemPrefab);
        itemCre.transform.position = spawnArea.transform.position;
        NetworkServer.Spawn(itemCre);
    }

}
