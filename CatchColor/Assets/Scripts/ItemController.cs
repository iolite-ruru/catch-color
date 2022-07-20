using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    private float range; //ȹ�� ������ �Ÿ�

    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask; //������ ���̾�� �����ϵ���

    [SerializeField]
    private Text text;

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
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " ȹ����");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item") ItemInfoAppear();
           
        }
        else
        {
            InfoDisappear();
        }
    }
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        text.gameObject.SetActive(true);
        text.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName +" ȹ��"+"<color=yellow>"+"(E)" +"</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        text.gameObject.SetActive(false);
    }
}
