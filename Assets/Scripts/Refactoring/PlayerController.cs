using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//enumを使用
using Cinemachine;//Cinemachineを使用

namespace yamap 
{
	public class PlayerController : MonoBehaviour 
    {
        [SerializeField]
        private float previousSpeed = 10;//前進速度

        [SerializeField]
        private float backSpeed = 2;//後進速度

        [SerializeField]
        private float speedX = 2;//左右移動速度

        [SerializeField, Range(1.0f, 30.0f)]
        private float normalZoomFOV = 20;//通常のズーム時の視野角

        [SerializeField, Range(1.0f, 30.0f)]
        private float ScopeZoomFOV = 5;//スコープによるズームの視野角

        [SerializeField]
        private float getItemLength = 1;//アイテムを取得できる距離

        [SerializeField]
        private KeyCode stoopKey = KeyCode.E;//かがむキー

        [SerializeField]
        private KeyCode getItemKey = KeyCode.Q;//アイテム取得キー

        [SerializeField]
        private KeyCode discardKey = KeyCode.X;//アイテム破棄キー

        private Rigidbody playerRb;//Rigidbody

        private BoxCollider boxCollider;//BoxCollider

        private Animator anim;//Animator

        private PlayerHealth playerHealth;//PlayerHealth

        /// <summary>
        /// PlayerHealth取得用
        /// </summary>
        public PlayerHealth PlayerHealth { get => playerHealth; }

        private UIManager uiManager;//UIManager

        [SerializeField]
        private CinemachineFollowZoom followZoom;//CinemachineFollowZoom

        private Transform mainCameraTran;//メインカメラの位置

        [SerializeField]
        private Transform playerCharacterTran;//Playerのキャラクターの位置

        private Vector3 moveDirection = Vector3.zero;//進行方向ベクトル

        private Vector3 desiredMove = Vector3.zero;//移動ベクトル

        private Vector3 firstColliderCenter;//コライダーのセンターの初期値

        private Vector3 firstColliderSize;//コライダーの大きさの初期値

        private bool landed;//飛行機から飛び降りて着地したかどうか

        private int selectedItemNo = 1;//使用しているアイテムの番号

        public int SelectedItemNo//useItemNo変数用のプロパティ
        {
            get { return selectedItemNo; }//外部からは取得処理のみ可能に
        }

        /// <summary>
        /// Playerの状態
        /// </summary>
        private enum PlayerCondition 
        {
			Idle,//何もしていない
			MoveBack,//後進している
			MovePrevious,//前進している
			MoveRight,//右移動している
			MoveLeft,//左移動している
			Stooping//かがんでいる
		}

        /// <summary>
        /// リセットする
        /// </summary>
        private void Reset() 
        {
            //PlayerHealthの取得に失敗したら
            if (!TryGetComponent(out playerHealth)) 
            {
                //問題を報告
                Debug.Log("PlayerHealth 取得出来ません");
            }

            //Rigidbodyの取得に失敗したら
            if (!TryGetComponent(out playerRb)) 
            {
                //問題を報告
                Debug.Log("Rigidbody 取得出来ません");
            } 
            //Rigidbodyの取得に成功したら
            else 
            {
                //Rigidbodyによる重力を無効化
                playerRb.useGravity = false;

                //物理演算を無効化
                playerRb.isKinematic = true;
            }

            //BoxColliderの取得に失敗したら
            if (!TryGetComponent(out boxCollider))
            {
                //問題を報告
                Debug.Log("boxCollider 取得出来ません");
            }
            //BoxColliderの取得に成功したら
            else 
            {
                //コライダーのセンターの初期値を取得
                firstColliderCenter = boxCollider.center;

                //コライダーの大きさの初期値を取得
                firstColliderSize = boxCollider.size;
            }

            //Animatorの取得に失敗したら
            if (!TryGetComponent(out anim)) 
            {
                //問題を報告
                Debug.Log("Animator 取得出来ません");
            }

            //メインカメラを取得
            mainCameraTran = Camera.main.transform;

            //前進速度を設定
            previousSpeed = 10;

            //後進速度を設定
            backSpeed = 2;

            //横方向の移動速度を設定
            speedX = 2;

            //通常の視野角を設定
            normalZoomFOV = 20;

            //スコープ使用時の視野角を設定
            ScopeZoomFOV = 5;

            //かがむキーを設定
            stoopKey = KeyCode.E;

            //アイテム取得キーを設定
            getItemKey = KeyCode.Q;

            //アイテム破棄キーを設定
            discardKey = KeyCode.X;
        }

