using DG.Tweening;
using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

namespace yamap {

    public class BulletManager : MonoBehaviour {

        [SerializeField]
        private Transform mainCamera;//���C���J����

        [SerializeField]
        private Transform temporaryObjectContainerTran;//�ꎞ�I�ɃQ�[���I�u�W�F�N�g�����e����Transform

        [SerializeField]
        private SoundManager soundManager;//SoundManager

        [SerializeField]
        private PlayerController playerController;//PlayerController

        private float timer;//�o�ߎ���

        private bool stopFlag;//�d��������h��

        private int grenadeBulletCount;//��֒e�̎c��̐�

        public int GrenadeBulletCount//grenadeBulletCount�ϐ��p�̃v���p�e�B
        {
            get { return grenadeBulletCount; }//�O������͎擾�����݂̂��\��
        }

        private int tearGasGrenadeBulletCount;//�×ܒe�̎c��̐�

        public int TearGasGrenadeBulletCount//tearGasGrenadeBulletCount�ϐ��p�̃v���p�e�B
        {
            get { return tearGasGrenadeBulletCount; }//�O������͎擾�����݂̂��\��
        }

        private int assaultBulletCount;//�A�T���g�p�e�̎c�e��

        public int AssaultBulletCount//assaultBulletCount�ϐ��p�̃v���p�e�B
        {
            get { return assaultBulletCount; }//�O������͎擾�����݂̂��\��
        }

        private int sniperBulletCount;//�X�i�C�p�[�p�e�̎c�e��

        public int SniperBulletCount//sniperBulletCount�ϐ��p�̃v���p�e�B
        {
            get { return sniperBulletCount; }//�O������͎擾�����݂̂��\��
        }

        private int shotgunBulletCount;//�V���b�g�K���p�e�̎c�e��

        public int ShotgunBulletCount//shotgunBulletCount�ϐ��p�̃v���p�e�B
        {
            get { return shotgunBulletCount; }//�O������͎擾�����݂̂��\��
        }

        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        private void Start() {
            // �e�̍ő吔���Z�b�g


            //�o�ߎ��Ԃ̌v�����J�n
            StartCoroutine(MeasureTime());
        }

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() {
            //Bullet�𔭎˂���������J�����̌����ɍ��킹��
            transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);
        }

        /// <summary>
        /// �o�ߎ���1���v������
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator MeasureTime() {
            //�������[�v
            while (true) {
                //�o�ߎ��Ԃ��v��
                timer += Time.deltaTime;

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }

        /// <summary>
        /// �c�e�����X�V����
        /// </summary>
        /// <param name="itemName">�A�C�e���̖��O</param>
        /// <param name="updateValue">�c�e���̕ύX��</param>
        public void UpdateBulletCount(ItemDataSO.ItemName itemName, int updateValue) {
            //�󂯎�����A�C�e���̖��O�ɉ����ď�����ύX
            switch (itemName) {
                //��֒e�Ȃ�
                case ItemDataSO.ItemName.Grenade:
                    grenadeBulletCount = Mathf.Clamp(grenadeBulletCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Grenade).maxBulletCount);
                    break;

                //�×ܒe�Ȃ�
                case ItemDataSO.ItemName.TearGasGrenade:
                    tearGasGrenadeBulletCount = Mathf.Clamp(tearGasGrenadeBulletCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.TearGasGrenade).maxBulletCount);
                    break;

                //�A�T���g�Ȃ�
                case ItemDataSO.ItemName.Assault:
                    assaultBulletCount = Mathf.Clamp(assaultBulletCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Assault).maxBulletCount);
                    break;

