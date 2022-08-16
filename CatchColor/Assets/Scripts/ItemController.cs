using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private float range; //»πµÊ ∞°¥…«— ∞≈∏Æ

    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask itemLayerMask; //æ∆¿Ã≈€ ∑π¿ÃæÓø°∏∏ π›¿¿«œµµ∑œ

    [SerializeField]
    private Text textItemInfo;

    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void SetPlayerColor(MyColor myColor, Color color)
    {
        player.myColor = myColor;
        player.color = color;
        Debug.Log("===" + ((int)myColor + 7));
        //player.ChangeColor(7);
        /*if (player is TaggerController)
        {
            ((TaggerController)player).ChangeColor(7);
        }*/
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

                if (itemName.Equals("Red")) SetPlayerColor(MyColor.Red, Color.red);
                else if (itemName.Equals("Green")) SetPlayerColor(MyColor.Green, Color.green);
                else if (itemName.Equals("Blue")) SetPlayerColor(MyColor.Blue, Color.blue);
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
