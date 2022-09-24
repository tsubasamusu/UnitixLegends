using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshAgentを使用

namespace yamap 
{
    public class EnemyController : MonoBehaviour 
    {
        private NavMeshAgent agent;//NavMeshAgent

        private Animator animator;//Animator

        [SerializeField, Header("射程距離")]
        private float range;//射程距離

        [SerializeField]
        private float getItemLength;//アイテムを取得できる距離

        [SerializeField]
        private Transform enemyWeaponTran;//Enemyが武器を構える位置

        private bool didPostLandingProcessing;//着地直後の処理を行ったかどうか

        private bool gotItem;//アイテムを取得したかどうか

        private bool stopFlag;//動きを停止するかどうか

        private float enemyhp = 100f;//Enemyの体力

        private float timer;//経過時間

        private Vector3 firstPos;//初期位置

        private ItemDataSO.ItemData usedItemData;//使用しているアイテムのデータ

        private UIManager uiManager;//UIManager

        private EnemyGenerator enemyGenerator;//EnemyGenerator

        private Transform shotBulletTran;//弾を生成する位置

        private Transform playerTran;//Playerの位置

        private ItemDetail usedItemObj;//使用しているアイテムのオブジェクト

        private int nearItemNo;//最も近くにある使用可能アイテムの番号

        private int myNo;//自分自身の番号


        /// <summary>
        /// Enemyの初期設定を行う
        /// </summary>
        /// <param name="uiManager">UIManager</param>
        /// <param name="enemyGenerator">EnemyGenerator</param>
        /// <param name="player">PlayerController</param>
        /// <param name="no">Enemyの番号</param>
        public void SetUpEnemy(UIManager uiManager, EnemyGenerator enemyGenerator, PlayerController player, int no) 
        {
            //NavMeshAgentの取得に失敗したら
            if (!TryGetComponent(out agent)) 
            {
                Debug.Log("NavMeshAgentの取得に失敗");
            }
            //NavMeshAgentの取得に成功したら
            else
            {
                //NavMeshAgentを無効化
                agent.enabled = false;
            }

            //Animatorの取得に失敗したら
            if (!TryGetComponent(out animator)) 
            {
                Debug.Log("Animatorの取得に失敗");
            }
            
            //UIManagerを取得
            this.uiManager = uiManager;

            //EnemyGeneratorを取得
            this.enemyGenerator = enemyGenerator;

            //Playerの位置情報を取得
            playerTran = player.transform;

            //自分の番号を取得
            myNo = no;

            //経過時間の計測を開始
            StartCoroutine(MeasureTime());

            //発射位置を取得
            shotBulletTran = transform.GetChild(3).transform;
        }

        /// <summary>
        /// 一定時間ごとに呼び出される
        /// </summary>
        private void FixedUpdate()//全ての端末で同じ移動速度にするためにFixedUpdateメソッドを使う
        {
            //接地していなかったら
            if (!CheckGrounded()) 
            {
                //落下する
                transform.Translate(0, -GameData.instance.FallSpeed, 0);
            }
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() 
        {
            //接地していないなら
            if (!CheckGrounded()) 
            {
                //以下の処理を行わない
                return;
            }

            //まだ着地直後の処理を行っていないなら
            if (!didPostLandingProcessing) 
            {
                //NavMeshAgentを有効化
                agent.enabled = true;

                //停止距離を0に設定
                agent.stoppingDistance = 0f;

                //アニメーションの制御を開始
                StartCoroutine(ControlAnimation());

                //着地直後の処理が完了した状態に切り替える
                didPostLandingProcessing = true;
            }

            //停止状態なら
            if (stopFlag) 
            {
                //以降の処理を行わない
                return;
            }

            //まだアイテムを取得していないなら
            if (!gotItem) 
            {
                //最も近くにある使用可能アイテムの番号を設定
                SetNearItemNo();

                //目標地点を設定
                SetTargetPosition(ItemManager.instance.generatedItemTranList[nearItemNo].position);

                //アイテムを取得できる距離まで近づいたら
                if (GetLengthToNearItem(nearItemNo) <= getItemLength) 
                {
                    //アイテムを取得し、アイテムを取得済みの状態に切り替える
                    gotItem = GetItem(nearItemNo);

                    //停止距離を設定
                    agent.stoppingDistance = 30f;
                }

                //以降の処理を行わない
                return;
            }

            //使用できないアイテムを拾ってしまったら
            if (!usedItemData.enemyCanUse) 
            {
                //アイテムをまだ取得していない状態に切り替える
                gotItem = false;

                //停止距離を0に設定
                agent.stoppingDistance = 0f;

                //使用しているアイテムのオブジェクトを消す
                Destroy(usedItemObj);

                //以降の処理を行わない
                return;
            }

            //敵の位置を目標地点に設定
            SetTargetPosition(GetNearEnemyPos());

            //射線上に敵がいたら
            if (CheckEnemy()) 
            {
                //射撃する
                ShotBullet(usedItemData);
            }
        }

        /// <summary>
        /// 最も近くにいる敵の位置を取得する
        /// </summary>
        /// <returns>最も近くにいる敵の位置</returns>
        private Vector3 GetNearEnemyPos() 
        {
            //最も近くにいる敵の位置にPlayerの位置を仮に登録
            Vector3 nearPos = playerTran.position;

            //生成したEnemyの位置情報のリストの要素数だけ繰り返す
            for (int i = 0; i < enemyGenerator.generatedEnemyList.Count; i++) 
            {
                //繰り返し処理で取得した敵が自分だったら
                if (i == myNo) 
                {
                    //次の繰り返し処理に移る
                    continue;
                }

                //繰り返し処理で取得した敵が死亡していたら（nullエラー回避）
                if (enemyGenerator.generatedEnemyList[i] == null)
                {
                    //そのEnemyをリストから取り除く
                    enemyGenerator.generatedEnemyList.RemoveAt(i);

                    //次の繰り返し処理に移る
                    continue;
                }

                //リストのi番の要素の座標をposに登録
                Vector3 pos = enemyGenerator.generatedEnemyList[i].transform.position;

                //仮登録した要素と、for文で得た要素の、myPosとの距離を比較
                if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude) 
                {
                    //nearPosを再登録
                    nearPos = pos;
                }
            }

            //nearPosを返す
            return nearPos;
        }

