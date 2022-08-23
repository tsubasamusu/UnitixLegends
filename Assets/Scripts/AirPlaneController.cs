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
    private EnemyGenerator enemyGenerator;//EnemyGenerator

    [SerializeField]
    private KeyCode fallKey;//飛行機から飛び降りるキー

    private bool fellFromAirplane;//飛行機から落下したかどうか

    private bool endFight;//飛行機の飛行が終わったかどうか

    [SerializeField]
    private float rotSpeed;//プロペラの回転速度

    /// <summary>
    /// 飛行機に関する設定を行う
    /// </summary>
    public void SetUpAirplane()
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
        uiManager.SetMessageText("Tap\n'Space'\nTo Fall", Color.blue);

        //Playerのキャラクターを無効化
        cinemachineManager.SetPlayerCharacterActive(false);
    }

    /// <summary>
    /// Playerの行動を制御する
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator ControlPlayerMovement()
    {
        //まだ飛行機から飛び降りていないなら繰り返す
        while(!fellFromAirplane)
        {
            //Playerを常に飛行機の真下に設置
            playerTran.position = aiplanePlayerTran.position;

            //飛行が終ったら
            if (endFight)
            {
                //飛行機から飛び降りる
                FallFromAirplane();
            }

            //飛行機から飛び降りるキーが押されたら
            if (Input.GetKeyDown(fallKey))
            {
                //飛行機から飛び降りる
                FallFromAirplane();
            }

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// 飛行機から飛び降りる
    /// </summary>
    private void FallFromAirplane()
    {
        //メッセージのテキストを空にする
        uiManager.SetMessageText("", Color.black);

        //PlayerControllerを有効化
        playerController.enabled = true;

        //Playerカメラに切り替え
        cinemachineManager.SetAirplaneCameraPriority(9);

        //CanvasGroupを表示
        uiManager.SetCanvasGroup(true);

        //テキストの表示の更新を開始
        StartCoroutine(uiManager.UpdateText());

        //飛行機から飛び降りた状態に変更
        fellFromAirplane = true;
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
            //目的地を設定
            Vector3 pos = Vector3.Scale(transform.forward, new Vector3(240f, 0f, 240f))+transform.position;

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

        //飛行が終わった状態に切り替える
        endFight = true;

        //ステージ外へ飛んでいく
        transform.DOMoveX(transform.position.x+100f, 10f);

        //10秒待つ
        yield return new WaitForSeconds(10f);

        //飛行機を消す
        Destroy(gameObject);
    }
}
