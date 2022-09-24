using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshAgent���g�p

namespace yamap 
{
    public class EnemyController : MonoBehaviour 
    {
        private NavMeshAgent agent;//NavMeshAgent

        private Animator animator;//Animator

        [SerializeField, Header("�˒�����")]
        private float range;//�˒�����

        [SerializeField]
        private float getItemLength;//�A�C�e�����擾�ł��鋗��

        [SerializeField]
        private Transform enemyWeaponTran;//Enemy��������\����ʒu

        private bool didPostLandingProcessing;//���n����̏������s�������ǂ���

        private bool gotItem;//�A�C�e�����擾�������ǂ���

        private bool stopFlag;//�������~���邩�ǂ���

        private float enemyhp = 100f;//Enemy�̗̑�

        private float timer;//�o�ߎ���

        private Vector3 firstPos;//�����ʒu

        private ItemDataSO.ItemData usedItemData;//�g�p���Ă���A�C�e���̃f�[�^

        private UIManager uiManager;//UIManager

        private EnemyGenerator enemyGenerator;//EnemyGenerator

        private Transform shotBulletTran;//�e�𐶐�����ʒu

        private Transform playerTran;//Player�̈ʒu

        private ItemDetail usedItemObj;//�g�p���Ă���A�C�e���̃I�u�W�F�N�g

        private int nearItemNo;//�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ�

        private int myNo;//�������g�̔ԍ�


        /// <summary>
        /// Enemy�̏����ݒ���s��
        /// </summary>
        /// <param name="uiManager">UIManager</param>
        /// <param name="enemyGenerator">EnemyGenerator</param>
        /// <param name="player">PlayerController</param>
        /// <param name="no">Enemy�̔ԍ�</param>
        public void SetUpEnemy(UIManager uiManager, EnemyGenerator enemyGenerator, PlayerController player, int no) 
        {
            //NavMeshAgent�̎擾�Ɏ��s������
            if (!TryGetComponent(out agent)) 
            {
                Debug.Log("NavMeshAgent�̎擾�Ɏ��s");
            }
            //NavMeshAgent�̎擾�ɐ���������
            else
            {
                //NavMeshAgent�𖳌���
                agent.enabled = false;
            }

            //Animator�̎擾�Ɏ��s������
            if (!TryGetComponent(out animator)) 
            {
                Debug.Log("Animator�̎擾�Ɏ��s");
            }
            
            //UIManager���擾
            this.uiManager = uiManager;

            //EnemyGenerator���擾
            this.enemyGenerator = enemyGenerator;

            //Player�̈ʒu�����擾
            playerTran = player.transform;

            //�����̔ԍ����擾
            myNo = no;

            //�o�ߎ��Ԃ̌v�����J�n
            StartCoroutine(MeasureTime());

            //���ˈʒu���擾
            shotBulletTran = transform.GetChild(3).transform;
        }

