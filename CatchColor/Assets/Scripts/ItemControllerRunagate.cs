using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class ItemControllerRunagate : NetworkBehaviour
{
    //[SerializeField]
    //private CharacterMover player;

    [SerializeField]
    private float range; //È¹µæ °¡´ÉÇÑ °Å¸®

    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask itemLayerMask; //¾ÆÀÌÅÛ ·¹ÀÌ¾î¿¡¸¸ ¹ÝÀÀÇÏµµ·Ï

    [SerializeField]
    private Text textItemInfo;
    [SerializeField]
    private Text textColor;

    private void Start()
    {
        if (!hasAuthority)
        {
            textColor.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (hasAuthority)
        {
            CheckItem();
            TryAction();
        }
        

    }

    private void SetColor(int idx)
    {
        MyColor color;
        if (idx == 0) color = MyColor.Red;
        else if (idx == 1) color = MyColor.Green;
        else color = MyColor.Blue;
        CharacterMover.MyPlayer.CmdSetColor(color);
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                
                string itemName = hitInfo.transform.GetComponent<ItemPickup>().item.itemName;
                //string itemNameKor = hitInfo.transform.GetComponent<ItemPickup>().item.itemNameKor;
                //Debug.Log(itemName + " È¹µæÇÔ");

                textColor.text = itemName;
                if (itemName.Equals("»¡°­")) SetColor(0);
                else if (itemName.Equals("ÃÊ·Ï")) SetColor(1);
                else if (itemName.Equals("ÆÄ¶û")) SetColor(2);

                CmdDestroyItem(hitInfo.transform.gameObject);
                
                SetItemInfo(false);
            }
        }
    }

    public void CmdDestroyItem(GameObject obj)
    {
        if (!obj) return;
        itemCreate.itemch = false;
        NetworkServer.Destroy(obj);
    }
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, itemLayerMask))
        {
            if (hitInfo.transform.tag == "Item")
                SetItemInfo(true);
        }
        else
        {
            SetItemInfo(false);
        }
    }

    private void SetItemInfo(bool flag)
    {
        if (flag)
        {
            pickupActivated = true;
            textItemInfo.gameObject.SetActive(true);
            textItemInfo.text = "<b>" + hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "</b> È¹µæ" + "<color=yellow>" + "(E)" + "</color>";
        }
        else
        {
            pickupActivated = false;
            textItemInfo.text = "";
        }
    }
}
