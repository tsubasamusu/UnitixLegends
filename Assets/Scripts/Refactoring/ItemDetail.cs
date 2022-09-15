using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace yamap {

    public class ItemDetail : MonoBehaviour {

        ItemDataSO.ItemData itemData;


        public void SetUpItemDetail(ItemDataSO.ItemData itemData) {
            this.itemData = itemData;
        }


        public ItemDataSO.ItemName GetItemName() {
            return itemData.itemName;
        }


        public float GetAttackPower() {
            return itemData.attackPower;
        }
    }
}