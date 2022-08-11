using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class PlayerController : MonoBehaviour
{
    [SerializeField]
	private float gravity;//�d��

    [SerializeField]
	private float previousSpeed;//�O�i���x

	[SerializeField]
	private float backSpeed;//��i���x

	[SerializeField]
	private float speedX;//���E�ړ����x

    [SerializeField]
	private float jumpHeight;//�W�����v�̍���

	[SerializeField]
	private KeyCode jumpKey;//�W�����v�{�^��

	[SerializeField]
	private KeyCode stoopKey;//�����ރ{�^��

	[SerializeField]
	private CharacterController controller;//CharacterController

    [SerializeField]
	private Animator anim;//Animator

	private Vector3 moveDirection = Vector3.zero;//�i�s�����x�N�g��

	/// <summary>
	/// Player�̏��
	/// </summary>
	public enum PlayerCondition
    {
		Idle,//�������Ă��Ȃ�
		MoveBack,//��i���Ă���
		MovePrevious,//�O�i���Ă���
		MoveX,//���E�ړ����Ă���
		Jumping,//�W�����v���Ă���
		Stooping//������ł���
	}

	void Update()
	{
		//Player�̈ړ��𐧌�
		MovePlayer();
	}

	/// <summary>
	/// Player�̈ړ��𐧌�
	/// </summary>
	private PlayerCondition MovePlayer()
    {
		//�d�͂𔭐�
		moveDirection.y -= gravity * Time.deltaTime;

		//���[�J����Ԃ��烏�[���h��Ԃ�direction��ϊ����A���̌����Ƒ傫���Ɉړ�
		controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);

		//Player���ڒn���Ă���Ȃ�
		if (CheckGrounded())
		{
			//W��������Ă����
			if (Input.GetAxis("Vertical") > 0.0f)
			{
				//�i�s�����x�N�g����ݒ�
				moveDirection.z = Input.GetAxis("Vertical") * previousSpeed;

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.MovePrevious;
			}
			//S��������Ă����
			else if (Input.GetAxis("Vertical") < 0.0f)
			{
				//�i�s�����x�N�g����ݒ�
				moveDirection.z = Input.GetAxis("Vertical") * backSpeed;

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.MoveBack;
			}

			//A��D��������Ă����
			if (Input.GetAxis("Horizontal") != 0.0f)
			{
				//�i�s�����x�N�g����ݒ�
				moveDirection.x = Input.GetAxis("Horizontal") * speedX;

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.MoveX;
			}

			//�W�����v�{�^���������ꂽ��
			if (Input.GetKeyDown(jumpKey))
			{
				//jumpHeight�̍�����0.5�b�����Ĉړ����āA���̈ʒu�ɖ߂�
				transform.DOMove(new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z), 0.5f).SetLoops(2, LoopType.Yoyo);

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.Jumping;
			}

			//�����ރ{�^����������Ă����
			if (Input.GetKey(stoopKey))
			{
				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.Stooping;

				//TODO:�����ޏ���
			}

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.Idle;
		}

		//Player�̏�Ԃ�Ԃ�
		return PlayerCondition.Idle;
	}

	/// <summary>
	/// Player���ڒn���Ă�����true��Ԃ�
	/// </summary>
	/// <returns></returns>
	public bool CheckGrounded()
	{
		//ray�̏����ʒu�ƌ����i�p���j��ݒ�
		var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

		//ray�̒T�������i�����j��ݒ�
		var tolerance = 0.3f;

		//ray�̃q�b�g����ibool�^�j��Ԃ�
		return Physics.Raycast(ray, tolerance);
	}
}