        /// <summary>
        /// ��莞�Ԃ��ƂɌĂяo�����
        /// </summary>
        private void FixedUpdate()//�S�Ă̒[���œ����ړ����x�ɂ��邽�߂�FixedUpdate���\�b�h���g��
        {
            //�ڒn���Ă��Ȃ�������
            if (!CheckGrounded()) 
            {
                //��������
                transform.Translate(0, -GameData.instance.FallSpeed, 0);
            }
        }

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() 
        {
            //�ڒn���Ă��Ȃ��Ȃ�
            if (!CheckGrounded()) 
            {
                //�ȉ��̏������s��Ȃ�
                return;
            }

            //�܂����n����̏������s���Ă��Ȃ��Ȃ�
            if (!didPostLandingProcessing) 
            {
                //NavMeshAgent��L����
                agent.enabled = true;

                //��~������0�ɐݒ�
                agent.stoppingDistance = 0f;

                //�A�j���[�V�����̐�����J�n
                StartCoroutine(ControlAnimation());

                //���n����̏���������������Ԃɐ؂�ւ���
                didPostLandingProcessing = true;
            }

            //��~��ԂȂ�
            if (stopFlag) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�܂��A�C�e�����擾���Ă��Ȃ��Ȃ�
            if (!gotItem) 
            {
                //�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ���ݒ�
                SetNearItemNo();

                //�ڕW�n�_��ݒ�
                SetTargetPosition(ItemManager.instance.generatedItemTranList[nearItemNo].position);

                //�A�C�e�����擾�ł��鋗���܂ŋ߂Â�����
                if (GetLengthToNearItem(nearItemNo) <= getItemLength) 
                {
                    //�A�C�e�����擾���A�A�C�e�����擾�ς݂̏�Ԃɐ؂�ւ���
                    gotItem = GetItem(nearItemNo);

                    //��~������ݒ�
                    agent.stoppingDistance = 30f;
                }

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�g�p�ł��Ȃ��A�C�e�����E���Ă��܂�����
            if (!usedItemData.enemyCanUse) 
            {
                //�A�C�e�����܂��擾���Ă��Ȃ���Ԃɐ؂�ւ���
                gotItem = false;

                //��~������0�ɐݒ�
                agent.stoppingDistance = 0f;

                //�g�p���Ă���A�C�e���̃I�u�W�F�N�g������
                Destroy(usedItemObj);

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�G�̈ʒu��ڕW�n�_�ɐݒ�
            SetTargetPosition(GetNearEnemyPos());

            //�ː���ɓG��������
            if (CheckEnemy()) 
            {
                //�ˌ�����
                ShotBullet(usedItemData);
            }
        }

        /// <summary>
        /// �ł��߂��ɂ���G�̈ʒu���擾����
        /// </summary>
        /// <returns>�ł��߂��ɂ���G�̈ʒu</returns>
        private Vector3 GetNearEnemyPos() 
        {
            //�ł��߂��ɂ���G�̈ʒu��Player�̈ʒu�����ɓo�^
            Vector3 nearPos = playerTran.position;

            //��������Enemy�̈ʒu���̃��X�g�̗v�f�������J��Ԃ�
            for (int i = 0; i < enemyGenerator.generatedEnemyList.Count; i++) 
            {
                //�J��Ԃ������Ŏ擾�����G��������������
                if (i == myNo) 
                {
                    //���̌J��Ԃ������Ɉڂ�
                    continue;
                }

                //�J��Ԃ������Ŏ擾�����G�����S���Ă�����inull�G���[����j
                if (enemyGenerator.generatedEnemyList[i] == null)
                {
                    //����Enemy�����X�g�����菜��
                    enemyGenerator.generatedEnemyList.RemoveAt(i);

                    //���̌J��Ԃ������Ɉڂ�
                    continue;
                }

                //���X�g��i�Ԃ̗v�f�̍��W��pos�ɓo�^
                Vector3 pos = enemyGenerator.generatedEnemyList[i].transform.position;

                //���o�^�����v�f�ƁAfor���œ����v�f�́AmyPos�Ƃ̋������r
                if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude) 
                {
                    //nearPos���ēo�^
                    nearPos = pos;
                }
            }

            //nearPos��Ԃ�
            return nearPos;
        }

        /// <summary>
        /// �ł��߂��ɂ���g�p�\�A�C�e���̔ԍ���ݒ肷��
        /// </summary>
        private void SetNearItemNo() 
        {
            //null�G���[���
            if (ItemManager.instance.generatedItemTranList.Count <= 0) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�A�C�e���̔ԍ�
            int itemNo = 0;

            //���X�g��0�Ԃ̗v�f�̍��W��nearPos�ɉ��ɓo�^
            Vector3 nearPos = ItemManager.instance.generatedItemTranList[0].position;

            //���X�g�̗v�f�������J��Ԃ�
            for (int i = 0; i < ItemManager.instance.generatedItemTranList.Count; i++) 
            {
                //�J��Ԃ������Ō������A�C�e�����g�p�s��������
                if (!ItemManager.instance.generatedItemDataList[i].enemyCanUse) 
                {
                    //�ȍ~�̏����͍s�킸�ɁA���̌J��Ԃ��Ɉڂ�
                    continue;
                }

                //���X�g��i�Ԃ̗v�f�̍��W��pos�ɓo�^
                Vector3 pos = ItemManager.instance.generatedItemTranList[i].position;

                //���o�^�����v�f�ƁAfor���œ����v�f�́AmyPos�Ƃ̋������r
                if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude) 
                {
                    //Player�̍ł��߂��ɂ���A�C�e���̔ԍ���i�œo�^
                    itemNo = i;

                    //nearPos���ēo�^
                    nearPos = pos;
                }
            }

            //�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ���ݒ�
            nearItemNo = itemNo;
        }

