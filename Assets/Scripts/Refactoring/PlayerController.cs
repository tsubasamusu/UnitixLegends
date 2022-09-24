using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//enum���g�p
using Cinemachine;//Cinemachine���g�p

namespace yamap 
{
	public class PlayerController : MonoBehaviour 
    {
        [SerializeField]
        private float previousSpeed = 10;//�O�i���x

        [SerializeField]
        private float backSpeed = 2;//��i���x

        [SerializeField]
        private float speedX = 2;//���E�ړ����x

        [SerializeField, Range(1.0f, 30.0f)]
        private float normalZoomFOV = 20;//�ʏ�̃Y�[�����̎���p

        [SerializeField, Range(1.0f, 30.0f)]
        private float ScopeZoomFOV = 5;//�X�R�[�v�ɂ��Y�[���̎���p

        [SerializeField]
        private float getItemLength = 1;//�A�C�e�����擾�ł��鋗��

        [SerializeField]
        private KeyCode stoopKey = KeyCode.E;//�����ރL�[

        [SerializeField]
        private KeyCode getItemKey = KeyCode.Q;//�A�C�e���擾�L�[

        [SerializeField]
        private KeyCode discardKey = KeyCode.X;//�A�C�e���j���L�[

        private Rigidbody playerRb;//Rigidbody

        private BoxCollider boxCollider;//BoxCollider

        private Animator anim;//Animator

        private PlayerHealth playerHealth;//PlayerHealth

        /// <summary>
        /// PlayerHealth�擾�p
        /// </summary>
        public PlayerHealth PlayerHealth { get => playerHealth; }

        private UIManager uiManager;//UIManager

        [SerializeField]
        private CinemachineFollowZoom followZoom;//CinemachineFollowZoom

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
        /// ���Z�b�g����
        /// </summary>
        private void Reset() 
        {
            //PlayerHealth�̎擾�Ɏ��s������
            if (!TryGetComponent(out playerHealth)) 
            {
                //�����
                Debug.Log("PlayerHealth �擾�o���܂���");
            }

            //Rigidbody�̎擾�Ɏ��s������
            if (!TryGetComponent(out playerRb)) 
            {
                //�����
                Debug.Log("Rigidbody �擾�o���܂���");
            } 
            //Rigidbody�̎擾�ɐ���������
            else 
            {
                //Rigidbody�ɂ��d�͂𖳌���
                playerRb.useGravity = false;

                //�������Z�𖳌���
                playerRb.isKinematic = true;
            }

            //BoxCollider�̎擾�Ɏ��s������
            if (!TryGetComponent(out boxCollider))
            {
                //�����
                Debug.Log("boxCollider �擾�o���܂���");
            }
            //BoxCollider�̎擾�ɐ���������
            else 
            {
                //�R���C�_�[�̃Z���^�[�̏����l���擾
                firstColliderCenter = boxCollider.center;

                //�R���C�_�[�̑傫���̏����l���擾
                firstColliderSize = boxCollider.size;
            }

            //Animator�̎擾�Ɏ��s������
            if (!TryGetComponent(out anim)) 
            {
                //�����
                Debug.Log("Animator �擾�o���܂���");
            }

            //���C���J�������擾
            mainCameraTran = Camera.main.transform;

            //�O�i���x��ݒ�
            previousSpeed = 10;

            //��i���x��ݒ�
            backSpeed = 2;

            //�������̈ړ����x��ݒ�
            speedX = 2;

            //�ʏ�̎���p��ݒ�
            normalZoomFOV = 20;

            //�X�R�[�v�g�p���̎���p��ݒ�
            ScopeZoomFOV = 5;

            //�����ރL�[��ݒ�
            stoopKey = KeyCode.E;

            //�A�C�e���擾�L�[��ݒ�
            getItemKey = KeyCode.Q;

            //�A�C�e���j���L�[��ݒ�
            discardKey = KeyCode.X;
        }

