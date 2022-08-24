using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneを使用
using System.Linq;//Whereメソッドを使用

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator enemyGenerator;//EnemyGenerator

    [SerializeField]
    private AirplaneController airplaneController;//AirplaneController

    [SerializeField]
    private PlayerController playerController;//PlayerControoler

    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private SoundManager soundManager;//SoundManager

    private bool isGameOver;//ゲーム完了状態かどうか

    public bool IsGameOver//isGameOver変数用のプロパティ
    {
        get { return isGameOver; }//外部からは取得処理のみを可能に
    }

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Start()
    {
        //ゲーム開始音を再生
        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.GameStartSE));

        //PlayerControolerを無効化
        playerController.enabled = false;

        //CanvasGroupを非表示にする
        uiManager.SetCanvasGroup(false);

        //メッセージを無効化
        uiManager.SetMessageActive(false);

        //キル数を初期化
        GameData.instance.KillCount = 0;

        //ゲームスタート演出を行う
        yield return StartCoroutine(uiManager.PlayGameStart());

        //メッセージを有効化
        uiManager.SetMessageActive(true);

        //飛行機に関する設定を行う
        airplaneController.SetUpAirplane();

        //Playerの行動の制御を開始
        StartCoroutine(airplaneController.ControlPlayerMovement());

        //Enemyの生成を開始
        StartCoroutine(enemyGenerator.GenerateEnemy());

        //アイテムスロットの設定等を行う
        uiManager.SetUpItemSlots();
    }

    /// <summary>
    /// ゲームクリア演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator MakeGameClear()
    {
        //ゲームクリア音を再生
        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.GameClearSE));

        //ゲームを終了するための準備を行う
        SetUpGameOver();

        //ゲームクリア演出を行う
        yield return StartCoroutine(uiManager.PlayGameClear());

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ゲームオーバー演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator MakeGameOver()
    {
        //ゲームオーバー音を再生
        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.GameOverSE));

        //ゲームを終了するための準備を行う
        SetUpGameOver();

        //ゲームオーバー演出を行う
        yield return StartCoroutine(uiManager.PlayGameOver());

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ゲームを終了するための準備を行う
    /// </summary>
    private void SetUpGameOver()
    {
        //ゲーム終了状態に切り替える
        isGameOver = true;

        //CanvasGroupを非表示にする
        uiManager.SetCanvasGroup(false);

        //全てのフロート表示を非表示にする
        uiManager.SetFloatingMessagesNotActive();

        //メッセージを無効化
        uiManager.SetMessageActive(false);
    }
}
