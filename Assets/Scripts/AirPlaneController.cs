using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    private Transform propellerTran;//プロペラの位置情報

    [SerializeField]
    private Transform playerTran;//Playerの位置

    [SerializeField]
    private Transform aiplanePlayerTran;//飛行機でのPlayerの位置

    [SerializeField]
    private PlayerController playerController;//PlayerController

    [SerializeField]
    private CinemachineManager cinemachineManager;//CinemachineManager

    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private KeyCode fallKey;//飛行機から飛び降りるキー

    private bool fellFromAirplane;//飛行機から落下したかどうか

    [SerializeField]
    private float rotSpeed;//プロペラの回転速度

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //PlayerControllerを無効化
        playerController.enabled = false;

        //飛行機を初期位置に配置
        transform.position = new Vector3(120f, 100f, -120f);

        //プロペラの回転を開始
        StartCoroutine(RotatePropeller());

        //飛行機の操縦を開始
        StartCoroutine(NavigateAirplane());

        //メッセージを表示
        uiManager.SetMessageText("Tap\n'Space'\nTo Fall",Color.blue);
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //既に飛行機から飛び降りたのなら
        if (fellFromAirplane)
        {
            //以降の処理を行わない
            return;
        }

        //Playerを常に飛行機の真下に設置
        playerTran.position=aiplanePlayerTran.position;

        //飛行機から飛び降りるキーが押されたら
        if (Input.GetKeyDown(fallKey))
        {
            //メッセージのテキストを空にする
            uiManager.SetMessageText("", Color.black);

            //PlayerControllerを有効化
            playerController.enabled=true;

            //Playerカメラに切り替え
            cinemachineManager.SetAirplaneCameraPriority(9);

            //飛行機から飛び降りた状態に変更
            fellFromAirplane = true;
        }
    }

    /// <summary>
    ///飛行機のプロペラを回転させる
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator RotatePropeller()
    {
        //無限ループ
        while (true)
        {
            //プロペラを回す
            propellerTran.Rotate(0f, rotSpeed, 0f);

            //一定時間待つ（実質、FixedUpdateメソッド）
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 飛行機を操縦する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator NavigateAirplane()
    {
        //4回繰り返す
        for (int i = 0; i < 4; i++)
        {
            //目標値
            Vector3 pos = Vector3.zero;

            //繰り返し回数に応じて目的地を変更
            switch (i)
            {
                case 0:
                    pos = new Vector3(120f, 100f, 120f);
                    break;
                case 1:
                    pos = new Vector3(-120f, 100f, 120f);
                    break;
                case 2:
                    pos = new Vector3(-120f, 100f, -120f);
                    break;
                case 3:
                    pos = new Vector3(120f, 100f, -120f);
                    break;
            }

            //10秒かけて前進
            transform.DOMove(pos, 10f).SetEase(Ease.Linear);

            //10秒待つ
            yield return new WaitForSeconds(10f);

            //4回目の繰り返し処理なら
            if(i==3)
            {
                //繰り返し処理を終了
                break;
            }

            //1秒かけて旋回
            transform.DORotate(new Vector3(0f, (float)-90*(i+1), 0f), 1f).SetEase(Ease.Linear);

            //1秒待つ
            yield return new WaitForSeconds(1f);
        }

        //ステージ外へ飛んでいく
        transform.DOMoveX(transform.position.x+100f, 10f);

        //10秒待つ
        yield return new WaitForSeconds(10f);

        //飛行機を消す
        Destroy(gameObject);
    }
}
