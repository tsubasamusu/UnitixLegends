using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshAgentを使用

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;//NavMeshAgent

    [SerializeField]
    private float gravity;//重力

    private bool componentFlag;//コンポーネント関連の処理を行ったかどうかの判断

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
    private void FixedUpdate()
    {
        //接地していなかったら
        if(!CheckGrounded())
        {
            //重力を生成
            transform.Translate(0,-gravity, 0);
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
    /// 最も近くにある攻撃可能アイテムの位置情報を返す
    /// </summary>
    /// <returns></returns>
    public Transform FindNearAggressiveItem()
    {
        return null;//（仮）
    }

    /// <summary>
    /// 最も近くにいる敵の位置情報を返す
    /// </summary>
    /// <returns></returns>
    public Transform FindNearEnemy()
    {
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
}
