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
    private float fallSpeed;//落下速度

    private bool componentFlag;//コンポーネント関連の処理を行ったかどうかの判断

    private float enemyhp = 100.0f;//Enemyの体力

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
        if(!CheckGrounded())
        {
            //落下する
            transform.Translate(0,-fallSpeed, 0);
        }
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //接地していないなら
        if(!CheckGrounded())
        {
            //以下の処理を行わない
            return;
        }

        //まだコンポーネントの処理を行っていないなら
        if(!componentFlag)
        {
            //NavMeshAgentを有効化
            agent.enabled=true;

            //コンポーネントの処理が完了した状態に切り替える
            componentFlag=true;
        }
    }

    /// <summary>
    /// 最も近くにある使用可能アイテムの位置情報を返す
    /// </summary>
    /// <returns></returns>
    public Transform FindNearItem()
    {
        //TODO:GameDataのアイテムのリストを元に、最も近くにある攻撃可能アイテムを見つける処理

        return null;//（仮）
    }

    /// <summary>
    /// 最も近くにいる敵の位置情報を返す
    /// </summary>
    /// <returns></returns>
    public Transform FindNearEnemy()
    {
        //TODO:EnemyGeneratorの敵のリストを元に、最も近くにいる敵を見つける処理

        return null ;//（仮）
    }

    /// <summary>
    /// 受け取った位置情報の目標値に設定する
    /// </summary>
    /// <param name="targetTran"></param>
    public void SetTargetPosition(Transform targetTran)
    {
        //引数を元に、AIの目標地点を設定
        agent.destination = targetTran.position;
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
    /// アイテムを拾い終えたらtrueを返す
    /// </summary>
    /// <returns></returns>
    public bool GetItem()
    {
        //TODO:アイテムを拾う処理

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
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //触れたゲームオブジェクトのタグに応じて処理を変更
        switch (collision.gameObject.tag)
        {
            case ("Grenade"):
                UpdateEnemyHp(-30.0f, collision);
                break;
            case ("TearGasGrenade"):
                UpdateEnemyHp(0, collision);
                StartCoroutine( AttackedByTearGasGrenade());
                break;
            case ("Knife"):
                UpdateEnemyHp(-100.0f, collision);
                break;
            case ("Bat"):
                UpdateEnemyHp(-50.0f, collision);
                break;
            case ("Assault"):
                UpdateEnemyHp(-1.0f, collision);
                break;
            case ("Sniper"):
                UpdateEnemyHp(-80.0f, collision);
                break;
            case ("Shotgun"):
                UpdateEnemyHp(-30.0f, collision);
                break;
        }
    }

    /// <summary>
    /// Enemyの体力を更新する
    /// </summary>
    private void UpdateEnemyHp(float updateValue, Collision collision)
    {
        //Enemyの体力を0以上100以下に制限しながら、更新する
        enemyhp = Mathf.Clamp(enemyhp + updateValue, 0.0f, 100.0f);

        //nullエラー回避
        if(collision != null)
        {
            //触れた相手を消す
            Destroy(collision.gameObject);
        }

        //Enemyの体力が0になったら
        if(enemyhp==0.0f)
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
}
