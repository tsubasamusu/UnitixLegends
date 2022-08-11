using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

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
	private KeyCode jumpKey;//ジャンプボタン

	[SerializeField]
	private KeyCode stoopKey;//かがむボタン

	[SerializeField]
	private CharacterController controller;//CharacterController

    [SerializeField]
	private Animator anim;//Animator

	private Vector3 moveDirection = Vector3.zero;//進行方向ベクトル

	/// <summary>
	/// Playerの状態
	/// </summary>
	public enum PlayerCondition
    {
		Idle,//何もしていない
		MoveBack,//後進している
		MovePrevious,//前進している
		MoveX,//左右移動している
		Jumping,//ジャンプしている
		Stooping//かがんでいる
	}

	void Update()
	{
		//Playerの移動を制御
		MovePlayer();
	}

	/// <summary>
	/// Playerの移動を制御
	/// </summary>
	private PlayerCondition MovePlayer()
    {
		//重力を発生
		moveDirection.y -= gravity * Time.deltaTime;

		//ローカル空間からワールド空間へdirectionを変換し、その向きと大きさに移動
		controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);

		//Playerが接地しているなら
		if (CheckGrounded())
		{
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

			//AかDを押されている間
			if (Input.GetAxis("Horizontal") != 0.0f)
			{
				//進行方向ベクトルを設定
				moveDirection.x = Input.GetAxis("Horizontal") * speedX;

				//Playerの状態を返す
				return PlayerCondition.MoveX;
			}

			//ジャンプボタンが押されたら
			if (Input.GetKeyDown(jumpKey))
			{
				//jumpHeightの高さへ0.5秒かけて移動して、元の位置に戻る
				transform.DOMove(new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z), 0.5f).SetLoops(2, LoopType.Yoyo);

				//Playerの状態を返す
				return PlayerCondition.Jumping;
			}

			//かがむボタンが押されている間
			if (Input.GetKey(stoopKey))
			{
				//Playerの状態を返す
				return PlayerCondition.Stooping;

				//TODO:かがむ処理
			}

			//Playerの状態を返す
			return PlayerCondition.Idle;
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
}
