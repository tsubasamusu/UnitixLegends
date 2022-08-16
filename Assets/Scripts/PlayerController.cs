using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//enumを使用
using Cinemachine;//Cinemachineを使用

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float gravity;//重力

	[SerializeField]
	private float previousSpeed;//前進速度

	[SerializeField]
	private float backSpeed;//後進速度

	[SerializeField]
	private float speedX;//左右移動速度

	[SerializeField]
	private float fallSpeed;//落下速度

	[SerializeField, Range(1.0f, 30.0f)]
	private float zoomFOV;//ズーム時の視野角

    [SerializeField]
	private float getItemLength;//アイテムを取得できる距離

	[SerializeField]
	private KeyCode fallKey;//飛び降りるキー

	[SerializeField]
	private KeyCode stoopKey;//かがむキー

	[SerializeField]
	private KeyCode getItemKey;//アイテム取得キー

	[SerializeField]
	private KeyCode discardKey;//アイテム破棄キー

	[SerializeField]
	private CharacterController controller;//CharacterController

	[SerializeField]
	private Animator anim;//Animator

    [SerializeField]
	private BulletManager bulletManager;//BulletManager

    [SerializeField]
	private UIManager uiManager;//UIManager

	[SerializeField]
	private CinemachineFollowZoom followZoom;//CinemachineFollowZoom

	[SerializeField]
	private Transform mainCamera;//メインカメラ

	private Vector3 moveDirection = Vector3.zero;//進行方向ベクトル

	private int selectedItemNo=1;//使用しているアイテムの番号

	public int SelectedItemNo//useItemNo変数用のプロパティ
    {
		get { return selectedItemNo; }//外部からは取得処理のみ可能に
    }

	/// <summary>
	/// Playerの状態
	/// </summary>
	private enum PlayerCondition
	{
		Idle,//何もしていない
		MoveBack,//後進している
		MovePrevious,//前進している
		MoveRight,//右移動している
		MoveLeft,//左移動している
		Stooping//かがんでいる
	}

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    void Update()
	{
		//飛び降りるキーを押されたら
		if (Input.GetKeyDown(fallKey))
		{
			//飛行機から飛び降りる
			FallFromAirplane();
		}

		//接地していなかったら
		if (!CheckGrounded())
		{
			//以降の処理を行わない
			return;
		}

		//アニメーションの再生と、Playerの動きの制御を行う
		PlayAnimation(MovePlayer());

		//アイテムを制御
		ControlItem();
	}

	/// <summary>
	/// 飛行機から飛び降りる
	/// </summary>
	/// <returns>待ち時間</returns>
	private IEnumerator FallFromAirplane()
    {
		//接地していない間、繰り返される
		while(!CheckGrounded())
        {
			//落下する
			transform.Translate(0,-fallSpeed,0);

			//待ち時間を返す（実質、FixedUpdateと同じ）
			yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

	/// <summary>
	/// 受け取ったPlayerの状態を元に、アニメーションの再生を行う
	/// </summary>
	/// <param name="playerCondition"></param>
	private void PlayAnimation(PlayerCondition playerCondition)
	{
		//アニメーション名
		string animationName = null;

		//Playerの状態に応じてアニメーション名を設定
		switch (playerCondition)
		{
			case PlayerCondition.Idle:
				animationName = "Idle";
				break;

			case PlayerCondition.MoveBack:
				animationName = "MoveBack";
				break;

			case PlayerCondition.MovePrevious:
				animationName = "MovePrevious";
				break;

			case PlayerCondition.MoveRight:
				animationName = "MoveRight";
				break;

			case PlayerCondition.MoveLeft:
				animationName = "MoveLeft";
				break;

			case PlayerCondition.Stooping:
				animationName = "Stooping";
				break;
		}

		//nullエラー回避
		if (animationName != null)
		{
			//指定したアニメーション名のアニメーションを再生
			anim.Play(animationName);
		}
	}

	/// <summary>
	/// Playerの移動を制御
	/// </summary>
	private PlayerCondition MovePlayer()
	{
		//Playerの向きをカメラの向きに合わせる
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);

		//重力を生成
		moveDirection.y -= gravity * Time.deltaTime;

		//ローカル空間からワールド空間へdirectionを変換し、その向きと大きさに移動
		controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);

		//Wを押されている間
		if (Input.GetAxis("Vertical") > 0.0f)
		{
			//進行方向ベクトルを設定
			moveDirection.z = Input.GetAxis("Vertical") * previousSpeed;

			//Playerの状態を返す
			return PlayerCondition.MovePrevious;
		}
		//Sを押されている間
		else if (Input.GetAxis("Vertical") < 0.0f)
		{
			//進行方向ベクトルを設定
			moveDirection.z = Input.GetAxis("Vertical") * backSpeed;

			//Playerの状態を返す
			return PlayerCondition.MoveBack;
		}

		//Dを押されている間
		if (Input.GetAxis("Horizontal") > 0.0f)
		{
			//進行方向ベクトルを設定
			moveDirection.x = Input.GetAxis("Horizontal") * speedX;

			//Playerの状態を返す
			return PlayerCondition.MoveRight;
		}
		//Aを押されている間
		else if (Input.GetAxis("Horizontal") < 0.0f)
		{
			//進行方向ベクトルを設定
			moveDirection.x = Input.GetAxis("Horizontal") * speedX;

			//Playerの状態を返す
			return PlayerCondition.MoveLeft;
		}

		//かがむキーが押されている間
		if (Input.GetKey(stoopKey))
		{
			//TODO:かがむ処理を追加

			//Playerの状態を返す
			return PlayerCondition.Stooping;
		}

		//Playerの状態を返す
		return PlayerCondition.Idle;
	}

	/// <summary>
	/// Playerが接地していたらtrueを返す
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
	/// アイテム関連の処理
	/// </summary>
	private void ControlItem()
	{
		//アイテムの切り替えを行う
		ChangeItem(CheckKey());

		//アイテム破棄キーが押されたら
		if (Input.GetKeyDown(discardKey))
		{
			//アイテムを破棄する
			GameData.instance.DiscardItem(SelectedItemNo-1);
		}
		//左クリックされている間
		else if (Input.GetKey(KeyCode.Mouse0))
		{
			//TODO:選択しているアイテムが銃火器なら

			//弾を発射
			bulletManager.ShotBullet();
		}
		//右クリックされている間
		else if (Input.GetKey(KeyCode.Mouse1))
		{
            //ズームする
            followZoom.m_MaxFOV = zoomFOV;
            followZoom.m_MinFOV = 1.0f;
        }
		//右クリックが終ったら
		else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            //元のカメラの倍率に戻す
            followZoom.m_MaxFOV = 30.0f;
            followZoom.m_MinFOV = 30.0f;
        }

		//Playerの最も近くにあるアイテムとの距離が、アイテムを取得できないほど離れているか、アイテムが存在しなかったら
		if(GameData.instance.LengthToNearItem>getItemLength||GameData.instance.generatedItemDataList.Count==0)
        {
			//メッセージを空にする
			uiManager.SetMessageText("",Color.black);

			//以下の処理を行わない
			return;
        }

		//許容オーバーかどうか調べる
		GameData.instance.CheckIsFull();

		//許容オーバーかつ、取得しようとしているアイテムが弾ではなかったら
		if (GameData.instance.IsFull &&GameData.instance.generatedItemDataList[GameData.instance.NearItemNo].isNotBullet)
		{
			//メッセージを表示
			uiManager.SetMessageText("Tap 'X' To\nDiscard\nThe Item",Color.red);
		}
		//許容オーバーではないなら
		else
		{
			//メッセージを表示
			uiManager.SetMessageText("Tap 'Q' To\nGet The\nItem",Color.green);
		}

		//アイテム取得キーが押されたら
		if (Input.GetKeyDown(getItemKey))
		{
			//アイテムを取得する
			GameData.instance.GetItem(GameData.instance.NearItemNo,true);
		}
	}

	/// <summary>
	/// 押されたキーの情報を返す
	/// </summary>
	private KeyCode CheckKey()
	{
		//何もキーを押されていないなら
		if (!Input.anyKeyDown)//無駄な処理を回避
		{
			//キーの情報を返す
			return KeyCode.None;
		}

		//1つずつキーの情報を取り出す
		foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
		{
			//foreach文で取得したキーと、押されたキーが同じなら
			if (Input.GetKeyDown(code))
			{
				//キーの情報を返す
				return code;
			}
		}

		//キーの情報を返す
		return KeyCode.None;
	}

	/// <summary>
	/// 使用アイテムの切り替えを行う
	/// </summary>
	private void ChangeItem(KeyCode code)
    {
		//押されたキーに応じて使用しているアイテムの番号を設定
        switch (code)
        {
			case KeyCode.Alpha1:
				selectedItemNo = 1;
				uiManager.SetItemSlotBackgroundColor(1, Color.red);
				break;

			case KeyCode.Alpha2:
				selectedItemNo = 2;
				uiManager.SetItemSlotBackgroundColor(2, Color.red);
				break;

			case KeyCode.Alpha3:
				selectedItemNo= 3;
				uiManager.SetItemSlotBackgroundColor(3, Color.red);
				break;

			case KeyCode.Alpha4:
				selectedItemNo = 4;
				uiManager.SetItemSlotBackgroundColor(4, Color.red);
				break;

			case KeyCode.Alpha5:
				selectedItemNo = 5;
				uiManager.SetItemSlotBackgroundColor(5, Color.red);
				break;
		}
    }
}