        /// <summary>
        /// �ł��߂��ɂ���g�p�\�A�C�e���Ƃ̋������擾
        /// </summary>
        /// <param name="nearItemNo">�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ�</param>
        /// <returns>�ł��߂��ɂ���g�p�\�A�C�e���Ƃ̋���</returns>
        private float GetLengthToNearItem(int nearItemNo) 
        {
            //�ł��߂��ɂ���g�p�\�A�C�e���Ƃ̋�����Ԃ�
            return Vector3.Scale((ItemManager.instance.generatedItemTranList[nearItemNo].position - transform.position), new Vector3(1, 0, 1)).magnitude;
        }

        /// <summary>
        /// �ڕW�n�_��ݒ肷��
        /// </summary>
        /// <param name="targetPos">�ڕW�n�_</param>
        private void SetTargetPosition(Vector3 targetPos) 
        {
            //���������ɁAAI�̖ڕW�n�_��ݒ�
            agent.destination = targetPos;
        }

        /// <summary>
        /// �ڒn������s��
        /// </summary>
        /// <returns>���g���ڒn���Ă�����true</returns>
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
        /// �ː���ɓG�����邩�ǂ������ׂ�
        /// </summary>
        /// <returns>�ː���ɓG��������true</returns>
        private bool CheckEnemy() 
        {
            //�����̏����ʒu�ƌ����i�p���j��ݒ�
            Ray ray = new Ray(enemyWeaponTran.position, enemyWeaponTran.forward);

            //���������ɂ�������Ȃ�������
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, range)) 
            {
                //false��Ԃ��A�ȍ~�̏������s��Ȃ�
                return false;
            }

            //������Player��Enemy�ɓ���������
            if (hitInfo.transform.gameObject.CompareTag("Player") || hitInfo.transform.gameObject.CompareTag("Enemy")) 
            {
                //true��Ԃ�
                return true;
            }

