using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//enum���g�p
using Cinemachine;//Cinemachine���g�p

namespace yamap {

	public class PlayerController : MonoBehaviour {

		[SerializeField]
		private float gravity;//�d��

		[SerializeField]
		private float previousSpeed;//�O�i���x

		[SerializeField]
		private float backSpeed;//��i���x

		[SerializeField]
		private float speedX;//���E�ړ����x

		[SerializeField]
		private float fallSpeed;//�������x

		[SerializeField, Range(1.0f, 30.0f)]
		private float normalZoomFOV;//�ʏ�̃Y�[�����̎���p

		[SerializeField, Range(1.0f, 30.0f)]
		private float ScopeZoomFOV;//�X�R�[�v�ɂ��Y�[���̎���p

		[SerializeField]
		private float getItemLength;//�A�C�e�����擾�ł��鋗��

		[SerializeField]
		private KeyCode fallKey;//��э~���L�[

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
		private BulletManager bulletManager;//BulletManager

		[SerializeField]
		private UIManager uiManager;//UIManager

		[SerializeField]
		private CinemachineFollowZoom followZoom;//CinemachineFollowZoom

		[SerializeField]
		private Transform mainCamera;//���C���J����

		private Vector3 moveDirection = Vector3.zero;//�i�s�����x�N�g��


		/// <summary>
		/// Player�̏��
		/// </summary>
		private enum PlayerCondition {
			Idle,//�������Ă��Ȃ�
			MoveBack,//��i���Ă���
			MovePrevious,//�O�i���Ă���
			MoveRight,//�E�ړ����Ă���
			MoveLeft,//���ړ����Ă���
			Stooping//������ł���
		}

		/// <summary>
		/// ���t���[���Ăяo�����
		/// </summary>
		void Update() {
			//��э~���L�[�������ꂽ��
			if (Input.GetKeyDown(fallKey)) {
				//��s�@�����э~���
				FallFromAirplane();
			}

			//�ڒn���Ă��Ȃ�������
			if (!CheckGrounded()) {
				//�ȍ~�̏������s��Ȃ�
				return;
			}

			//�A�j���[�V�����̍Đ��ƁAPlayer�̓����̐�����s��
			PlayAnimation(MovePlayer());

			//�A�C�e���𐧌�
			ControlItem();
		}

		/// <summary>
		/// ��s�@�����э~���
		/// </summary>
		/// <returns>�҂�����</returns>
		private IEnumerator FallFromAirplane() {
			//�ڒn���Ă��Ȃ��ԁA�J��Ԃ����
			while (!CheckGrounded()) {
				//��������
				transform.Translate(0, -fallSpeed, 0);

				//�҂����Ԃ�Ԃ��i�����AFixedUpdate�Ɠ����j
				yield return new WaitForSeconds(Time.fixedDeltaTime);
			}
		}

		/// <summary>
		/// �󂯎����Player�̏�Ԃ����ɁA�A�j���[�V�����̍Đ����s��
		/// </summary>
		/// <param name="playerCondition"></param>
		private void PlayAnimation(PlayerCondition playerCondition) {
			//�A�j���[�V������
			//string animationName = null;

			string animationName = playerCondition.ToString();

   //         //Player�̏�Ԃɉ����ăA�j���[�V��������ݒ�
   //         switch (playerCondition) {
			//	case PlayerCondition.Idle:
			//		animationName =   //"Idle";
			//		break;

			//	case PlayerCondition.MoveBack:
			//		animationName = "MoveBack";
			//		break;

			//	case PlayerCondition.MovePrevious:
			//		animationName = "MovePrevious";
			//		break;

			//	case PlayerCondition.MoveRight:
			//		animationName = "MoveRight";
			//		break;

			//	case PlayerCondition.MoveLeft:
			//		animationName = "MoveLeft";
			//		break;

			//	case PlayerCondition.Stooping:
			//		animationName = "Stooping";
			//		break;
			//}

			//null�G���[���
			if (animationName != null) {
				//�w�肵���A�j���[�V�������̃A�j���[�V�������Đ�
				anim.Play(animationName);
			}
		}

		/// <summary>
		/// Player�̈ړ��𐧌�
		/// </summary>
		private PlayerCondition MovePlayer() {
			//Player�̌������J�����̌����ɍ��킹��
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);

			//�d�͂𐶐�
			moveDirection.y -= gravity * Time.deltaTime;

