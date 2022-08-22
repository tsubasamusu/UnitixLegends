using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshAgentを使用

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;//NavMeshAgent

    [SerializeField]
    private Animator animator;//Animator

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    [SerializeField, Header("射程距離")]
    private float range;//射程距離

    [SerializeField]
    private float stoppingDistance;//敵との距離

    [SerializeField]
    private float getItemLength;//アイテムを取得できる距離

    [SerializeField,Header("安置への移動指示を出し続ける時間")]
    private float instructionTime;//安置への移動指示を出し続ける時間

    [SerializeField]
    private Transform enemyWeaponTran;//Enemyが武器を構える位置

    private bool didPostLandingProcessing;//着地直後の処理を行ったかどうか

    private bool gotItem;//アイテムを取得したかどうか

    private bool stopFlag;//動きを停止するかどうか

    private bool goToEnshrine;//安置に移動するかどうか

    private float enemyhp = 100f;//Enemyの体力

    private float timer;//武器用の経過時間

    private float stormTimer;//ストームの中にいる間の経過時間

    private Vector3 firstPos;//初期位置

    private ItemDataSO.ItemData usedItemData;//使用しているアイテムのデータ

    private UIManager uiManager;//UIManager

    private EnemyGenerator enemyGenerator;//EnemyGenerator

    private StormController stormController;//StormController

    private PlayerHealth playerHealth;//PlayerHealth

    private Transform shotBulletTran;//弾を生成する位置

    private Transform playerTran;//Playerの位置

    private GameObject usedItemObj;//使用しているアイテムのオブジェクト

    private int nearItemNo;//最も近くにある使用可能アイテムの番号

    private int myNo;//自分自身の番号

    public int MyNo//myNo変数用のプロパティ
    {
        set { myNo = value; }//外部からは設定処理のみを可能に
    }

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //NavMeshAgentを無効化
        agent.enabled = false;

        //経過時間を計測
        StartCoroutine(MeasureTime());

        //UIManagerを取得
        if (!GameObject.Find("UIManager").TryGetComponent(out uiManager))
        {
            //問題を報告
            Debug.Log("UIManagerの取得に失敗");
        }

        //EnemyGeneratorを取得
        if(!GameObject.Find("EnemyGenerator").TryGetComponent(out enemyGenerator))
        {
            //問題を報告
            Debug.Log("EnemyGeneratorの取得に失敗");
        }

        //StormControllerを取得
        if (!GameObject.Find("Storm").TryGetComponent(out stormController))
        {
            //問題を報告
            Debug.Log("StormControllerの取得に失敗");
        }

        //PlayerHealthを取得
        if (!GameObject.Find("Player").TryGetComponent(out playerHealth))
        {
            //問題を報告
            Debug.Log("PlayerHealthの取得に失敗");
        }

        //Playerの位置情報を取得
        if (!GameObject.Find("Player").TryGetComponent(out playerTran))
        {
            //問題を報告
            Debug.Log("Playerの位置情報の取得に失敗");
        }

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
        //裏世界に行ってしまったら
        if (transform.position.y <= -1f)
        {
            //問題を報告
            Debug.Log("Enemyが裏世界に落下");

            //自身の座標を(0,0,0)に設定
            transform.position = Vector3.zero;
        }

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
        if(stopFlag)
        {
            //以降の処理を行わない
            return;
        }

        //安置への移動指示が出ているなら
        if (goToEnshrine)
        {
            //中央へ向かう
            SetTargetPosition(Vector3.zero);

            //以降の処理を行わない
            return;
        }

        //安置外にいるなら
        if (!stormController.CheckEnshrine(transform.position))
        {
            //ストームにいる間の経過時間を計測
            stormTimer+=Time.deltaTime;

            //経過時間が一定時間を超えたら
            if(stormTimer>=(100f/playerHealth.StormDamage))
            {
                //死ぬ
                KillMe();
            }


            //一定時間、安置への移動指示出す
            StartCoroutine(GoToEnshrine());

            //以降の処理を行わない
            return;
        }

        //まだアイテムを取得していないなら
        if(!gotItem)
        {
            //生成したアイテムのリストの要素が0なら
            if (GameData.instance.generatedItemTranList.Count <= 0)//nullエラー回避
            {
                //問題を報告
                Debug.Log("アイテムが見当たりません");

                //以降の処理を行わない
                return;
            }

            //最も近くにある使用可能アイテムの番号を設定
            SetNearItemNo();

            //目標地点を設定
            SetTargetPosition(GameData.instance.generatedItemTranList[nearItemNo].position);

            //アイテムを取得できる距離まで近づいたら
            if (GetLengthToNearItem(nearItemNo) <= getItemLength)
            {
                //アイテムを取得し、アイテムを取得済みの状態に切り替える
                gotItem = GetItem(nearItemNo);

                //停止距離を設定
                agent.stoppingDistance = stoppingDistance;
            }

            //以降の処理を行わない
            return;
        }

        //使用できないアイテムを拾ってしまったら
        if(!usedItemData.enemyCanUse)
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

        //EnemyかPlayerが存在していなかったら
        if (enemyGenerator.generatedEnemyList.Count <= 0 || playerTran.gameObject == null)//nullエラー回避
        {
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
    /// 一定時間、安置へ向かう
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator GoToEnshrine()
    {
        //安置に向かうよう指示を出す
        goToEnshrine = true;

        //一定時間、移動指示を出し続ける
        yield return new WaitForSeconds(instructionTime);

        //移動指示を解除
        goToEnshrine = false;
    }

    /// <summary>
    /// 最も近くにいる敵の位置を取得する
    /// </summary>
    /// <returns>最も近くにいる敵の位置</returns>
    private Vector3 GetNearEnemyPos()
    {
        //最も近くにいる敵の位置にPlayerの位置を仮に登録
        Vector3 nearPos = playerTran. position;

        //生成したEnemyの位置情報のリストの要素数だけ繰り返す
        for (int i = 0; i < enemyGenerator.generatedEnemyList.Count; i++)
        {
            //繰り返し処理で取得した敵が自分だったら
            if (i==myNo)
            {
                //次の繰り返し処理に移る
                continue;
            }

            //繰り返し処理で取得した敵が死亡していたら
            if (enemyGenerator.generatedEnemyList[i]==null)//nullエラー回避
            {
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
        //アイテムの番号
        int itemNo = 0;

        //リストの0番の要素の座標をnearPosに仮に登録
        Vector3 nearPos = GameData.instance.generatedItemTranList[0].position;

        //リストの要素数だけ繰り返す
        for (int i = 0; i < GameData.instance.generatedItemTranList.Count; i++)
        {
            //繰り返し処理で見つけたアイテムが使用不可だったら
            if (!GameData.instance.generatedItemDataList[i].enemyCanUse)
            {
                //以降の処理は行わずに、次の繰り返しに移る
                continue;
            }

            //リストのi番の要素の座標をposに登録
            Vector3 pos = GameData.instance.generatedItemTranList[i].position;

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
        nearItemNo= itemNo;
    }

    /// <summary>
    /// 最も近くにある使用可能アイテムとの距離を取得
    /// </summary>
    /// <param name="nearItemNo">最も近くにある使用可能アイテムの番号</param>
    /// <returns>最も近くにある使用可能アイテムとの距離</returns>
    private float GetLengthToNearItem(int nearItemNo)
    {
        //最も近くにある使用可能アイテムとの距離を返す
        return Vector3.Scale((GameData.instance.generatedItemTranList[nearItemNo].position - transform.position), new Vector3(1, 0, 1)).magnitude;
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
	/// <returns>接地していたらtrue</returns>
	private bool CheckGrounded()
    {
        //rayの初期位置と向き（姿勢）を設定
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //rayの探索距離（長さ）を設定
        float tolerance = 0.2f;

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
        usedItemData=GameData.instance.generatedItemDataList[nearItemNo];

        //取得したアイテムを配置
        usedItemObj= Instantiate(GameData.instance.generatedItemDataList[nearItemNo].prefab, enemyWeaponTran);

        //アイテムを拾う
        GameData.instance.GetItem(nearItemNo, false);

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

        //弾を生成
        Rigidbody bulletRb = Instantiate(itemData.bulletPrefab, shotBulletTran.position,Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0));

        //生成した弾の親を自身に設定
        bulletRb.transform.parent = transform;

        //弾を発射
        bulletRb.AddForce(enemyWeaponTran.forward * itemData.shotSpeed);

        //経過時間を初期化
        timer = 0;

        //発射した弾を3.0秒後に消す
        Destroy(bulletRb.gameObject, 3.0f);
    }

    /// <summary>
    /// 他のコライダーに触れた際に呼び出される
    /// </summary>
    /// <param name="collision">触れた相手</param>
    private void OnCollisionEnter(Collision collision)
    {
        //触れたゲームオブジェクトのタグに応じて処理を変更
        switch (collision.gameObject.tag)
        {
            //手榴弾なら
            case "Grenade":
                UpdateEnemyHp(-itemDataSO.itemDataList[1].attackPower, collision,false);
                break;

            //催涙弾なら
            case "TearGasGrenade":
                UpdateEnemyHp(-itemDataSO.itemDataList[2].attackPower, collision,false);
                StartCoroutine(AttackedByTearGasGrenade());
                break;

            //ナイフなら
            case "Knife":
                UpdateEnemyHp(-itemDataSO.itemDataList[3].attackPower,collision,false);
                break;

            //バットなら
            case "Bat":
                UpdateEnemyHp(-itemDataSO.itemDataList[4].attackPower,collision,false);
                break;

            //アサルトなら
            case "Assault":
                UpdateEnemyHp(-itemDataSO.itemDataList[5].attackPower, collision, true);
                break;

            //ショットガンなら
            case "Shotgun":
                UpdateEnemyHp(-itemDataSO.itemDataList[6].attackPower, collision, true);
                break;

            //スナイパーなら
            case ("Sniper"):
                UpdateEnemyHp(-itemDataSO.itemDataList[7].attackPower, collision, true);
                break;
        }
    }

    /// <summary>
    /// EnemyのHpを更新
    /// </summary>
    /// <param name="updateValue">EnemyのHpの更新量</param>
    /// <param name="collision">衝突相手</param>
    /// <param name="destoryFlag">衝突相手を消すかどうか</param>
    private void UpdateEnemyHp(float updateValue, Collision collision,bool destoryFlag)
    {
        //自分が撃った弾を被弾したなら
        if(collision.transform.parent==transform)
        {
            //以降の処理を行わない
            return;
        }

        //攻撃してきた相手がPlayerかどうか
        bool isPlayer = false;

        //Enemyの体力を0以上100以下に制限しながら、更新する
        enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

        //衝突相手の親がPlayerTranなら
        if(collision.transform.parent.gameObject.CompareTag("PlayerTran"))
        {
            //攻撃してきた相手をPlayerに設定
            isPlayer = true;

            //フロート表示を生成
            StartCoroutine(uiManager.GenerateFloatingMessage(Mathf.Abs(updateValue).ToString("F0"), Color.yellow));
        }

        //衝突相手を消すという指示なら
        if (destoryFlag)
        {
            //触れた相手を消す
            Destroy(collision.gameObject);
        }

        //Enemyの体力が0になったら
        if (enemyhp == 0.0f)
        {
            //攻撃してきた相手がPlayerなら
            if(isPlayer)
            {
                //Playerが倒した敵の数に1を加える
                GameData.instance.KillCount++;
            }

            //死ぬ
            KillMe();
        }
    }

    /// <summary>
    /// 死ぬ際の処理
    /// </summary>
    private void KillMe()
    {
        //自身をゲームオブジェクトごと消す
        Destroy(gameObject);
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
            float velocity = (currentPos - firstPos).magnitude /0.1f;

            //走る
            animator.SetBool("MovePrevious", velocity > 0.1f);

            //何もしない
            animator.SetBool("Idle", velocity <= 0.1f);

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }
}
