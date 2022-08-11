using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
	private float gravity;//重力

    [SerializeField]
	private float speedZ;//前進速度

    [SerializeField]
	private float jumpSpeed;//ジャンプ速度

	[SerializeField]
	private KeyCode jumpKey;//ジャンプボタン

    [SerializeField]
	private CharacterController controller;//CharacterController

    [SerializeField]
	private Animator anim;//Animator

	private Vector3 moveDirection = Vector3.zero;//進行方向

	void Start()
	{

	}

	void Update()
	{
		//Playerが接地しているなら
		if (CheckGrounded())
		{
			//「W,S」で前後移動
			if (Input.GetAxis("Vertical") != 0.0f)//WかSを押されている間
			{
				moveDirection.z = Input.GetAxis("Vertical") * speedZ;
			}
			

			// 方向転換
			transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

			//ジャンプ
			if (Input.GetKeyDown(jumpKey))//ジャンプボタンが押されたら
			{
				moveDirection.y = jumpSpeed;
			}
		}

		//重力を発生させる
		moveDirection.y -= gravity * Time.deltaTime;

		// 移動の実行
		Vector3 globalDirection = transform.TransformDirection(moveDirection);
		controller.Move(globalDirection * Time.deltaTime);

		// ★ポイント（アニメーションの実行）
		// 速度が０以上の場合、Runフラグをtrueにする。
		anim.SetBool("Run", moveDirection.z > 0.0f);

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
