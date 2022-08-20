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

    [SerializeField]
    private float fallSpeed;//落下速度

    [SerializeField]
    private float getItemLength;//アイテムを取得できる距離

    private bool didPostLandingProcessing;//着地直後の処理を行ったかどうか

    private bool gotItem;//アイテムを取得したかどうか

    private float enemyhp = 100.0f;//Enemyの体力

    private float lengthToNearItem;//近くの使用可能アイテムまでの距離

    private Vector3 firstPos;//初期位置

    private int nearItemNo;//最も近くにある使用可能アイテムの番号

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //NavMeshAgentを無効化
        agent.enabled = false;
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
            transform.Translate(0, -fallSpeed, 0);
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

        //まだアイテムを取得していなかったら
        if(!gotItem)
        {
            //最も近くにある使用可能アイテムの番号を設定
            SetNearItemNo();

            //目標地点を設定
            SetTargetPosition(GameData.instance.generatedItemTranList[nearItemNo].position);

            //アイテムを取得できる距離まで近づいたら
            if (GetLengthToNearItem(nearItemNo) <= getItemLength)
            {
                //アイテムを取得し、アイテムを取得済みの状態に切り替える
                gotItem = GetItem(nearItemNo);
            }
        }
    }

    /// <summary>
    /// 最も近くにある使用可能アイテムの番号を設定する
    /// </summary>
    private void SetNearItemNo()
    {
        //nullエラー回避
        if (GameData.instance.generatedItemTranList.Count <= 0)
        {
            //以降の処理を行わない
            return;
        }

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
    /// 最も近くにいる敵の位置情報を返す
    /// </summary>
    /// <returns></returns>
    private Transform FindNearEnemy()
    {
        //TODO:EnemyGeneratorの敵のリストを元に、最も近くにいる敵を見つける処理

        return null;//（仮）
    }

    /// <summary>
    /// 受け取った位置情報の目標値に設定する
    /// </summary>
    /// <param name="targetTran"></param>
    private void SetTargetPosition(Vector3 targetPos)
    {
        //引数を元に、AIの目標地点を設定
        agent.destination = targetPos;
    }

    /// <summary>
	/// 自身が接地していたらtrueを返す
	/// </summary>
	/// <returns></returns>
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
    /// アイテムを拾う
    /// </summary>
    /// <param name="nearItemNo">近くのアイテムの番号</param>
    /// <returns>アイテムを拾い終えたらtrueを返す</returns>
    public bool GetItem(int nearItemNo)
    {
        Debug.Log("Get");

        //アイテムを拾う
        GameData.instance.GetItem(nearItemNo, false);

        //trueを返す
        return true;
    }

    /// <summary>
    /// 弾を撃つ
    /// </summary>
    public void ShotBullet()
    {
        //TODO:弾を撃つ処理
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
            case ("Grenade"):
                UpdateEnemyHp(-itemDataSO.itemDataList[1].attackPower, collision);
                break;

            //催涙弾なら
            case ("TearGasGrenade"):
                UpdateEnemyHp(-itemDataSO.itemDataList[2].attackPower, collision);
                StartCoroutine(AttackedByTearGasGrenade());
                break;

            //ナイフなら
            case ("Knife"):
                UpdateEnemyHp(-itemDataSO.itemDataList[3].attackPower);
                break;

            //バットなら
            case ("Bat"):
                UpdateEnemyHp(-itemDataSO.itemDataList[4].attackPower);
                break;

            //アサルトなら
            case ("Assault"):
                UpdateEnemyHp(-itemDataSO.itemDataList[5].attackPower, collision);
                break;

            //ショットガンなら
            case ("Shotgun"):
                UpdateEnemyHp(-itemDataSO.itemDataList[6].attackPower, collision);
                break;

            //スナイパーなら
            case ("Sniper"):
                UpdateEnemyHp(-itemDataSO.itemDataList[7].attackPower, collision);
                break;
        }
    }

    /// <summary>
    /// Enemyの体力を更新する
    /// </summary>
    private void UpdateEnemyHp(float updateValue, Collision collision = null)
    {
        //Enemyの体力を0以上100以下に制限しながら、更新する
        enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

        //nullエラー回避
        if (collision != null)
        {
            //触れた相手を消す
            Destroy(collision.gameObject);
        }

        //Enemyの体力が0になったら
        if (enemyhp == 0.0f)
        {
            //死亡処理を行う
            WasKilled();
        }
    }

    /// <summary>
    /// 催涙弾を受けた際の処理
    /// </summary>
    private IEnumerator AttackedByTearGasGrenade()
    {
        //Enemyの動きを止める
        agent.enabled = false;

        //5.0秒間、動きを止め続ける
        yield return new WaitForSeconds(5.0f);

        //Enemyの活動を再開する
        agent.enabled = true;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void WasKilled()
    {
        //TODO:死亡処理
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
