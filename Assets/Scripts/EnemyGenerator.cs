using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform enemyPrefab;//Enemyのプレファブ

    [SerializeField]
    private int maxGenerateCount;//Enemyの最大生成数

    [SerializeField]
    private float flightTime;//飛行機の飛行時間

    [SerializeField]
    private Transform enemiesTran;//Enemyの親オブジェクト

    [HideInInspector]
    public List<Transform> generatedEnemyTranList=new List<Transform>();//生成したEnemyの位置情報のリスト

    private List<float> generateTimeList=new List<float>();//Enemyを生成する時間のリスト

    private float timer;//経過時間

    private int generateCount;//Enemyの生成数

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //Enemyの最大生成数だけ繰り返す
        for(int i = 0; i < maxGenerateCount; i++)
        {
            //Enemyを生成する時間のリストを完成させる
            generateTimeList.Add(Random.Range(1f,flightTime));
        }
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //TODO:飛行機が動き始めたら以降の処理を行う

        //全てのEnemyを生成し終わったら
        if(generateCount==maxGenerateCount)
        {
            //以降の処理を行わない
            return;
        }

        //経過時間を計測
        timer+=Time.deltaTime;

        //Enemyを生成する時間のリストの要素数だけ繰り返す
        for (int i = 0; i < generateTimeList.Count; i++)
        {
            //経過時間が生成時間以上になったら
            if (timer >= generateTimeList[i])
            {
                //Enemyを生成し、親を設定
                Transform enemyTran = Instantiate(enemyPrefab, enemiesTran);

                //生成したEnemyの場所を調整
                enemyTran.position = transform.position;

                //生成したEnemyの大きさを調整
                enemyTran.localScale = new Vector3(2f, 2f, 2f);

                //生成したEnemyの位置情報をリストに加える
                generatedEnemyTranList.Add(enemyTran);

                //生成数に1を加える
                generateCount++;

                //Enemyを生成する時間のリストからその要素を排除
                generateTimeList.RemoveAt(i);
            }
        }
    }
}