            //Player��Enemy�ȊO�Ɍ���������������false��Ԃ�
            return false;
        }

        /// <summary>
        /// �A�C�e�����E��
        /// </summary>
        /// <param name="nearItemNo">�߂��̃A�C�e���̔ԍ�</param>
        /// <returns>�A�C�e�����E���I������true��Ԃ�</returns>
        public bool GetItem(int nearItemNo) 
        {
            //�g�p����A�C�e���̃f�[�^��ݒ�
            usedItemData = ItemManager.instance.generatedItemDataList[nearItemNo];

            //�擾�����A�C�e����z�u
            usedItemObj = Instantiate(ItemManager.instance.generatedItemDataList[nearItemNo].itemPrefab, enemyWeaponTran);

            //�A�C�e�����E��
            ItemManager.instance.GetItem(nearItemNo, false);

            //true��Ԃ�
            return true;
        }

        /// <summary>
        /// �o�ߎ��Ԃ��v������
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator MeasureTime() 
        {
            //�������[�v
            while (true) 
            {
                //�o�ߎ��Ԃ��v��
                timer += Time.deltaTime;

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }


        /// <summary>
        /// �ˌ�����
        /// </summary>
        /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
        private void ShotBullet(ItemDataSO.ItemData itemData) 
        {
            //�o�ߎ��Ԃ��A�ˊԊu��菬�����Ȃ�
            if (timer < itemData.interval) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�e�𐶐����A���������e����BulletDetailBase�N���X���擾
            BulletDetailBase bullet = Instantiate(itemData.weaponPrefab, shotBulletTran.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0)).GetComponent<BulletDetailBase>();

            //BulletDetailBase�N���X�̏����ݒ���s��
            bullet.SetUpBulletDetail(itemData.attackPower, BulletOwnerType.Enemy, enemyWeaponTran.forward, itemData.seName, itemData.interval, itemData.effectPrefab);

            //���������e�̐e�����g�ɐݒ�
            bullet.transform.parent = transform;

            //�o�ߎ��Ԃ�������
            timer = 0;
        }

        /// <summary>
        /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
        /// </summary>
        /// <param name="collision">�ڐG����</param>
        private void OnCollisionEnter(Collision collision) 
        {
            //�������������e���e�����Ȃ�
            if (collision.transform.parent == transform) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�e�ɐG�ꂽ��
            if (collision.gameObject.TryGetComponent(out BulletDetailBase bulletDetail)) 
            {
                //�G�ꂽ�e�̍U���͂��擾
                float attackPower = bulletDetail.GetAttackPower();

                //���̒e�̎����傪Player�Ȃ�
                if (bulletDetail.BulletOwnerType == BulletOwnerType.Player) 
                {
                    //�t���[�g�\���𐶐� -> �Ȃ�ׂ��O������Ȃ�
                    //TODO:�u�O�����璼�ڃR���[�`�����J�n����͔̂�����ׂ��v�Ƃ������Ƃ��H)
                    uiManager.PrepareGenerateFloatingMessage(Mathf.Abs(attackPower).ToString("F0"), Color.yellow);
                }

                //Enemy�̗̑͂��X�V
                UpdateEnemyHp(attackPower, bulletDetail.BulletOwnerType);

                //�e�̒ǉ����ʂ��s��
                bulletDetail.AddTriggerBullet(this);
            }
        }

        /// <summary>
        /// Enemy�̗̑͂��X�V����
        /// </summary>
        /// <param name="updateValue">�̗͂̍X�V��</param>
        /// <param name="bulletOwnerType">�e�̏��L��</param>
        private void UpdateEnemyHp(float updateValue, BulletOwnerType bulletOwnerType = BulletOwnerType.Player) 
        {
            //Enemy�̗̑͂�0�ȏ�100�ȉ��ɐ������Ȃ���A�X�V����
            enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

            //Enemy�̗̑͂�0�ɂȂ�����
            if (enemyhp == 0.0f) 
            {
                //�U�����Ă������肪Player�Ȃ�
                if (bulletOwnerType == BulletOwnerType.Player)
                {
                    //Player���|�����G�̐���1��������
                    yamap.GameData.instance.KillCount++;
                }

                //���g������
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// �×ܒe���ʂ̏������s��
        /// </summary>
        public void PrepareTearGasGrenade() 
        {
            //�×ܒe���󂯂��ۂ̏������J�n����
            StartCoroutine(AttackedByTearGasGrenade());
        }

        /// <summary>
        /// �×ܒe���󂯂��ۂ̏���
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator AttackedByTearGasGrenade() 
        {
            //��~��Ԃɂ���
            stopFlag = true;

            //�ڕW�n�_�����g�̍��W�ɐݒ�
            SetTargetPosition(transform.position);

            //5.0�b�ԁA�������~�ߑ�����
            yield return new WaitForSeconds(5.0f);

            //��~��Ԃ���������
            stopFlag = false;
        }

        /// <summary>
        /// ���S����
        /// </summary>
        private void WasKilled() 
        {
            //���S����
        }

        /// <summary>
        /// �A�j���[�V�����𐧌䂷��
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator ControlAnimation() 
        {
            //�����ɌJ��Ԃ�
            while (true) 
            {
                //�����ʒu��ݒ�
                firstPos = transform.position;

                //0.1�b�҂�
                yield return new WaitForSeconds(0.1f);

                //���݈ʒu��ݒ�
                Vector3 currentPos = transform.position;

                //���x���擾
                float velocity = (currentPos - firstPos).magnitude / 0.1f;

                //����
                animator.SetBool("MovePrevious", velocity > 0.1f);

                //�������Ȃ�
                animator.SetBool("Idle", velocity <= 0.1f);

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }
    }
}