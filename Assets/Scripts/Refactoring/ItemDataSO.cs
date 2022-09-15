using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//Serializable属性を使用

namespace yamap {

    //アセットメニューで「Create ItemDataSO」を選択すると、「ItemDataSO」を作成できる
    [CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
    public class ItemDataSO : ScriptableObject {
        /// <summary>
        /// アイテムの名前
        /// </summary>
        public enum ItemName {
            None,//何者でもない
            Grenade,//手榴弾
            TearGasGrenade,//催涙弾
            Knife,//ナイフ
            Bat,//バット
            Assault,//アサルトライフル
            Shotgun,//ショットガン
            Sniper,//スナイパーライフル
            Bandage,//包帯
            MedicinalPlants,//薬草
            Syringe,//注射器
            AssaultBullet,//アサルト用弾
            ShotgunBullet,//ショットガン用弾
            SniperBullet//スナイパー用弾
        }

        /// <summary>
        /// アイテムの種類
        /// </summary>
        public enum ItemType {
            Missile,
            HandWeapon,
            Bullet
        }

        /// <summary>
        /// アイテムのデータを管理するクラス
        /// </summary>
        [Serializable]
        public class ItemData {
            public ItemName itemName;//アイテムの名前
            [Range(0.0f, 100.0f)]
            public float restorativeValue;//回復量
            [Range(0.0f, 100.0f)]
            public float attackPower;//攻撃力
            public float shotSpeed;//発射速度
            public float interval;//連射間隔
            public float timeToExplode;//爆破・ガス発生までの時間
            public int bulletCount;//弾の数
            public int maxBulletCount;//一度に所持できる弾の最大数
            public bool enemyCanUse;//Enemyが使用できるかどうか
            //public bool isNotBullet;//弾のアイテムではないかどうか
            //public bool isMissile;//飛び道具かどうか
            //public bool isHandWeapon;//近接武器かどうか
            public Sprite sprite;//Sprite
            public GameObject prefab;//落ちている方のプレファブ

            public ItemType itemType;       　　　　　  // アイテムの種類
            public WeaponBase weaponPrefab; 　　　　　  // 弾と近接武器のプレファブ
            public SeName seName;                       // SE の種類
            public GameObject effectPrefab;
        }

        public List<ItemData> itemDataList = new List<ItemData>();//アイテムデータのリスト
    }
}