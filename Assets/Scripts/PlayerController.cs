using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用
using System;//enumを使用

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
	private float jumpHeight;//ジャンプの高さ

	[SerializeField]
	private KeyCode jumpKey;//ジャンプキー

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
	private GameObject mainCamera;//メインカメラ

	private Vector3 moveDirection = Vector3.zero;//進行方向ベクトル

	private int useItemNo=1;//使用しているアイテムの番号

	public int UseItemNo//useItemNo変数用のプロパティ
    {
		get { return useItemNo; }//外部からは取得処理のみ可能に
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
		Jumping,//ジャンプしている
		Stooping//かがんでいる
	}

	/// <summary>
	/// 毎フレーム呼び出される
	/// </summary>
    void Update()
	{
		//アニメーションの再生と、Playerの動きの制御を行う
		PlayAnimation(MovePlayer());

		//アイテムを制御
		ControlItem();
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

			case PlayerCondition.Jumping:
				animationName = "Jumping";
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
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, transform.eulerAngles.z);

		//重力を生成
		moveDirection.y -= gravity * Time.deltaTime;

		//ローカル空間からワールド空間へdirectionを変換し、その向きと大きさに移動
		controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);

		//Playerが接地していないなら
		if (!CheckGrounded())
		{
			//Playerの状態を返す
			return PlayerCondition.Jumping;
		}

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

		//ジャンプキーが押されたら
		if (Input.GetKeyDown(jumpKey))
		{
			//jumpHeightの高さへ0.5秒かけて移動して、元の位置に戻る
			transform.DOMove(new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z), 0.5f).SetLoops(2, LoopType.Yoyo);

			//Playerの状態を返す
			return PlayerCondition.Jumping;
		}

		//かがむキーが押されている間
		if (Input.GetKey(stoopKey))
		{
			//TODO:かがむ処理

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
		//何かキーが押されていたら
		if (CheckKey() != KeyCode.None)//無駄な処理を回避
		{
			//アイテムの切り替えを行う
			ChangeItem(CheckKey());

			//アイテム破棄キーが押されたら
			if (Input.GetKeyDown(discardKey))
			{
				//TODO:アイテムを破棄する処理
			}
			//左クリックされている間
			else if (Input.GetKey(KeyCode.Mouse0))
			{
				//TODO:アイテムの使用処理
			}
			//右クリックされている間
			else if (Input.GetKey(KeyCode.Mouse1))
			{
				//アイテムを構える処理
			}
		}

		//TODO:アイテムに近づいたら

		//アイテム取得キーが押されたら
		if (Input.GetKeyDown(getItemKey))
		{
			//TODO:アイテムの取得処理
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
				useItemNo = 1;
				break;

			case KeyCode.Alpha2:
				useItemNo = 2;
				break;

			case KeyCode.Alpha3:
				useItemNo= 3;
				break;

			case KeyCode.Alpha4:
				useItemNo = 4;
				break;

			case KeyCode.Alpha5:
				useItemNo = 5;
				break;
		}
    }
}
