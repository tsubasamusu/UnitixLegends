using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;

namespace yamap {

    public class EnemyGenerator : MonoBehaviour {

        [SerializeField]
        private PlayerController player;

        [SerializeField]
        private UIManager uIManager;//UIManager

        //[SerializeField]
        //private Transform enemyPrefab;//Enemyのプレファブ

        [SerializeField]
        private EnemyController enemyPrefab;//Enemyのプレファブ

        [SerializeField]
        private int maxGenerateCount;//Enemyの最大生成数

        [SerializeField]
        private float flightTime;//飛行機の飛行時間

        [SerializeField]
        private Transform enemiesTran;//Enemyの親オブジェクト

        [HideInInspector]
        public List<EnemyController> generatedEnemyList = new List<EnemyController>();//生成したEnemyのリスト

        private List<float> generateTimeList = new List<float>();//Enemyを生成する時間のリスト

        private float timer;//経過時間

        private int generateCount;//Enemyの生成数

        /// <summary>
        /// Enemyを生成する
        /// </summary>
        /// <returns>待ち時間</returns>
        public IEnumerator GenerateEnemy() {
            //Enemyの最大生成数だけ繰り返す
            for (int i = 0; i < maxGenerateCount; i++) {
                //Enemyを生成する時間のリストを完成させる
                generateTimeList.Add(Random.Range(1f, flightTime));
            }

            //無限に繰り返す
            while (true) {
                //経過時間を計測
                timer += Time.deltaTime;

                //Enemyを生成する時間のリストの要素数だけ繰り返す
                for (int i = 0; i < generateTimeList.Count; i++) {
                    //経過時間が生成時間以上になったら
                    if (timer >= generateTimeList[i]) {

                        EnemyController enemy = Instantiate(enemyPrefab, transform.position, enemiesTran.rotation);

                        enemy.SetUpEnemy(uIManager, this, player, generateCount);

                        generatedEnemyList.Add(enemy);

                        //生成数に1を加える
                        generateCount++;

                        //Enemyを生成し、親を設定
                        //Transform enemyTran = Instantiate(enemyPrefab, enemiesTran);

                        //生成したEnemyの場所を調整
                        //enemyTran.position = transform.position;

                        //生成したEnemyをリストに加える
                        //generatedEnemyList.Add(enemyTran.gameObject);

                        ////生成数に1を加える
                        //generateCount++;

                        ////生成したEnemyからEnemyControllerを取得
                        //if (enemyTran.gameObject.TryGetComponent(out EnemyController enemyController)) {
                        //    //生成したEnemyに番号を持たせる
                        //    enemyController.MyNo = generateCount - 1;
                        //}
                        ////EnemyControllerの取得に失敗したら
                        //else {
                        //    //問題を報告
                        //    Debug.Log("EnemyControllerの取得に失敗");
                        //}

                        //Enemyの数を更新
                        uIManager.UpdateTxtOtherCount(enemiesTran.childCount);

                        //Enemyを生成する時間のリストからその要素を排除
                        generateTimeList.RemoveAt(i);
                    }
                }

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }
    }
}