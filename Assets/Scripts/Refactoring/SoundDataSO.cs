using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

namespace yamap 
{
    /// <summary>
    /// BGM
    /// </summary>
    public enum BgmName 
    {
        Main,
    }

    /// <summary>
    /// 効果音の名前
    /// </summary>
    public enum SeName 
    {
        None,//選択しない
        KnifeSE,//ナイフで刺す音
        BatSE,//バットで殴る音
        ExplosionSE,//爆発音
        GasSE,//ガスの音
        AssaultSE,//アサルトライフルの音
        ShotgunSE,//ショットガンの音
        SniperSE,//スナイパーライフルの音
        RecoverySE,//回復音
        BulletSE,//弾のアイテムを拾ったときの音
        AirplaneSE,//飛行機の音
        FallSE,//飛行機から飛び降りる音
        LandingSE,//着地音
        RunningSE,//走る音
        BePreparedSE,//構える音
        GetItemSE,//アイテム取得音
        DiscardItemSE,//アイテム破棄音
        SelectItemSE,//アイテム選択音
        GameStartSE,//ゲーム開始音
        GameOverSE,//ゲームオーバー音
        GameClearSE,//ゲームクリア音
        NoneItemSE,//選択したアイテムがNoneの時の音
        NextShotOkSE,//発射完了音（リロード音）
        InflictDamageSE,//PlayerがEnemyにダメージを与えたときの音
    }

    /// <summary>
    /// 音のデータを管理する
    /// </summary>
    [Serializable]
    public class SeData 
    {
        public SeName seName;//効果音の名前
        public AudioClip audioClip;//オーディオクリップ
    }

    /// <summary>
    /// BGMのデータを管理する
    /// </summary>
    [Serializable]
    public class BgmData 
    {
        public BgmName bgmName;//BGMの名前
        public AudioClip audioClip;//オーディオクリップ
    }

    //アセットメニューで「Create SoundDataSO_yamap」を選択すると、「SoundDataSO」を作成できる
    [CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO_yamap")]
    public class SoundDataSO : ScriptableObject 
    {
        public List<SeData> seList = new ();//効果音のデータのリスト
        public List<BgmData> bgmList = new();//BGMのデータのリスト
    }
}