using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//enum���g�p
using Cinemachine;//Cinemachine���g�p

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float previousSpeed;//�O�i���x

	[SerializeField]
	private float backSpeed;//��i���x

	[SerializeField]
	private float speedX;//���E�ړ����x

	[SerializeField, Range(1.0f, 30.0f)]
	private float normalZoomFOV;//�ʏ�̃Y�[�����̎���p

	[SerializeField, Range(1.0f, 30.0f)]
	private float ScopeZoomFOV;//�X�R�[�v�ɂ��Y�[���̎���p

	[SerializeField]
	private float getItemLength;//�A�C�e�����擾�ł��鋗��

	[SerializeField]
	private KeyCode stoopKey;//�����ރL�[

	[SerializeField]
	private KeyCode getItemKey;//�A�C�e���擾�L�[

	[SerializeField]
	private KeyCode discardKey;//�A�C�e���j���L�[

	[SerializeField]
	private Rigidbody playerRb;//Rigidbody

	[SerializeField]
	private BoxCollider boxCollider;//BoxCollider

	[SerializeField]
	private Animator anim;//Animator

	[SerializeField]
	private BulletManager bulletManager;//BulletManager

	[SerializeField]
	private UIManager uiManager;//UIManager

	[SerializeField]
	private ItemManager itemManager;//ItemManager

	[SerializeField]
	private SoundDataSO soundDataSO;//SoundDataSO

	[SerializeField]
	private AudioSource audioSource;//AudioSource

	[SerializeField]
	private CinemachineFollowZoom followZoom;//CinemachineFollowZoom

	[SerializeField]
	private Transform mainCameraTran;//���C���J�����̈ʒu

	[SerializeField]
	private Transform playerCharacterTran;//Player�̃L�����N�^�[�̈ʒu

	private Vector3 moveDirection = Vector3.zero;//�i�s�����x�N�g��

	private Vector3 desiredMove = Vector3.zero;//�ړ��x�N�g��

	private Vector3 firstColliderCenter;//�R���C�_�[�̃Z���^�[�̏����l

	private Vector3 firstColliderSize;//�R���C�_�[�̑傫���̏����l

	private bool landed;//��s�@�����э~��Ē��n�������ǂ���

	private int selectedItemNo = 1;//�g�p���Ă���A�C�e���̔ԍ�

	public int SelectedItemNo//useItemNo�ϐ��p�̃v���p�e�B
	{
		get { return selectedItemNo; }//�O������͎擾�����̂݉\��
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
		Stooping//������ł���
	}

	/// <summary>
	/// �Q�[���J�n����ɌĂяo�����
	/// </summary>
	private void Start()
	{
		//Rigidbody�ɂ��d�͂𖳌���
		playerRb.useGravity = false;

		//�R���C�_�[�̃Z���^�[�̏����l���擾
		firstColliderCenter = boxCollider.center;

		//�R���C�_�[�̑傫���̏����l���擾
		firstColliderSize = boxCollider.size;

		//�������Z�𖳌���
		playerRb.isKinematic = true;

		//AudioSouce�𖳌���
		audioSource.enabled = false;
	}

	/// <summary>
	/// ���t���[���Ăяo�����
	/// </summary>
	void Update()
	{
		//Player�������E�ɍs���Ă��܂�����
		if (transform.position.y <= -1f)
		{
			//���g�̍��W��(0,0,0)�ɐݒ�
			transform.position = Vector3.zero;

			//�ȍ~�̏������s��Ȃ�
			return;
		}

		//Player���]�|���Ă�����
		if (CheckToppled())
		{
			//���b�Z�[�W��\��
			uiManager.SetMessageText("I'm\nTrying To\nRecover", Color.red);

			//�Ԑ��𗧂Ē���
			transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
		}

		//�ڒn���Ă��Ȃ�������
		if (!CheckGrounded())
		{
			//�ȍ~�̏������s��Ȃ�
			return;
		}

		//�A�j���[�V�����̍Đ��ƁAPlayer�̓����̐�����s��
		PlayAnimation(MovePlayer());

		//�A�C�e���𐧌�
		ControlItem();
	}

	/// <summary>
	/// ��莞�Ԃ��ƂɌĂяo�����
	/// </summary>
	private void FixedUpdate()
	{
		//�ړ�����
		playerRb.MovePosition(transform.position + (desiredMove * Time.fixedDeltaTime));

		//��s�@�����э~��āA���ɒ��n�����̂Ȃ�
		if (landed)
		{
			//�ȍ~�̏������s��Ȃ�
			return;
		}

		//Player���ڒn���Ă��Ȃ�������
		if (!CheckGrounded())
		{
			//��������
			transform.Translate(0, -GameData.instance.FallSpeed, 0);
		}
		//Player���ڒn������
		else
		{
			//���ʉ����Đ�
			AudioSource.PlayClipAtPoint(soundDataSO.soundDataList[11].audioClip, Camera.main.transform.position);

			//���n������������Ԃɐ؂�ւ���
			landed = true;

			//�������Z��L����
			playerRb.isKinematic = false;

			//Rigidbody�ɂ��d�͂�L����
			playerRb.useGravity = true;
		}
	}

	/// <summary>
	/// Player���]�|���Ă��邩�ǂ������ׂ�
	/// </summary>
	/// <returns>Player���]�|���Ă�����true</returns>
	private bool CheckToppled()
	{
		//�p�x������Ȃ�false��Ԃ�
		if (transform.eulerAngles.x < 40f && transform.eulerAngles.x >= 0f)
		{
			return false;
		}
		else if (transform.eulerAngles.x <= 360 && transform.eulerAngles.x > 320f)
		{
			return false;
		}

		if (transform.eulerAngles.z < 40f && transform.eulerAngles.z >= 0f)
		{
			return false;
		}
		else if (transform.eulerAngles.z <= 360 && transform.eulerAngles.z > 320f)
		{
			return false;
		}

		//true��Ԃ�
		return true;
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
		//Player�̃L�����N�^�[�̌������J�����̌����ɍ��킹��
		playerCharacterTran.eulerAngles = new Vector3(0f, mainCameraTran.eulerAngles.y, 0f);

		//�ړ�������Player�̌����ɍ��킹��
		desiredMove = (mainCameraTran.forward * moveDirection.z) + (mainCameraTran.right * moveDirection.x);

		//�ړ��x�N�g���̑傫����1.0��菬�����Ȃ�
		if (desiredMove.magnitude < 1f)
		{
			//�ړ��x�N�g����0����
			desiredMove = Vector3.zero;//�o�O�h�~
		}

		//W��������Ă����
		if (Input.GetAxis("Vertical") > 0.0f)
		{
			//AudioSource��L����
			audioSource.enabled = true;

			//�i�s�����x�N�g����ݒ�
			moveDirection.z = Input.GetAxis("Vertical") * previousSpeed;

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.MovePrevious;
		}
		//S��������Ă����
		else if (Input.GetAxis("Vertical") < 0.0f)
		{
			//AudioSouce�𖳌���
			audioSource.enabled = false;

			//�i�s�����x�N�g����ݒ�
			moveDirection.z = Input.GetAxis("Vertical") * backSpeed;

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.MoveBack;
		}
		//W��S��������Ă��Ȃ��Ȃ�
		else
		{
			//AudioSouce�𖳌���
			audioSource.enabled = false;
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

		//�����ރL�[��������Ă����
		if (Input.GetKey(stoopKey))
		{
			//�R���C�_�[�̃Z���^�[��ݒ�
			boxCollider.center = new Vector3(0f, 0.25f, 0f);

			//�R���C�_�[�̑傫����ݒ�
			boxCollider.size = new Vector3(0.5f, 0.5f, 0.5f);

			//Player�̏�Ԃ�Ԃ�
			return PlayerCondition.Stooping;
		}
		//�����ރL�[��������Ă��Ȃ��Ȃ�
		else
		{
			//�R���C�_�[�̃Z���^�[�������l�ɐݒ�
			boxCollider.center = firstColliderCenter;

			//�R���C�_�[�̑傫���������l�ɐݒ�
			boxCollider.size = firstColliderSize;
		}

		//Player�̏�Ԃ�Ԃ�
		return PlayerCondition.Idle;
	}

	/// <summary>
	/// Player���ڒn���Ă��邩���ׂ�
	/// </summary>
	/// <returns>�ڒn���Ă�����true</returns>
	private bool CheckGrounded()
	{
		//ray�̏����ʒu�ƌ����i�p���j��ݒ�
		var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

		//ray�̒T�������i�����j��ݒ�
		var tolerance = 0.2f;

		//ray�̃q�b�g����ibool�^�j��Ԃ�
		return Physics.Raycast(ray, tolerance);
	}

	/// <summary>
	/// �A�C�e���֘A�̏���
	/// </summary>
	private void ControlItem()
	{
		//�A�C�e���̐؂�ւ����s��
		ChangeItem(CheckKey());

		//�A�C�e���j���L�[�������ꂽ��
		if (Input.GetKeyDown(discardKey))
		{
			//���ʉ����Đ�
			AudioSource.PlayClipAtPoint(soundDataSO.soundDataList[15].audioClip, Camera.main.transform.position);

			//�A�C�e����j������
			itemManager.DiscardItem(SelectedItemNo - 1);
		}
		//���N���b�N����Ă����
		else if (Input.GetKey(KeyCode.Mouse0))
		{
			//�A�C�e�����g�p����
			itemManager.UseItem(itemManager.GetSelectedItemData());
		}
		//�E�N���b�N����Ă����
		else if (Input.GetKey(KeyCode.Mouse1))
		{
			//�I�����Ă���A�C�e�����X�i�C�p�[�ł͂Ȃ��Ȃ�
			if (itemManager.GetSelectedItemData().itemName != ItemDataSO.ItemName.Sniper)
			{
				//�Y�[������
				followZoom.m_MaxFOV = normalZoomFOV;
				followZoom.m_MinFOV = 1.0f;
			}
			//�I�����Ă���A�C�e�����X�i�C�p�[�Ȃ�
			else
			{
				//�Y�[������
				followZoom.m_MaxFOV = ScopeZoomFOV;
				followZoom.m_MinFOV = 1.0f;

				//�X�R�[�v��`��
				uiManager.PeekIntoTheScope();
			}
		}
		//�E�N���b�N���I������
		else if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			//���̃J�����̔{���ɖ߂�
			followZoom.m_MaxFOV = 30.0f;
			followZoom.m_MinFOV = 30.0f;

			//�X�R�[�v��`���̂���߂�
			uiManager.NotPeekIntoTheScope();
		}

		//�E�N���b�N���ꂽ��
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			//Enemy���g���Ȃ�����Ȃ�
			if (!itemManager.GetSelectedItemData().enemyCanUse)
			{
				//�ȍ~�̏������s��Ȃ�
				return;
			}

			//���ʉ����Đ�
			AudioSource.PlayClipAtPoint(soundDataSO.soundDataList[13].audioClip, Camera.main.transform.position);
		}

		//Player�̍ł��߂��ɂ���A�C�e���Ƃ̋������A�A�C�e�����擾�ł��Ȃ��قǗ���Ă��邩�A�A�C�e�������݂��Ȃ�������
		if (itemManager.LengthToNearItem > getItemLength || itemManager.generatedItemDataList.Count == 0)
		{
			//���b�Z�[�W����ɂ���
			uiManager.SetMessageText("", Color.black);

			//�ȉ��̏������s��Ȃ�
			return;
		}

		//���e�I�[�o�[���ǂ������ׂ�
		itemManager.CheckIsFull();

		//���e�I�[�o�[���A�擾���悤�Ƃ��Ă���A�C�e�����e�ł͂Ȃ�������
		if (itemManager.IsFull && itemManager.generatedItemDataList[itemManager.NearItemNo].isNotBullet)
		{
			//���b�Z�[�W��\��
			uiManager.SetMessageText("Tap 'X' To\nDiscard\nThe Item", Color.red);
		}
		//���e�I�[�o�[�ł͂Ȃ��Ȃ�
		else
		{
			//���b�Z�[�W��\��
			uiManager.SetMessageText("Tap 'Q' To\nGet The\nItem", Color.green);
		}

		//�A�C�e���擾�L�[�������ꂽ��
		if (Input.GetKeyDown(getItemKey))
		{
			//�A�C�e�����擾����
			itemManager.GetItem(itemManager.NearItemNo, true);
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
				selectedItemNo = 1;
				uiManager.SetItemSlotBackgroundColor(1, Color.red);
				break;

			case KeyCode.Alpha2:
				selectedItemNo = 2;
				uiManager.SetItemSlotBackgroundColor(2, Color.red);
				break;

			case KeyCode.Alpha3:
				selectedItemNo = 3;
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

			//��L�ȊO�Ȃ�
			default:
				//�ȍ~�̏������s��Ȃ�
				return;
		}

		//���ʉ����Đ�
		AudioSource.PlayClipAtPoint(soundDataSO.soundDataList[16].audioClip, Camera.main.transform.position);
	}
}