			//���[�J����Ԃ��烏�[���h��Ԃ�direction��ϊ����A���̌����Ƒ傫���Ɉړ�
			controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);

			//W��������Ă����
			if (Input.GetAxis("Vertical") > 0.0f) {
				//�i�s�����x�N�g����ݒ�
				moveDirection.z = Input.GetAxis("Vertical") * previousSpeed;

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.MovePrevious;
			}
			//S��������Ă����
			else if (Input.GetAxis("Vertical") < 0.0f) {
				//�i�s�����x�N�g����ݒ�
				moveDirection.z = Input.GetAxis("Vertical") * backSpeed;

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.MoveBack;
			}

			//D��������Ă����
			if (Input.GetAxis("Horizontal") > 0.0f) {
				//�i�s�����x�N�g����ݒ�
				moveDirection.x = Input.GetAxis("Horizontal") * speedX;

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.MoveRight;
			}
			//A��������Ă����
			else if (Input.GetAxis("Horizontal") < 0.0f) {
				//�i�s�����x�N�g����ݒ�
				moveDirection.x = Input.GetAxis("Horizontal") * speedX;

				//Player�̏�Ԃ�Ԃ�
				return PlayerCondition.MoveLeft;
			}

			//�����ރL�[��������Ă����
			if (Input.GetKey(stoopKey)) {
				//TODO:�����ޏ�����ǉ�

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
		public bool CheckGrounded() {
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
		private void ControlItem() {
			//�A�C�e���̐؂�ւ����s��
			ChangeItem(CheckKey());

			//�A�C�e���j���L�[�������ꂽ��
			if (Input.GetKeyDown(discardKey)) {
				//�A�C�e����j������
				ItemManager.instance.DiscardItem(ItemManager.instance.SelectedItemNo - 1);
			}
			//���N���b�N����Ă����
			else if (Input.GetKey(KeyCode.Mouse0)) {
				//�A�C�e�����g�p����
				UseItem(ItemManager.instance.playerItemList[ItemManager.instance.SelectedItemNo - 1]);
			}
			//�E�N���b�N����Ă����
			else if (Input.GetKey(KeyCode.Mouse1)) {
				//�I�����Ă���A�C�e�����X�i�C�p�[�ł͂Ȃ��Ȃ�
				if (ItemManager.instance.playerItemList[ItemManager.instance.SelectedItemNo - 1].itemName != ItemDataSO.ItemName.Sniper) {
					//�Y�[������
					followZoom.m_MaxFOV = normalZoomFOV;
					followZoom.m_MinFOV = 1.0f;
				}
				//�I�����Ă���A�C�e�����X�i�C�p�[�Ȃ�
				else {
					//�Y�[������
					followZoom.m_MaxFOV = ScopeZoomFOV;
					followZoom.m_MinFOV = 1.0f;

					//�X�R�[�v��`��
					uiManager.PeekIntoTheScope();
				}
			}
			//�E�N���b�N���I������
			else if (Input.GetKeyUp(KeyCode.Mouse1)) {
				//���̃J�����̔{���ɖ߂�
				followZoom.m_MaxFOV = 30.0f;
				followZoom.m_MinFOV = 30.0f;

				//�X�R�[�v��`���̂���߂�
				uiManager.NotPeekIntoTheScope();
			}

			//Player�̍ł��߂��ɂ���A�C�e���Ƃ̋������A�A�C�e�����擾�ł��Ȃ��قǗ���Ă��邩�A�A�C�e�������݂��Ȃ�������
			if (ItemManager.instance.LengthToNearItem > getItemLength || ItemManager.instance.generatedItemDataList.Count == 0) {
				//���b�Z�[�W����ɂ���
				uiManager.SetMessageText("", Color.black);

				//�ȉ��̏������s��Ȃ�
				return;
			}

			//���e�I�[�o�[���ǂ������ׂ�
			//GameData.instance.CheckIsFull();

			//���e�I�[�o�[���A�擾���悤�Ƃ��Ă���A�C�e�����e�ł͂Ȃ�������  !yamap.GameData.instance.CheckIsFull() &&GameData.instance.generatedItemDataList[GameData.instance.NearItemNo].itemType != ItemDataSO.ItemType.Bullet
			if (ItemManager.instance.IsFull && ItemManager.instance.generatedItemDataList[ItemManager.instance.NearItemNo].isNotBullet) //
			{
				//���b�Z�[�W��\��
				uiManager.SetMessageText("Tap 'X' To\nDiscard\nThe Item", Color.red);
			}
			//���e�I�[�o�[�ł͂Ȃ��Ȃ�
			else {
				//���b�Z�[�W��\��
				uiManager.SetMessageText("Tap 'Q' To\nGet The\nItem", Color.green);
			}

			//�A�C�e���擾�L�[�������ꂽ��
			if (Input.GetKeyDown(getItemKey)) {
                //�A�C�e�����擾����
                ItemManager.instance.GetItem(ItemManager.instance.NearItemNo, true);
			}
		}

		/// <summary>
		/// �����ꂽ�L�[�̏���Ԃ�
		/// </summary>
		private KeyCode CheckKey() {
			//�����L�[��������Ă��Ȃ��Ȃ�
			if (!Input.anyKeyDown)//���ʂȏ��������
			{
				//�L�[�̏���Ԃ�
				return KeyCode.None;
			}

			//1���L�[�̏������o��
			foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
				//foreach���Ŏ擾�����L�[�ƁA�����ꂽ�L�[�������Ȃ�
				if (Input.GetKeyDown(code)) {
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
		private void ChangeItem(KeyCode code) {
			//�����ꂽ�L�[�ɉ����Ďg�p���Ă���A�C�e���̔ԍ���ݒ�
			//switch (code) {
			//	case KeyCode.Alpha1:
   //                 GameData.instance.SelectedItemNo = 1;  // ���� 0 �X�^�[�g�ł͂Ȃ��H �z���List �Ƃ̑����������Ȃ�
			//		uiManager.SetItemSlotBackgroundColor(1, Color.red);
			//		break;

			//	case KeyCode.Alpha2:
   //                 GameData.instance.SelectedItemNo = 2;
			//		uiManager.SetItemSlotBackgroundColor(2, Color.red);
			//		break;

			//	case KeyCode.Alpha3:
   //                 GameData.instance.SelectedItemNo = 3;
			//		uiManager.SetItemSlotBackgroundColor(3, Color.red);
			//		break;

			//	case KeyCode.Alpha4:
   //                 GameData.instance.SelectedItemNo = 4;
			//		uiManager.SetItemSlotBackgroundColor(4, Color.red);
			//		break;

			//	case KeyCode.Alpha5:
   //                 GameData.instance.SelectedItemNo = 5;
			//		uiManager.SetItemSlotBackgroundColor(5, Color.red);
			//		break;
			//}
		}




		/// <summary>
		/// �A�C�e�����g�p����
		/// </summary>
		/// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
		public void UseItem(yamap.ItemDataSO.ItemData itemData) {

			switch (itemData.itemType) {
				case ItemDataSO.ItemType.FireArms:
                    // �e�𔭎�
                    bulletManager.ShotBullet(itemData);
					break;

				case ItemDataSO.ItemType.HandWeapon:
				    // �ߐڕ�����g�p���� �ʃN���X�ɂ���
				    bulletManager.PrepareUseHandWeapon(itemData);
					break;

            }


			//�g�p����A�C�e������ѓ���Ȃ�
			if (itemData.isMissile) {
				//�e�𔭎�
				bulletManager.ShotBullet(itemData);
			}
			//�g�p����A�C�e���ɉ񕜌��ʂ�����A���N���b�N���ꂽ��
			else if (itemData.restorativeValue > 0 && Input.GetKeyDown(KeyCode.Mouse0)) {


				////Player��Hp���X�V
				//playerHealth.UpdatePlayerHp(itemData.restorativeValue);

				////���̉񕜃A�C�e���̏�������1���炷
				//playerHealth.UpdateRecoveryItemCount(itemData.itemName, -1);



				//�I�����Ă���񕜃A�C�e���̏�������0�ɂȂ�����
				//if (playerHealth.GetRecoveryItemCount(GameData.instance.playerItemList[GameData.instance.SelectedItemNo - 1].itemName) == 0) {
				//	//�I�����Ă���A�C�e���̗v�f������
				//	itemManager.DiscardItem(GameData.instance.SelectedItemNo - 1);
				//}
			}
			//�g�p����A�C�e�����ߐڕ��킩�A���N���b�N���ꂽ��
			else if (itemData.isHandWeapon && Input.GetKeyDown(KeyCode.Mouse0)) {
				//�ߐڕ�����g�p����
				StartCoroutine(bulletManager.UseHandWeapon(itemData));
			}
		}
	}
}