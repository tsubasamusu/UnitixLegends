using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p
using System;//enum���g�p

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
	private KeyCode jumpKey;//�W�����v�L�[

	[SerializeField]
	private KeyCode stoopKey;//�����ރL�[

	[SerializeField]
	private KeyCode getItemKey;//�A�C�e���擾�L�[

	[SerializeField]
	private KeyCode discardKey;//�A�C�e���j���L�[

	[SerializeField]
	private CharacterController controller;//CharacterController

	[SerializeField]
	private Animator anim;//Animator

    [SerializeField]
	private GameObject mainCamera;//���C���J����

	private Vector3 moveDirection = Vector3.zero;//�i�s�����x�N�g��

	private int useItemNo=1;//�g�p���Ă���A�C�e���̔ԍ�

	public int UseItemNo//useItemNo�ϐ��p�̃v���p�e�B
    {
		get { return useItemNo; }//�O������͎擾�����̂݉\��
    }

	/// <summary>
	/// Player�̏��
	/// </summary>
	private enum PlayerCondition
	{
		Idle,//�������Ă��Ȃ�
		MoveBack,//��i���Ă���
		MovePrevious,//�O�i���Ă���
		MoveRight,//�E�ړ����Ă���
		MoveLeft,//���ړ����Ă���
		Jumping,//�W�����v���Ă���
		Stooping//������ł���
	}

	/// <summary>
	/// ���t���[���Ăяo�����
	/// </summary>
    void Update()
	{
		//�A�j���[�V�����̍Đ��ƁAPlayer�̓����̐�����s��
		PlayAnimation(MovePlayer());

		//�A�C�e���𐧌�
		ControlItem();
	}

	/// <summary>
	/// �󂯎����Player�̏�Ԃ����ɁA�A�j���[�V�����̍Đ����s��
	/// </summary>
	/// <param name="playerCondition"></param>
	private void PlayAnimation(PlayerCondition playerCondition)
	{
		//�A�j���[�V������
		string animationName = null;

		//Player�̏�Ԃɉ����ăA�j���[�V��������ݒ�
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

		//null�G���[���
		if (animationName != null)
		{
			//�w�肵���A�j���[�V�������̃A�j���[�V�������Đ�
			anim.Play(animationName);
		}
	}

	/// <summary>
	/// Player�̈ړ��𐧌�
	/// </summary>
	private PlayerCondition MovePlayer()
	{
		//Player�̌������J�����̌����ɍ��킹��
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, transform.eulerAngles.z);

		//�d�͂𐶐�
		moveDirection.y -= gravity * Time.deltaTime;

		//���[�J����Ԃ��烏�[���h��Ԃ�direction��ϊ����A���̌����Ƒ傫���Ɉړ�
		controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);

		//Player���ڒn���Ă��Ȃ��Ȃ�
		if (!CheckGrounded())
		{
			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.Jumping;
		}

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

		//D��������Ă����
		if (Input.GetAxis("Horizontal") > 0.0f)
		{
			//�i�s�����x�N�g����ݒ�
			moveDirection.x = Input.GetAxis("Horizontal") * speedX;

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.MoveRight;
		}
		//A��������Ă����
		else if (Input.GetAxis("Horizontal") < 0.0f)
		{
			//�i�s�����x�N�g����ݒ�
			moveDirection.x = Input.GetAxis("Horizontal") * speedX;

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.MoveLeft;
		}

		//�W�����v�L�[�������ꂽ��
		if (Input.GetKeyDown(jumpKey))
		{
			//jumpHeight�̍�����0.5�b�����Ĉړ����āA���̈ʒu�ɖ߂�
			transform.DOMove(new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z), 0.5f).SetLoops(2, LoopType.Yoyo);

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.Jumping;
		}

		//�����ރL�[��������Ă����
		if (Input.GetKey(stoopKey))
		{
			//TODO:�����ޏ���

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.Stooping;
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

	/// <summary>
	/// �A�C�e���֘A�̏���
	/// </summary>
	private void ControlItem()
	{
		//�����L�[��������Ă�����
		if (CheckKey() != KeyCode.None)//���ʂȏ��������
		{
			//�A�C�e���̐؂�ւ����s��
			ChangeItem(CheckKey());

			//�A�C�e���j���L�[�������ꂽ��
			if (Input.GetKeyDown(discardKey))
			{
				//TODO:�A�C�e����j�����鏈��
			}
			//���N���b�N����Ă����
			else if (Input.GetKey(KeyCode.Mouse0))
			{
				//TODO:�A�C�e���̎g�p����
			}
			//�E�N���b�N����Ă����
			else if (Input.GetKey(KeyCode.Mouse1))
			{
				//�A�C�e�����\���鏈��
			}
		}

		//TODO:�A�C�e���ɋ߂Â�����

		//�A�C�e���擾�L�[�������ꂽ��
		if (Input.GetKeyDown(getItemKey))
		{
			//TODO:�A�C�e���̎擾����
		}
	}

	/// <summary>
	/// �����ꂽ�L�[�̏���Ԃ�
	/// </summary>
	private KeyCode CheckKey()
	{
		//�����L�[��������Ă��Ȃ��Ȃ�
		if (!Input.anyKeyDown)//���ʂȏ��������
		{
			//�L�[�̏���Ԃ�
			return KeyCode.None;
		}

		//1���L�[�̏������o��
		foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
		{
			//foreach���Ŏ擾�����L�[�ƁA�����ꂽ�L�[�������Ȃ�
			if (Input.GetKeyDown(code))
			{
				//�L�[�̏���Ԃ�
				return code;
			}
		}

		//�L�[�̏���Ԃ�
		return KeyCode.None;
	}

	/// <summary>
	/// �g�p�A�C�e���̐؂�ւ����s��
	/// </summary>
	private void ChangeItem(KeyCode code)
    {
		//�����ꂽ�L�[�ɉ����Ďg�p���Ă���A�C�e���̔ԍ���ݒ�
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
