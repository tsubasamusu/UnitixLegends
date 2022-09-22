using DG.Tweening;

namespace yamap 
{
    public class HandWeapon_Knife : HandWeaponDetailBase
    {
        /// <summary>
        /// ナイフの効果処理
        /// </summary>
        protected override void TriggerWeapon() 
        {
            //アニメーションを開始する（前後移動）
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}