        /// <summary>
        /// 最も近くにある使用可能アイテムの番号を設定する
        /// </summary>
        private void SetNearItemNo() 
        {
            //nullエラー回避
            if (ItemManager.instance.generatedItemTranList.Count <= 0) 
            {
                //以降の処理を行わない
                return;
            }

            //アイテムの番号
            int itemNo = 0;

            //リストの0番の要素の座標をnearPosに仮に登録
            Vector3 nearPos = ItemManager.instance.generatedItemTranList[0].position;

            //リストの要素数だけ繰り返す
            for (int i = 0; i < ItemManager.instance.generatedItemTranList.Count; i++) 
            {
                //繰り返し処理で見つけたアイテムが使用不可だったら
                if (!ItemManager.instance.generatedItemDataList[i].enemyCanUse) 
                {
                    //以降の処理は行わずに、次の繰り返しに移る
                    continue;
                }

                //リストのi番の要素の座標をposに登録
                Vector3 pos = ItemManager.instance.generatedItemTranList[i].position;

                //仮登録した要素と、for文で得た要素の、myPosとの距離を比較
                if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude) 
                {
                    //Playerの最も近くにあるアイテムの番号をiで登録
                    itemNo = i;

                    //nearPosを再登録
                    nearPos = pos;
                }
            }

            //最も近くにある使用可能アイテムの番号を設定
            nearItemNo = itemNo;
        }

        /// <summary>
        /// 最も近くにある使用可能アイテムとの距離を取得
        /// </summary>
        /// <param name="nearItemNo">最も近くにある使用可能アイテムの番号</param>
        /// <returns>最も近くにある使用可能アイテムとの距離</returns>
        private float GetLengthToNearItem(int nearItemNo) 
        {
            //最も近くにある使用可能アイテムとの距離を返す
            return Vector3.Scale((ItemManager.instance.generatedItemTranList[nearItemNo].position - transform.position), new Vector3(1, 0, 1)).magnitude;
        }

        /// <summary>
        /// 目標地点を設定する
        /// </summary>
        /// <param name="targetPos">目標地点</param>
        private void SetTargetPosition(Vector3 targetPos) 
        {
            //引数を元に、AIの目標地点を設定
            agent.destination = targetPos;
        }

        /// <summary>
        /// 接地判定を行う
        /// </summary>
        /// <returns>自身が接地していたらtrue</returns>
        public bool CheckGrounded() 
        {
            //rayの初期位置と向き（姿勢）を設定
            var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

            //rayの探索距離（長さ）を設定
            var tolerance = 0.3f;

            //rayのヒット判定（bool型）を返す
            return Physics.Raycast(ray, tolerance);
        }

