using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneを使用

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator enemyGenerator;//EnemyGenerator

    [SerializeField]
    private AirplaneController airplaneController;//AirplaneController

    [SerializeField]
    private UIManager uiManager;//UIManager

    private IEnumerator Start()
    {
        //CanvasGroupを非表示にする
        uiManager.SetCanvasGroup(false);

        //ゲームスタート演出を行う
        yield return StartCoroutine(uiManager.PlayGameStart());

        //飛行機に関する設定を行う
        airplaneController.SetUpAirplane();

        //Playerの行動の制御を開始
        StartCoroutine(airplaneController.ControlPlayerMovement());

        //Enemyの生成を開始
        StartCoroutine(enemyGenerator.GenerateEnemy());

        //アイテムスロットの設定等を行う
        uiManager.SetUpItemSlots();
    }
}