       /// <summary>
       /// Player�̏����ݒ���s��
       /// </summary>
       /// <param name="uiManager">UIManager</param>
        public void SetUpPlayer(UIManager uiManager) 
        {   
            ///���Z�b�g����
            Reset();

            //UIManager���擾
            this.uiManager = uiManager;

            //TODO:���߂����Ă�H
            //PlayerHealth��null�ł͂Ȃ��Ȃ�APlayerHealth�̏����ݒ���s��
            playerHealth?.SetUpHealth(uiManager);
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
                SoundManager.instance.PlaySE(SeName.LandingSE);

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
		/// <param name="playerCondition">PlayerCondition</param>
		private void PlayAnimation(PlayerCondition playerCondition) 
        {
			//�w�肵���A�j���[�V�������̃A�j���[�V�������Đ�
			anim.Play(playerCondition.ToString());
		}

		/// <summary>
        /// Player�̍s���𐧌䂷��
        /// </summary>
        /// <returns>Player�̏��</returns>
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
        /// Player�̐ڒn������s��
        /// </summary>
        /// <returns>Player���ڒn���Ă�����tru</returns>
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
            //�����L�[�������ꂽ��
            if (Input.anyKeyDown)
            {
                //�A�C�e���̐؂�ւ����s��
                ChangeItem(CheckKey());
            }

            //�A�C�e���j���L�[�������ꂽ��
            if (Input.GetKeyDown(discardKey)) 
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySE(SeName.DiscardItemSE);

                //�A�C�e����j������
                ItemManager.instance.DiscardItem(SelectedItemNo - 1);
            }
            //���N���b�N����Ă����
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                //�A�C�e�����g�p����
                ItemManager.instance.UseItem(ItemManager.instance.GetSelectedItemData(), playerHealth);
            }
            //�E�N���b�N����Ă����
            else if (Input.GetKey(KeyCode.Mouse1)) 
            {
                //�I�����Ă���A�C�e�����X�i�C�p�[�ł͂Ȃ��Ȃ�
                if (ItemManager.instance.GetSelectedItemData().itemName != ItemDataSO.ItemName.Sniper)
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
                if (!ItemManager.instance.GetSelectedItemData().enemyCanUse) 
                {
                    //�ȍ~�̏������s��Ȃ�
                    return;
                }

                //���ʉ����Đ�
                SoundManager.instance.PlaySE(SeName.BePreparedSE);
            }

            //Player�̍ł��߂��ɂ���A�C�e���Ƃ̋������A�A�C�e�����擾�ł��Ȃ��قǗ���Ă��邩�A�A�C�e�������݂��Ȃ�������
            if (ItemManager.instance.LengthToNearItem > getItemLength || ItemManager.instance.generatedItemDataList.Count == 0)
            {
                //���b�Z�[�W����ɂ���
                uiManager.SetMessageText("", Color.black);

                //�ȉ��̏������s��Ȃ�
                return;
            }

            //���e�I�[�o�[���ǂ������ׂ�
            ItemManager.instance.CheckIsFull();

            //���e�I�[�o�[���A�擾���悤�Ƃ��Ă���A�C�e�����e�ł͂Ȃ�������
            if (ItemManager.instance.IsFull && ItemManager.instance.generatedItemDataList[ItemManager.instance.NearItemNo].itemType != ItemDataSO.ItemType.Bullet) 
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
                ItemManager.instance.GetItem(ItemManager.instance.NearItemNo, true, playerHealth);
            }
        }

		/// <summary>
		/// �����ꂽ�L�[�̏���Ԃ�
		/// </summary>
		private KeyCode CheckKey() 
        {
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
        /// �g�p�A�C�e����؂�ւ���
        /// </summary>
        /// <param name="code">�����ꂽ�L�[</param>
		private void ChangeItem(KeyCode code)
        {
			//�����ꂽ�L�[�ɉ����Ďg�p���Ă���A�C�e���̔ԍ���ݒ�
			switch (code) 
            {
				case KeyCode.Alpha1:
					ItemManager.instance.SelectedItemNo = 0;
					break;

				case KeyCode.Alpha2:
                    ItemManager.instance.SelectedItemNo = 1;
					break;

				case KeyCode.Alpha3:
                    ItemManager.instance.SelectedItemNo = 2;
					break;

				case KeyCode.Alpha4:
                    ItemManager.instance.SelectedItemNo = 3;
					break;

				case KeyCode.Alpha5:
                    ItemManager.instance.SelectedItemNo = 4;
					break;
                default:
                    return;
			}

            //�A�C�e���X���b�g�̔w�i�F��ݒ�
            uiManager.SetItemSlotBackgroundColor(ItemManager.instance.SelectedItemNo, Color.red);

            //�I�������A�C�e����None�Ȃ�
            if (ItemManager.instance.GetSelectedItemData().itemName == ItemDataSO.ItemName.None) 
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySE(SeName.NoneItemSE);
            }
            //�I�������A�C�e����None�ł͂Ȃ�������
            else
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySE(SeName.SelectItemSE);
            }
        }
	}
}