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
    private float range; //È¹µæ °¡´ÉÇÑ °Å¸®
    private bool pickupActivated = false;

    [SerializeField]
    private LayerMask layerMask;*/

}