       /// <summary>
       /// Playerの初期設定を行う
       /// </summary>
       /// <param name="uiManager">UIManager</param>
        public void SetUpPlayer(UIManager uiManager) 
        {   
            ///リセットする
            Reset();

            //UIManagerを取得
            this.uiManager = uiManager;

            //TODO:解釈あってる？
            //PlayerHealthがnullではないなら、PlayerHealthの初期設定を行う
            playerHealth?.SetUpHealth(uiManager);
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        void Update() 
        {
            //Playerが裏世界に行ってしまったら
            if (transform.position.y <= -1f) 
            {
                //自身の座標を(0,0,0)に設定
                transform.position = Vector3.zero;

                //以降の処理を行わない
                return;
            }

            //Playerが転倒していたら
            if (CheckToppled()) 
            {
                //メッセージを表示
                uiManager.SetMessageText("I'm\nTrying To\nRecover", Color.red);

                //態勢を立て直す
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            }

            //接地していなかったら
            if (!CheckGrounded()) 
            {
                //以降の処理を行わない
                return;
            }

            //アニメーションの再生と、Playerの動きの制御を行う
            PlayAnimation(MovePlayer());

            //アイテムを制御
            ControlItem();
        }

        /// <summary>
        /// 一定時間ごとに呼び出される
        /// </summary>
        private void FixedUpdate() 
        {
            //移動する
            playerRb.MovePosition(transform.position + (desiredMove * Time.fixedDeltaTime));

            //飛行機から飛び降りて、既に着地したのなら
            if (landed) 
            {
                //以降の処理を行わない
                return;
            }

            //Playerが接地していなかったら
            if (!CheckGrounded()) 
            {
                //落下する
                transform.Translate(0, -GameData.instance.FallSpeed, 0);
            }
            //Playerが接地したら
            else 
            {
                //効果音を再生
                SoundManager.instance.PlaySE(SeName.LandingSE);

                //着地が完了した状態に切り替える
                landed = true;

                //物理演算を有効化
                playerRb.isKinematic = false;

                //Rigidbodyによる重力を有効化
                playerRb.useGravity = true;
            }
        }

        /// <summary>
        /// Playerが転倒しているかどうか調べる
        /// </summary>
        /// <returns>Playerが転倒していたらtrue</returns>
        private bool CheckToppled() 
        {
            //角度が正常ならfalseを返す
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

            //trueを返す
            return true;
        }

		/// <summary>
		/// 受け取ったPlayerの状態を元に、アニメーションの再生を行う
		/// </summary>
		/// <param name="playerCondition">PlayerCondition</param>
		private void PlayAnimation(PlayerCondition playerCondition) 
        {
			//指定したアニメーション名のアニメーションを再生
			anim.Play(playerCondition.ToString());
		}

		/// <summary>
        /// Playerの行動を制御する
        /// </summary>
        /// <returns>Playerの状態</returns>
		private PlayerCondition MovePlayer() 
        {
            //Playerのキャラクターの向きをカメラの向きに合わせる
            playerCharacterTran.eulerAngles = new Vector3(0f, mainCameraTran.eulerAngles.y, 0f);

            //移動方向をPlayerの向きに合わせる
            desiredMove = (mainCameraTran.forward * moveDirection.z) + (mainCameraTran.right * moveDirection.x);

            //移動ベクトルの大きさが1.0より小さいなら
            if (desiredMove.magnitude < 1f) 
            {
                //移動ベクトルに0を代入
                desiredMove = Vector3.zero;//バグ防止
            }

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

            //Dを押されている間
            if (Input.GetAxis("Horizontal") > 0.0f) 
            {
                //進行方向ベクトルを設定
                moveDirection.x = Input.GetAxis("Horizontal") * speedX;

                //Playerの状態を返す
                return PlayerCondition.MoveRight;
            }
            //Aを押されている間
            else if (Input.GetAxis("Horizontal") < 0.0f)
            {
                //進行方向ベクトルを設定
                moveDirection.x = Input.GetAxis("Horizontal") * speedX;

                //Playerの状態を返す
                return PlayerCondition.MoveLeft;
            }

            //かがむキーが押されている間
            if (Input.GetKey(stoopKey)) 
            {
                //コライダーのセンターを設定
                boxCollider.center = new Vector3(0f, 0.25f, 0f);

                //コライダーの大きさを設定
                boxCollider.size = new Vector3(0.5f, 0.5f, 0.5f);

                //Playerの状態を返す
                return PlayerCondition.Stooping;
            }
            //かがむキーが押されていないなら
            else 
            {
                //コライダーのセンターを初期値に設定
                boxCollider.center = firstColliderCenter;

                //コライダーの大きさを初期値に設定
                boxCollider.size = firstColliderSize;
            }

            //Playerの状態を返す
            return PlayerCondition.Idle;
        }

        /// <summary>
        /// Playerの接地判定を行う
        /// </summary>
        /// <returns>Playerが接地していたらtru</returns>
        public bool CheckGrounded() 
        {
			//rayの初期位置と向き（姿勢）を設定
			var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

			//rayの探索距離（長さ）を設定
			var tolerance = 0.3f;

			//rayのヒット判定（bool型）を返す
			return Physics.Raycast(ray, tolerance);
		}

		/// <summary>
		/// アイテム関連の処理
		/// </summary>
		private void ControlItem() 
        {
            //何かキーを押されたら
            if (Input.anyKeyDown)
            {
                //アイテムの切り替えを行う
                ChangeItem(CheckKey());
            }

            //アイテム破棄キーが押されたら
            if (Input.GetKeyDown(discardKey)) 
            {
                //効果音を再生
                SoundManager.instance.PlaySE(SeName.DiscardItemSE);

                //アイテムを破棄する
                ItemManager.instance.DiscardItem(SelectedItemNo - 1);
            }
            //左クリックされている間
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                //アイテムを使用する
                ItemManager.instance.UseItem(ItemManager.instance.GetSelectedItemData(), playerHealth);
            }
            //右クリックされている間
            else if (Input.GetKey(KeyCode.Mouse1)) 
            {
                //選択しているアイテムがスナイパーではないなら
                if (ItemManager.instance.GetSelectedItemData().itemName != ItemDataSO.ItemName.Sniper)
                {
                    //ズームする
                    followZoom.m_MaxFOV = normalZoomFOV;
                    followZoom.m_MinFOV = 1.0f;
                }
                //選択しているアイテムがスナイパーなら
                else 
                {
                    //ズームする
                    followZoom.m_MaxFOV = ScopeZoomFOV;
                    followZoom.m_MinFOV = 1.0f;

                    //スコープを覗く
                    uiManager.PeekIntoTheScope();
                }
            }
            //右クリックが終ったら
            else if (Input.GetKeyUp(KeyCode.Mouse1)) 
            {
                //元のカメラの倍率に戻す
                followZoom.m_MaxFOV = 30.0f;
                followZoom.m_MinFOV = 30.0f;

                //スコープを覗くのをやめる
                uiManager.NotPeekIntoTheScope();
            }

            //右クリックされたら
            if (Input.GetKeyDown(KeyCode.Mouse1)) 
            {
                //Enemyが使えない武器なら
                if (!ItemManager.instance.GetSelectedItemData().enemyCanUse) 
                {
                    //以降の処理を行わない
                    return;
                }

                //効果音を再生
                SoundManager.instance.PlaySE(SeName.BePreparedSE);
            }

            //Playerの最も近くにあるアイテムとの距離が、アイテムを取得できないほど離れているか、アイテムが存在しなかったら
            if (ItemManager.instance.LengthToNearItem > getItemLength || ItemManager.instance.generatedItemDataList.Count == 0)
            {
                //メッセージを空にする
                uiManager.SetMessageText("", Color.black);

                //以下の処理を行わない
                return;
            }

            //許容オーバーかどうか調べる
            ItemManager.instance.CheckIsFull();

            //許容オーバーかつ、取得しようとしているアイテムが弾ではなかったら
            if (ItemManager.instance.IsFull && ItemManager.instance.generatedItemDataList[ItemManager.instance.NearItemNo].itemType != ItemDataSO.ItemType.Bullet) 
            {
                //メッセージを表示
                uiManager.SetMessageText("Tap 'X' To\nDiscard\nThe Item", Color.red);
            }
            //許容オーバーではないなら
            else
            {
                //メッセージを表示
                uiManager.SetMessageText("Tap 'Q' To\nGet The\nItem", Color.green);
            }

            //アイテム取得キーが押されたら
            if (Input.GetKeyDown(getItemKey)) 
            {
                //アイテムを取得する
                ItemManager.instance.GetItem(ItemManager.instance.NearItemNo, true, playerHealth);
            }
        }

		/// <summary>
		/// 押されたキーの情報を返す
		/// </summary>
		private KeyCode CheckKey() 
        {
			//1つずつキーの情報を取り出す
			foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) 
            {
				//foreach文で取得したキーと、押されたキーが同じなら
				if (Input.GetKeyDown(code)) 
                {
					//キーの情報を返す
					return code;
				}
			}

			//キーの情報を返す
			return KeyCode.None;
		}

		/// <summary>
        /// 使用アイテムを切り替える
        /// </summary>
        /// <param name="code">押されたキー</param>
		private void ChangeItem(KeyCode code)
        {
			//押されたキーに応じて使用しているアイテムの番号を設定
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

            //アイテムスロットの背景色を設定
            uiManager.SetItemSlotBackgroundColor(ItemManager.instance.SelectedItemNo, Color.red);

            //選択したアイテムがNoneなら
            if (ItemManager.instance.GetSelectedItemData().itemName == ItemDataSO.ItemName.None) 
            {
                //効果音を再生
                SoundManager.instance.PlaySE(SeName.NoneItemSE);
            }
            //選択したアイテムがNoneではなかったら
            else
            {
                //効果音を再生
                SoundManager.instance.PlaySE(SeName.SelectItemSE);
            }
        }
	}
}