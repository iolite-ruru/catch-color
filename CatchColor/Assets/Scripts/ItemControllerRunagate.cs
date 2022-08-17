using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemControllerRunagate : MonoBehaviour
{
    [SerializeField]
    private RunagateController player;

    [SerializeField]
    private float range; //»πµÊ ∞°¥…«— ∞≈∏Æ

    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask itemLayerMask; //æ∆¿Ã≈€ ∑π¿ÃæÓø°∏∏ π›¿¿«œµµ∑œ

    [SerializeField]
    private Text textItemInfo;

    Color color;

    void Start()
    {
        SetPlayerColor(Random.Range(0, 3));
    }

    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void SetPlayerColor(int idx)
    {
        if(idx == 0) player.SetColor(MyColor.Red, Color.red);
        else if(idx == 1) player.SetColor(MyColor.Green, Color.green);
        else if (idx == 2) player.SetColor(MyColor.Blue, Color.blue);
        player.SetLayer(idx + 7);
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

                if (itemName.Equals("Red")) SetPlayerColor(0);
                else if (itemName.Equals("Green")) SetPlayerColor(1);
                else if (itemName.Equals("Blue")) SetPlayerColor(2);
                player.SetTextColor();

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
