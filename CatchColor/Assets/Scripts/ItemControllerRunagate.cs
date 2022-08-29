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
    private float range; //»πµÊ ∞°¥…«— ∞≈∏Æ

    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask itemLayerMask; //æ∆¿Ã≈€ ∑π¿ÃæÓø°∏∏ π›¿¿«œµµ∑œ

    [SerializeField]
    private Text textItemInfo;
    [SerializeField]
    private Text textColor;
    [SerializeField]
    private Text textRole;

    private void Start()
    {
        if (!hasAuthority)
        {
            textItemInfo.gameObject.SetActive(false);
            textColor.gameObject.SetActive(false);
            textRole.gameObject.SetActive(false);
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
        //Debug.Log(color + " »πµÊ«‘!");
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
                //Debug.Log(itemName + " »πµÊ«‘");

                textColor.text = itemName;
                if (itemName.Equals("Red")) SetColor(0);
                else if (itemName.Equals("Green")) SetColor(1);
                else if (itemName.Equals("Blue")) SetColor(2);

                if(RoomPlayer.MyRoomPlayer.isClient) CmdDestroyItem(hitInfo.transform.gameObject);
                else DestroyItem(hitInfo.transform.gameObject);
                SetItemInfo(false);
            }
        }
    }

    [Command]
    public void CmdDestroyItem(GameObject obj)
    {
        if (!obj) return;
        Debug.Log(itemCreate.Instance.itemPrefabNumber + "ø°º≠ ªË¡¶");
        itemCreate.Instance.itemch = false;
        if (obj.gameObject.name == "item_red(Clone)") itemCreate.destroyNum = 1;
        else if (obj.gameObject.name == "item_green(Clone)") itemCreate.destroyNum = 2;
        else if (obj.gameObject.name == "item_blue(Clone)") itemCreate.destroyNum = 3;
        NetworkServer.Destroy(obj);
    }

    public void DestroyItem(GameObject obj)
    {
        if (!obj) return;
        Debug.Log(itemCreate.Instance.itemPrefabNumber + "ø°º≠ ªË¡¶");
        itemCreate.Instance.itemch = false;
        if (obj.gameObject.name == "item_red(Clone)") itemCreate.destroyNum = 1;
        else if (obj.gameObject.name == "item_green(Clone)") itemCreate.destroyNum = 2;
        else if (obj.gameObject.name == "item_blue(Clone)") itemCreate.destroyNum = 3;
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
            textItemInfo.text = "<b>" + hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "</b> »πµÊ" + "<color=yellow>" + "(E)" + "</color>";
        }
        else
        {
            pickupActivated = false;
            textItemInfo.text = "";
        }
    }
}
