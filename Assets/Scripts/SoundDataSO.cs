using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

//アセットメニューで「Create SoundDataSO」を選択すると、「SoundDataSO」を作成できる
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>
    /// 音の種類
    /// </summary>
    public enum SoundType
    {
        SoundEffect,//効果音
        BackgroundMusic//BGM
    }

    /// <summary>
    /// 効果音の名前
    /// </summary>
    public enum SoundEffectName
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
    }

    /// <summary>
    /// BGMの名前
    /// </summary>
    public enum BackgroundMusicName
    {
        None,//選択しない
        Main//メイン
    }

    /// <summary>
    /// 音のデータを管理するクラス
    /// </summary>
    [Serializable]
    public class SoundData
    { 
        public SoundType soundType;//音の種類
        public SoundEffectName soundEffectName;//効果音の名前
        public BackgroundMusicName backgroundMusicName;//BGMの名前
        public AudioClip audioClip;//オーディオクリップ
    }

    public List<SoundData> soundDataList=new List<SoundData>();//音のデータのリスト
}
