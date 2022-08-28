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
            Invoke("CmdItemCreate", 1);
            itemch = true;

        }
    }

    
    private void CmdItemCreate()
    {
        GameObject itemCre = Instantiate(itemPrefab);
        itemCre.transform.position = new Vector3(spawnArea.transform.position.x, 0.5f, spawnArea.transform.position.z);
        NetworkServer.Spawn(itemCre);
    }

}
