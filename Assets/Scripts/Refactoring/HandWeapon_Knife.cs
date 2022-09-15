using DG.Tweening;

namespace yamap {

    public class HandWeapon_Knife : HandWeaponDetailBase
    {
        /// <summary>
        /// ナイフの効果処理
        /// </summary>
        protected override void TriggerWeapon() {
            // ループ処理には、繰り返す回数が決まっていても、基本的にはSetLink をつけて、処理をケアする
            // この処理の再生中にプレイヤーのゲームオブジェクトが破棄されたり、シーン遷移するとエラー(警告)になるため
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}