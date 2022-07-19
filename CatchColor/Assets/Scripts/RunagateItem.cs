using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="New Item/item")]
public class RunagateItem : ScriptableObject
{

    public string itemName;
    public MyColor itemColor;
    public Sprite itemImage;
    public GameObject itemPrefab;

/*    [SerializeField]
    private float range; //ȹ�� ������ �Ÿ�
    private bool pickupActivated = false;

    [SerializeField]
    private LayerMask layerMask;*/

}
