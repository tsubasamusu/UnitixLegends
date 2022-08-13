using System.Collections;
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

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    public IEnumerator SetGameStart()
    {
        //視界を白色に設定
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f);

        //ロゴをゲームスタートに設定
        logo.sprite = gameStart;

        //2.0秒かけてロゴを表示
        logo.DOFade(1.0f,12.0f);

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
}
