using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UIを使用
using DG.Tweening;//DOTweenを使用

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image eventHorizon;//視界

    [SerializeField]
    private Image logo;//ロゴ

    [SerializeField]
    private Sprite gameStart;//ゲームスタートロゴ

    [SerializeField]
    private Sprite gameClear;//ゲームクリアロゴ

    [SerializeField]
    private Slider hpSlider;//体力スライダー

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    public IEnumerator SetGameStart()
    {
        //視界を白色に設定
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f,0.0f);

        //ロゴをゲームスタートに設定
        logo.sprite = gameStart;

        //2.0秒かけてロゴを表示
        logo.DOFade(1.0f,2.0f);

        //ロゴの表示が終わるまで待つ
        yield return new WaitForSeconds(2.0f);

        //視界を1.0秒掛けて透明にする
        eventHorizon.DOFade(0.0f, 1.0f);

        //ロゴを1.0秒かけて消す
        logo.DOFade(0.0f, 1.0f);

        //視界とロゴの演出が終わるまで待つ
        yield return new WaitForSeconds(1.0f);

        //GameManagerからゲーム開始状態に切り替える
    }

    /// <summary>
    /// 視界を指定された時間だけ暗くする
    /// </summary>
    public IEnumerator SetEventHorizonBlack(float time)
    {
        //視界を黒色に設定
        eventHorizon.color = new Color(0.0f, 0.0f, 0.0f,0.0f);

        //1.0秒かけて視界を完全に暗くする
        eventHorizon.DOFade(1.0f, 1.0f);

        //引数で指定された時間だけ視界を暗いまま保つ
        yield return new WaitForSeconds(time);

        //0.5秒かけて視界を元に戻す
        eventHorizon.DOFade(0.0f, 0.5f);
    }

    /// <summary>
    /// 被弾した際の視界の処理
    /// </summary>
    public IEnumerator AttackEventHorizon()
    {
        //視界を赤色に設定
        eventHorizon.color = new Color(255.0f, 0.0f, 0.0f, 0.0f);

        //0.25秒かけて視界を少し赤くする
        eventHorizon.DOFade(0.5f, 0.25f);

        //視界が少し赤くなるまで待つ
        yield return new WaitForSeconds(0.25f);

        //0.25秒かけて視界を元に戻す
        eventHorizon.DOFade(0.0f, 0.25f);
    }

    /// <summary>
    /// 体力用スライダーを更新する
    /// </summary>
    public void UpdateHpSliderValue(float currentValue,float updateValue)
    {
        //0.5秒かけて体力用スライダーを更新する
        hpSlider.DOValue((currentValue + updateValue)/100.0f, 0.5f);
    }
}
