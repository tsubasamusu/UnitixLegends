using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneを使用
using System.Linq;//Whereメソッドを使用
using DG.Tweening.Core.Easing;

namespace yamap {

    public class GameManager : MonoBehaviour {

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

        //[SerializeField]
        //private SoundManager soundManager;//SoundManager

        private bool isGameOver;//ゲーム完了状態かどうか

        public bool IsGameOver//isGameOver変数用のプロパティ
        {
            get { return isGameOver; }//外部からは取得処理のみを可能に
        }

        /// <summary>
        /// ゲーム開始直後に呼び出される
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator Start() {
            //ゲーム開始音を再生
            SoundManager.instance.PlaySE(SeName.GameStartSE);

            //PlayerControolerを無効化
            playerController.enabled = false;
            playerController.SetUpPlayer(uiManager);

            uiManager.SetUpUIManager(bulletManager, playerController.PlayerHealth);

            //CanvasGroupを非表示にする
            uiManager.SetCanvasGroup(false);

            //メッセージを無効化
            uiManager.SetMessageActive(false);

            bulletManager.SetUpBulletManager(playerController, temporaryObjectContainerTran);       

            ItemManager.instance.SetUpItemManager(uiManager, playerController, bulletManager);

            //キル数を初期化
            GameData.instance.KillCount = 0;

            //ゲームスタート演出を行う
            yield return StartCoroutine(uiManager.PlayGameStart());

            Debug.Log("スタート演出終了");

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

            // HP 監視
            StartCoroutine(CheckStormDamageAsync());
        }

        /// <summary>
        /// 飛行機から飛び降りる
        /// </summary>
        /// <returns>待ち時間</returns>
        public void FallAirPlane() {     
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
        private IEnumerator CheckStormDamageAsync() {
            Debug.Log("Hp の監視スタート");

            //空の判定
            bool skyFlag = false;

            //ゲーム終了状態ではないなら、繰り返される
            while (playerController.PlayerHealth.PlayerHp > 0) {
                //Playerが安置内におらず
                while (!stormController.CheckEnshrine(playerController.transform.position)) {
                    //空の判定がtrueなら
                    if (skyFlag) {

                        //悪天候に変更
                        stormController.ChangeSkyBox(PlayerStormState.InStorm);

                        //空の判定にfalseを入れる
                        skyFlag = false;
                    }

                    //PlayerのHpを減少させる
                    playerController.PlayerHealth.UpdatePlayerHp(-stormController.StormDamage);

                    //1秒待つ
                    yield return new WaitForSeconds(1f);

                    // 残り HP 確認
                    if (playerController.PlayerHealth.PlayerHp <= 0) {
                        break;
                    }
                }

                //空の判定がfalseなら
                if (!skyFlag) {
                    //快晴に変更
                    stormController.ChangeSkyBox(PlayerStormState.OutStorm);

                    //空の判定にtrueを入れる
                    skyFlag = true;
                }

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }

            Debug.Log("Hp の監視終了");

            //Playerの体力が0になったらゲームオーバー演出を行う
            StartCoroutine(MakeGameOverAsync());
        }

        /// <summary>
        /// ゲームクリア演出を行う
        /// </summary>
        /// <returns>待ち時間</returns>
        public IEnumerator MakeGameClear() {
            //ゲームクリア音を再生
            SoundManager.instance.PlaySE(SeName.GameClearSE);

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
        public IEnumerator MakeGameOverAsync() {
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
        private void SetUpGameOver() {
            //ゲーム終了状態に切り替える
            isGameOver = true;

            // UIManager に3回命令しているので、専用のメソッドを１つ、UIManager 側に作っては？
            // そうすれば、こちらからの命令は1回で済む
            uiManager.DisplayGameOver();

            //PlayerControolerを無効化
            playerController.enabled = false;
        }
    }
}