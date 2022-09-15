public class References
{
    // GameData を GameData と ItemManager に分割。どちらもシングルトンにする

    // BulletManager の弾の生成と近接武器の武器の生成は、分岐をつくらない。メソッド内で振る舞いを変える

    // 弾と近接武器のプレファブには、クラスをアタッチする
    // 弾には BulletDetailBase 
    // 武器には、各武器の名前のクラス

    // クラスの継承は、近接武器については、子クラスは親クラスを２つ分、継承している
    // 全武器の親クラス(WeaponBase) →　弾の親クラス(BulletDetailBase) WeaponBase からみたら子クラス

    // 全武器の親クラス(WeaponBase) →　近接武器の親クラス(HandWeaponDetailBase)  WeaponBase からみたら子クラス
    //　　　　　　　　　　　　　　　　　　　　→　バット用の子クラス(HandWeapon_Bat)  WeaponBase からみたら孫クラス
    //　　　　　　　　　　　　　　　　　　　　→　ナイフ用の子クラス(HandWeapon_Knife)  WeaponBase からみたら孫クラス


}
