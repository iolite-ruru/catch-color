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

    private Text textItemInfo;
    private Text textColor;

    void Start()
    {
        textItemInfo = GameObject.Find("TextItem").GetComponent<Text>();
        textColor = GameObject.Find("TextColor").GetComponent<Text>();
        //SetPlayerColor(Random.Range(0, 3));
    }

    void Update()
    {
        CheckItem();
        TryAction();

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
                Debug.Log(itemName + " »πµÊ«‘");

                textColor.text = itemName;
                if (itemName.Equals("Red")) SetColor(0);
                else if (itemName.Equals("Green")) SetColor(1);
                else if (itemName.Equals("Blue")) SetColor(2);

                Destroy(hitInfo.transform.gameObject);
                SetItemInfo(false);
            }
        }
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
            textItemInfo.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " »πµÊ" + "<color=yellow>" + "(E)" + "</color>";
        }
        else
        {
            pickupActivated = false;
            textItemInfo.gameObject.SetActive(false);
        }
    }
}