                //�V���b�g�K���Ȃ�
                case ItemDataSO.ItemName.Shotgun:
                    shotgunBulletCount = Mathf.Clamp(shotgunBulletCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Shotgun).maxBulletCount);
                    break;

                //�X�i�C�p�[�Ȃ�
                case ItemDataSO.ItemName.Sniper:
                    sniperBulletCount = Mathf.Clamp(sniperBulletCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Sniper).maxBulletCount);
                    break;
            }
        }

        /// <summary>
        /// �e�𔭎˂���(�߂�l�� void �ɂ���)
        /// </summary>
        /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
        /// <returns>�҂�����</returns>
        public void ShotBullet(ItemDataSO.ItemData itemData)   // ���ݑI�����Ă���A�C�e���̏��
        {
            //�o�ߎ��Ԃ��A�ˊԊu��菬�������A�c�e����0�Ȃ�
            if (timer < itemData.interval || GetBulletCount(itemData.itemName) == 0) {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�e�𐶐�
            BulletDetailBase bullet = Instantiate(itemData.weaponPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0)).GetComponent<BulletDetailBase>();

            //�e�𔭎�
            bullet.SetUpBulletDetail(itemData.attackPower, BulletOwnerType.Player, transform.forward * itemData.shotSpeed, itemData.seName);

            //Player�̎�������˂����e�͈�莞�Ԏc��
            bullet.gameObject.transform.SetParent(temporaryObjectContainerTran);

            //�c�e�������炷
            UpdateBulletCount(itemData.itemName, -1);

            //�o�ߎ��Ԃ�������
            timer = 0;

            // ���イ�e���A�×ܒe��
            if (itemData.itemName == ItemDataSO.ItemName.Grenade || itemData.itemName == ItemDataSO.ItemName.TearGasGrenade) {
                // �e���� 0 �ɂȂ�����
                if (GetBulletCount(itemData.itemName) <= 0) {
                    // �A�C�e����j��
                    ItemManager.instance.DiscardItem(ItemManager.instance.SelectedItemNo - 1);
                }
            }
        }

        /// <summary>
        /// �w�肵���A�C�e���̎c�e�����擾����
        /// </summary>
        /// <param name="itemName">���̒e���g�p����A�C�e��</param>
        /// <returns>���̃A�C�e�����g�p����e�̎c�e��</returns>
        public int GetBulletCount(ItemDataSO.ItemName itemName) {
            //�I�����Ă���A�C�e���̖��O�ɉ����ď�����ύX
            switch (itemName) {
                //��֒e�Ȃ�
                case ItemDataSO.ItemName.Grenade:
                    return grenadeBulletCount;

                //�×ܒe�Ȃ�
                case ItemDataSO.ItemName.TearGasGrenade:
                    return tearGasGrenadeBulletCount;

                //�A�T���g�Ȃ�
                case ItemDataSO.ItemName.Assault:
                    return assaultBulletCount;

                //�X�i�C�p�[�Ȃ�
                case ItemDataSO.ItemName.Sniper:
                    return sniperBulletCount;

                //�V���b�g�K���Ȃ�
                case ItemDataSO.ItemName.Shotgun:
                    return shotgunBulletCount;

                //��L�ȊO�Ȃ�
                default:
                    return 0;
            }
        }

        /// <summary>
        /// �O������ߐڕ�����g�p����ۂɌĂяo��
        /// </summary>
        /// <param name="itemData"></param>
        public void PrepareUseHandWeapon(ItemDataSO.ItemData itemData) {
            StartCoroutine(UseHandWeapon(itemData));
        }

        /// <summary>
        /// �ߐڕ�����g�p����
        /// </summary>
        /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
        /// <returns>�҂�����</returns>
        public IEnumerator UseHandWeapon(ItemDataSO.ItemData itemData) {
            //stopFlag��true�Ȃ�
            if (stopFlag) {
                //�ȍ~�̏������s��Ȃ�
                yield break;
            }

            //�d��������h��
            stopFlag = true;

            // �ߐڕ���ɂ��N���X�����āA�U�镑����ς���
            // ����A���킪�����Ă��A�v���O�����͏C�����Ȃ��čς�

            //�A�C�e���𐶐�
            HandWeaponDetailBase handWeapon = Instantiate(itemData.weaponPrefab, transform).GetComponent<HandWeaponDetailBase>();

            // �A�C�e���̐ݒ�ƍU��
            handWeapon.SetUpWeaponDetail(itemData.attackPower, BulletOwnerType.Player, itemData.seName, itemData.interval);
            
            //�A�C�e���̈ʒu��ݒ�(�A�C�e�����̃N���X�ōs���Ă��悢)
            handWeapon.gameObject.transform.localPosition = Vector3.zero;

            //�A�C�e���̃A�j���[�V�������I���܂ő҂�
            yield return new WaitForSeconds(itemData.interval);

            //�g�p����
            stopFlag = false;
        }
    }
}