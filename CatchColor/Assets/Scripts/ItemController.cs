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
    private LayerMask layerMask; //æ∆¿Ã≈€ ∑π¿ÃæÓø°∏∏ π›¿¿«œµµ∑œ

    [SerializeField]
    private Text textItemInfo;

    void Update()
    {
        CheckItem();
        TryAction();
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
            if(hitInfo.transform != null)
            {
                string itemName = hitInfo.transform.GetComponent<ItemPickup>().item.itemName;
                Debug.Log(itemName + " »πµÊ«‘");
                
                if (itemName.Equals("Red")) player.MyColor = MyColor.Red;
                else if (itemName.Equals("Green")) player.MyColor = MyColor.Green;
                else if (itemName.Equals("Blue")) player.MyColor = MyColor.Blue;
                player.SetTextColor();

                Destroy(hitInfo.transform.gameObject);
                DisappearItemInfo();
            }
        }
    }
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item") AppearItemInfo();
           
        }
        else
        {
            DisappearItemInfo();
        }
    }
    private void AppearItemInfo()
    {
        pickupActivated = true;
        textItemInfo.gameObject.SetActive(true);
        textItemInfo.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName +" »πµÊ"+"<color=yellow>"+"(E)" +"</color>";
    }

    private void DisappearItemInfo()
    {
        pickupActivated = false;
        textItemInfo.gameObject.SetActive(false);
    }
}