        /// <summary>
        /// 射線上に敵がいるかどうか調べる
        /// </summary>
        /// <returns>射線上に敵がいたらtrue</returns>
        private bool CheckEnemy() 
        {
            //光線の初期位置と向き（姿勢）を設定
            Ray ray = new Ray(enemyWeaponTran.position, enemyWeaponTran.forward);

            //光線が何にも当たらなかったら
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, range)) 
            {
                //falseを返し、以降の処理を行わない
                return false;
            }

            //光線がPlayerかEnemyに当たったら
            if (hitInfo.transform.gameObject.CompareTag("Player") || hitInfo.transform.gameObject.CompareTag("Enemy")) 
            {
                //trueを返す
                return true;
            }

            //PlayerとEnemy以外に光線が当たったらfalseを返す
            return false;
        }

        /// <summary>
        /// アイテムを拾う
        /// </summary>
        /// <param name="nearItemNo">近くのアイテムの番号</param>
        /// <returns>アイテムを拾い終えたらtrueを返す</returns>
        public bool GetItem(int nearItemNo) 
        {
            //使用するアイテムのデータを設定
            usedItemData = ItemManager.instance.generatedItemDataList[nearItemNo];

            //取得したアイテムを配置
            usedItemObj = Instantiate(ItemManager.instance.generatedItemDataList[nearItemNo].itemPrefab, enemyWeaponTran);

            //アイテムを拾う
            ItemManager.instance.GetItem(nearItemNo, false);

            //trueを返す
            return true;
        }

        /// <summary>
        /// 経過時間を計測する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator MeasureTime() 
        {
            //無限ループ
            while (true) 
            {
                //経過時間を計測
                timer += Time.deltaTime;

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }


        /// <summary>
        /// 射撃する
        /// </summary>
        /// <param name="itemData">使用するアイテムのデータ</param>
        private void ShotBullet(ItemDataSO.ItemData itemData) 
        {
            //経過時間が連射間隔より小さいなら
            if (timer < itemData.interval) 
            {
                //以降の処理を行わない
                return;
            }

            //弾を生成し、生成した弾からBulletDetailBaseクラスを取得
            BulletDetailBase bullet = Instantiate(itemData.weaponPrefab, shotBulletTran.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0)).GetComponent<BulletDetailBase>();

            //BulletDetailBaseクラスの初期設定を行う
            bullet.SetUpBulletDetail(itemData.attackPower, BulletOwnerType.Enemy, enemyWeaponTran.forward, itemData.seName, itemData.interval, itemData.effectPrefab);

            //生成した弾の親を自身に設定
            bullet.transform.parent = transform;

            //経過時間を初期化
            timer = 0;
        }

        /// <summary>
        /// 他のコライダーに触れた際に呼び出される
        /// </summary>
        /// <param name="collision">接触相手</param>
        private void OnCollisionEnter(Collision collision) 
        {
            //自分が撃った弾を被弾したなら
            if (collision.transform.parent == transform) 
            {
                //以降の処理を行わない
                return;
            }

            //弾に触れたら
            if (collision.gameObject.TryGetComponent(out BulletDetailBase bulletDetail)) 
            {
                //触れた弾の攻撃力を取得
                float attackPower = bulletDetail.GetAttackPower();

                //その弾の持ち主がPlayerなら
                if (bulletDetail.BulletOwnerType == BulletOwnerType.Player) 
                {
                    //フロート表示を生成 -> なるべく外からやらない
                    //TODO:「外部から直接コルーチンを開始するのは避けるべき」ということか？)
                    uiManager.PrepareGenerateFloatingMessage(Mathf.Abs(attackPower).ToString("F0"), Color.yellow);
                }

                //Enemyの体力を更新
                UpdateEnemyHp(attackPower, bulletDetail.BulletOwnerType);

                //弾の追加効果を行う
                bulletDetail.AddTriggerBullet(this);
            }
        }

        /// <summary>
        /// Enemyの体力を更新する
        /// </summary>
        /// <param name="updateValue">体力の更新量</param>
        /// <param name="bulletOwnerType">弾の所有者</param>
        private void UpdateEnemyHp(float updateValue, BulletOwnerType bulletOwnerType = BulletOwnerType.Player) 
        {
            //Enemyの体力を0以上100以下に制限しながら、更新する
            enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

            //Enemyの体力が0になったら
            if (enemyhp == 0.0f) 
            {
                //攻撃してきた相手がPlayerなら
                if (bulletOwnerType == BulletOwnerType.Player)
                {
                    //Playerが倒した敵の数に1を加える
                    yamap.GameData.instance.KillCount++;
                }

                //自身を消す
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 催涙弾効果の準備を行う
        /// </summary>
        public void PrepareTearGasGrenade() 
        {
            //催涙弾を受けた際の処理を開始する
            StartCoroutine(AttackedByTearGasGrenade());
        }

        /// <summary>
        /// 催涙弾を受けた際の処理
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator AttackedByTearGasGrenade() 
        {
            //停止状態にする
            stopFlag = true;

            //目標地点を自身の座標に設定
            SetTargetPosition(transform.position);

            //5.0秒間、動きを止め続ける
            yield return new WaitForSeconds(5.0f);

            //停止状態を解除する
            stopFlag = false;
        }

        /// <summary>
        /// 死亡処理
        /// </summary>
        private void WasKilled() 
        {
            //死亡処理
        }

        /// <summary>
        /// アニメーションを制御する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator ControlAnimation() 
        {
            //無限に繰り返す
            while (true) 
            {
                //初期位置を設定
                firstPos = transform.position;

                //0.1秒待つ
                yield return new WaitForSeconds(0.1f);

                //現在位置を設定
                Vector3 currentPos = transform.position;

                //速度を取得
                float velocity = (currentPos - firstPos).magnitude / 0.1f;

                //走る
                animator.SetBool("MovePrevious", velocity > 0.1f);

                //何もしない
                animator.SetBool("Idle", velocity <= 0.1f);

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }
    }
}