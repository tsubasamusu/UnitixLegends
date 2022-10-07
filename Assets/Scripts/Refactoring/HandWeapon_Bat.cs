using UnityEngine;
using DG.Tweening;

namespace yamap 
{
    public class HandWeapon_Bat : HandWeaponDetailBase 
    {
        /// <summary>
        /// バットの効果の処理
        /// </summary>
        protected override void TriggerWeapon() 
        {
            //バットのアニメーションを開始（前後移動）
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);

            //バットのアニメーションを開始（回転）
            transform.DOLocalRotate(new Vector3(60f, 0f, 0f), 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}