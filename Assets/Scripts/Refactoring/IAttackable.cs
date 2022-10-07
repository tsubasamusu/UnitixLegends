/// <summary>
/// 攻撃武器用のインターフェイス
/// </summary>
public interface IAttackable
{
    //子クラスにShotBulletメソッドの実装を強制させる
    public abstract void ShotBullet(ItemDataSO.ItemData itemData);    
}
