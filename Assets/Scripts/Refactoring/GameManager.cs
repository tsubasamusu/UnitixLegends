using System.Collections;//IEnumeratorを使用
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneを使用

namespace yamap 
{
    public class GameManager : MonoBehaviour 
    {
        [SerializeField]
        private EnemyGenerator enemyGenerator;//EnemyGenerator

        [SerializeField]
        private AirPlaneController airplaneController;//AirplaneController

        [SerializeField]
        private PlayerController playerController;//PlayerControoler

        [SerializeField]
        private UIManager uiManager;//UIManager

        [SerializeField]
        private StormController stormController;//StormController

        [SerializeField]
        private BulletManager bulletManager;

        [SerializeField]
        private Transform temporaryObjectContainerTran;

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
            SoundManager.instance.PlaySE(SeName.GameStartSE);

            //PlayerControolerを無効化
            playerController.enabled = false;

            //TODO:PlayerControllerを非活性したのに、初期設定できるの？
            playerController.SetUpPlayer(uiManager);

            //UIManagerの初期設定を行う
            uiManager.SetUpUIManager(bulletManager, playerController.PlayerHealth);

            //CanvasGroupを非表示にする
            uiManager.SetCanvasGroup(false);

            //メッセージを無効化
            uiManager.SetMessageActive(false);

            //BulletManagerの初期設定を行う
            bulletManager.SetUpBulletManager(playerController, temporaryObjectContainerTran);       

            //ItemManagerの初期設定を行う
            ItemManager.instance.SetUpItemManager(uiManager, playerController, bulletManager);

            //キル数を初期化
            GameData.instance.KillCount = 0;

            //ゲームスタート演出が終わるまで待つ
            yield return uiManager.PlayGameStart();

            //メッセージを有効化
            uiManager.SetMessageActive(true);

            //飛行機に関する設定を行う
            airplaneController.SetUpAirplane();

            //メッセージを表示
            uiManager.SetMessageText("Tap\n'Space'\nTo Fall", Color.blue);

            //Playerの行動の制御を開始
            StartCoroutine(airplaneController.ControlPlayerMovement(this, playerController));

            //Enemyの生成を開始
            StartCoroutine(enemyGenerator.GenerateEnemy(uiManager, playerController));

            //アイテムスロットの設定等を行う
            uiManager.SetUpItemSlots();

            //ストームによるダメージの監視を開始する
            StartCoroutine(CheckStormDamageAsync());
        }

        /// <summary>
        /// 飛行機から飛び降りる
        /// </summary>
        public void FallAirPlane() 
        {     
            //メッセージのテキストを空にする
            uiManager.SetMessageText("", Color.black);

            //PlayerControllerを有効化
            playerController.enabled = true;

            //CanvasGroupを表示
            uiManager.SetCanvasGroup(true);

            //テキストの表示の更新を開始
            StartCoroutine(uiManager.UpdateText());
        }

        /// <summary>
        /// ストームによるダメージを受けるかどうか調べる
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator CheckStormDamageAsync() 
        {
            //空の判定用
            bool skyFlag = false;

            //Playerが死亡していないなら繰り返す
            while (playerController.PlayerHealth.PlayerHp > 0) 
            {
                //Playerが安置内にいないなら
                while (!stormController.CheckEnshrine(playerController.transform.position)) 
                {
                    //空の判定がtrueなら
                    if (skyFlag) 
                    {
                        //悪天候に変更
                        stormController.ChangeSkyBox(PlayerStormState.InStorm);

                        //空の判定にfalseを入れる（無駄な処理を防ぐ）
                        skyFlag = false;
                    }

                    //PlayerのHpを減少させる
                    playerController.PlayerHealth.UpdatePlayerHp(-stormController.StormDamage);

                    //1秒待つ
                    yield return new WaitForSeconds(1f);

                    //PlayerのHpが0以下になったら
                    if (playerController.PlayerHealth.PlayerHp <= 0) 
                    {
                        //繰り返し処理から抜け出す
                        break;
                    }
                }

                //空の判定がfalseなら
                if (!skyFlag) 
                {
                    //快晴に変更
                    stormController.ChangeSkyBox(PlayerStormState.OutStorm);

                    //空の判定にtrueを入れる
                    skyFlag = true;
                }

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }

            //ゲームオーバー処理を開始する
            StartCoroutine(MakeGameOverAsync());
        }

        /// <summary>
        /// ゲームクリア演出を行う
        /// </summary>
        /// <returns>待ち時間</returns>
        public IEnumerator MakeGameClear() 
        {
            //ゲームクリア音を再生
            SoundManager.instance.PlaySE(SeName.GameClearSE);

            //ゲームを終了するための準備を行う
            SetUpGameOver();

            //ゲームクリア演出が終わるまで待つ
            yield return StartCoroutine(uiManager.PlayGameClear());

            //Mainシーンを読み込む
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// ゲームオーバー演出を行う
        /// </summary>
        /// <returns>待ち時間</returns>
        public IEnumerator MakeGameOverAsync() 
        {
            //ゲームオーバー音を再生
            SoundManager.instance.PlaySE(SeName.GameOverSE);

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

            //GameOverを表示する
            uiManager.DisplayGameOver();

            //PlayerControolerを無効化
            playerController.enabled = false;
        }
    }
}