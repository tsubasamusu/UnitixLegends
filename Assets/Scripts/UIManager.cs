using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;
using UnityEngine.UI;//UIを使用
using DG.Tweening;//DOTweenを使用
using UnityEngine.SceneManagement;//LOadScenを使用

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image eventHorizon;//視界

    [SerializeField]
    private Image logo;//ロゴ

    [SerializeField]
    private Sprite gameStart;//ゲームスタートロゴ

    [SerializeField]
    private Sprite gameClear;//ゲームクリアロゴ

    [SerializeField]
    private Text txtGameOver;//ゲームオーバーテキスト

    [SerializeField]
    private Text txtItemCount;//残弾数テキスト

    [SerializeField]
    private Text txtOtherCount;//他の数のテキスト

    [SerializeField]
    private Text txtFps;//FPSのテキスト

    [SerializeField]
    private Text txtMessage;//メッセージのテキスト

    [SerializeField]
    private Text floatingMessagePrefab;//フロート表示のプレファブ

    [SerializeField]
    private Slider hpSlider;//体力スライダー

    [SerializeField]
    private CanvasGroup canvasGroup;//CanvasGroup

    [SerializeField]
    private Transform itemSlot;//アイテムスロットの親

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    [SerializeField]
    private BulletManager bulletManager;//BulletManager

    [SerializeField]
    private ItemManager itemManager;//ItemManager

    [SerializeField]
    private PlayerHealth playerHealth;//PlayerHealth

    [SerializeField]
    private Transform floatingMessagesTran;//フロート表示の親

    [SerializeField]
    private Transform enemies;//全てのEnemyの親

    [SerializeField]
    private GameObject itemSlotSetPrefab;//アイテムスロットセットのプレファブ

    [SerializeField]
    private GameObject scope;//スコープ

    [HideInInspector]
    public List<Image> imgItemSlotList = new List<Image>();//アイテムスロットのイメージのリスト

    [HideInInspector]
    public List<Image> imgItemSlotBackgroundList= new List<Image>();//アイテムスロットの背景のイメージのリスト

    /// <summary>
    /// テキストの表示の更新を常に行う
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateText()
    {
        //無限に繰り返す
        while(true)
        {
            //フレームレートを計算し、表示を更新する
            UpdateFpsText();

            //残弾数の表示を更新する
            UpdateTxtBulletCount();

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// アイテムスロットの設定等を行う
    /// </summary>
    public void SetUpItemSlots()
    {
        //アイテムスロットを生成
        GenerateItemSlots(5);

        //一番右のアイテムスロットの背景を設定
        SetItemSlotBackgroundColor(1, Color.red);
    }

    /// <summary>
    /// 他の数の表示を更新
    /// </summary>
    /// <param name="enemyNumber">Enemyの数</param>
    public void UpdateTxtOtherCount(int enemyNumber)
    {
        txtOtherCount.text = (enemyNumber+1).ToString()+"Players\n"+GameData.instance.KillCount.ToString()+"Kills";
    }

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameStart()
    {
        //視界を白色に設定
        SetEventHorizonColor(Color.white);

        //視界の色をハッキリと表示
        eventHorizon.DOFade(1f, 0f);

        //ロゴをゲームスタートに設定
        logo.sprite = gameStart;

        //2.0秒かけてロゴを表示
        logo.DOFade(1.0f,2.0f);

        //ロゴの表示が終わるまで待つ
        yield return new WaitForSeconds(2.0f);

        //視界を1.0秒掛けて透明にする
        eventHorizon.DOFade(0.0f, 1.0f);

        //ロゴを1.0秒かけて消す
        logo.DOFade(0.0f, 1.0f);

        //視界とロゴの演出が終わるまで待つ
        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// ゲームクリア演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameClear()
    {
        //視界を白色に設定
        SetEventHorizonColor(Color.white);

        //視界の色をハッキリと表示
        eventHorizon.DOFade(1f, 0f);

        //ロゴをゲームクリアに設定
        logo.sprite = gameClear;

        //2.0秒かけてロゴを表示
        logo.DOFade(1.0f, 2.0f);

        //ロゴの表示が終わるまで待つ
        yield return new WaitForSeconds(2.0f);

        //ロゴを1.0秒かけて消す
        logo.DOFade(0.0f, 1.0f);

        //視界とロゴの演出が終わるまで待つ
        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// ゲームオーバー演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameOver()
    {
        //視界を黒色に設定
        SetEventHorizonColor(Color.black);

        //1.0秒かけて視界を完全に暗くする
        eventHorizon.DOFade(1.0f, 1.0f);

        //視界が完全に暗転するまで待つ
        yield return new WaitForSeconds(1.0f);

        //3.0秒かけて等速で「GameOver」を表示
        txtGameOver.DOText("GameOver",3.0f).SetEase(Ease.Linear);

        //「GameOverの表示が終ったあと、さらに1.0秒間待つ
        yield return new WaitForSeconds(4.0f);
    }

    /// <summary>
    /// 視界を指定された時間だけ暗くする
    /// </summary>
    /// <param name="time">暗くする時間</param>
    /// <returns>待ち時間</returns>
    public IEnumerator SetEventHorizonBlack(float time)
    {
        //視界を黒色に設定
        SetEventHorizonColor(Color.black);

        //1.0秒かけて視界を完全に暗くする
        eventHorizon.DOFade(1.0f, 1.0f);

        //引数で指定された時間だけ視界を暗いまま保つ
        yield return new WaitForSeconds(time);

        //0.5秒かけて視界を元に戻す
        eventHorizon.DOFade(0.0f, 0.5f);
    }

    /// <summary>
    /// 視界の色を設定する
    /// </summary>
    /// <param name="color">視界の色</param>
    public void SetEventHorizonColor(Color color)
    {
        //引数を元に、視界の色を設定
        eventHorizon.color=color;
    }

    /// <summary>
    /// 被弾した際の視界の処理
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator AttackEventHorizon()
    {
        //視界を赤色に設定
        SetEventHorizonColor(Color.red);

        //0.25秒かけて視界を少し赤くする
        eventHorizon.DOFade(0.5f, 0.25f);

        //視界が少し赤くなるまで待つ
        yield return new WaitForSeconds(0.25f);

        //0.25秒かけて視界を元に戻す
        eventHorizon.DOFade(0.0f, 0.25f);
    }

    /// <summary>
    /// 体力用スライダーを更新する
    /// </summary>
    /// <param name="currentValue">現在の体力</param>
    /// <param name="updateValue">変更する体力量</param>
    public void UpdateHpSliderValue(float currentValue,float updateValue)
    {
        //0.5秒かけて体力用スライダーを更新する
        hpSlider.DOValue((currentValue + updateValue)/100f, 0.5f);
    }

    /// <summary>
    /// CanvasGroupの表示、非表示を切り替える
    /// </summary>
    /// <param name="set">表示するならtrue</param>
    public void SetCanvasGroup(bool isSetting)
    {
        //引数を元に、CanvasGroupの透明度を設定
        canvasGroup.alpha = isSetting ? 1.0f : 0.0f;
    }

    /// <summary>
    /// アイテムの数の表示の更新を行う
    /// </summary>
    private void UpdateTxtBulletCount()
    {
        //選択しているアイテムが飛び道具なら
        if (itemManager.GetSelectedItemData().isMissile)
        {
            //選択されている飛び道具の残弾数をテキストに設定
            txtItemCount.text = bulletManager.GetBulletCount(itemManager.GetSelectedItemData().itemName).ToString();
        }
        //選択しているアイテムに回復効果があるなら
        else if(itemManager.GetSelectedItemData().restorativeValue>0)
        {
            //選択されている回復アイテムの所持数をテキストに設定
            txtItemCount.text = playerHealth.GetRecoveryItemCount(itemManager.GetSelectedItemData().itemName).ToString();
        }
        //選択しているアイテムが、飛び道具でも回復アイテムでもないなら
        else
        {
            //テキストを空にする
            txtItemCount.text = "";
        }
    }

    /// <summary>
    /// 全てのフロート表示を非表示にする
    /// </summary>
    public void SetFloatingMessagesNotActive()
    {
        //フロート表示の親を無効化
        floatingMessagesTran.gameObject.SetActive(false);
    }

    /// <summary>
    /// フロート表示を生成する
    /// </summary>
    /// <param name="messageText">表示したいテキスト</param>
    /// <param name="color">表示する色</param>
    /// <returns>待ち時間</returns>
    public IEnumerator GenerateFloatingMessage(string messageText,Color color)
    {
        //フロート表示を生成
        Text txtFloatingMessage = Instantiate(floatingMessagePrefab);

        //生成したフロート表示の親を設定
        txtFloatingMessage.gameObject.transform.SetParent(floatingMessagesTran);

        //引数を元に、フロート表示のテキストを設定
        txtFloatingMessage.text = messageText;

        //引数を元に、フロート表示の色を設定
        txtFloatingMessage.color = color;

        //フロート表示の場所を初期位置を設定
        txtFloatingMessage.gameObject.transform.localPosition = Vector3.zero;

        //フロート表示の大きさを初期化
        txtFloatingMessage.gameObject.transform.localScale = new Vector3(3f,3f,3f);

        //生成したフロート表示を3.0秒後に消す
        Destroy(txtFloatingMessage.gameObject, 3.0f);

        //フロート表示を2.0秒かけて、上に移動させる
        txtFloatingMessage.gameObject.transform.DOLocalMoveY(100.0f,2.0f);

        //フロート表示の移動が終わるまで待つ
        yield return new WaitForSeconds(2.0f);

        //フロート表示を1.0秒かけて非表示にする
        txtFloatingMessage.DOFade(0.0f, 1.0f);
    }

    /// <summary>
    /// フレームレートを計算し、表示を更新する
    /// </summary>
    private void UpdateFpsText()
    {
        //表示を更新
        txtFps.text=(1f/Time.deltaTime).ToString("F0");
    }

    /// <summary>
    /// 指定された数だけアイテムスロットを生成する
    /// </summary>
    /// <param name="generateNumber">アイテムスロットの数</param>
    private void GenerateItemSlots(int generateNumber)
    {
        //引数で指定された回数だけ生成処理を繰り返す
        for (int i = 0; i < generateNumber; i++)
        {
            //アイテムスロットを生成
            GameObject itemSlot = Instantiate(itemSlotSetPrefab);

            //生成したアイテムスロットの親をitemSlotに設定
            itemSlot.transform.SetParent(this.itemSlot);

            //生成したアイテムスロットの大きさを設定
            itemSlot.transform.localScale = Vector3.one;

            //生成したアイテムスロットの位置を設定
            itemSlot.transform.localPosition = new Vector3((-1 * (200 + (50 * (generateNumber - 5)))) + (100 * i), -100, 0);

            //生成したアイテムスロットの子オブジェクトのImageを取得
            if (itemSlot.transform.GetChild(2).TryGetComponent<Image>(out Image imgItem))//nullエラー回避
            {
                //生成したアイテムスロットのイメージをリストに追加
                imgItemSlotList.Add(imgItem);

                //取得したイメージを透明に設定
                imgItem.DOFade(0.0f, 0f);
            }

            //生成したアイテムスロットの子オブジェクト（背景）のImageを取得
            if (itemSlot.transform.GetChild(0).TryGetComponent<Image>(out Image imgBackGround))//nullエラー回避
            {
                //生成したアイテムスロットのイメージ（背景）をリストに追加
                imgItemSlotBackgroundList.Add(imgBackGround);

                //背景を半透明に設定
                imgBackGround.DOFade(0.3f, 0f);
            }

            //Playerが所持しているアイテムのリストの要素を、アイテムスロットの数だけ作る
            itemManager.playerItemList.Add(itemDataSO.itemDataList[0]);
        }

        //アイテムスロット全体の大きさを2倍にする
        itemSlot.localScale = new Vector3(2f, 2f, 2f);

        //アイテムスロット全体の位置を調整
        itemSlot.localPosition= new Vector3(0f,270f,0f);
    }

    /// <summary>
    /// アイテムのSpriteを設定する
    /// </summary>
    /// <param name="itemNo">アイテムの番号</param>
    /// <param name="itemSprite">アイテムのSprite</param>
    public void SetItemSprite(int itemNo,Sprite itemSprite)
    {
        //引数を元に、指定されたアイテムのSpriteを設定する
        imgItemSlotList[itemNo-1].sprite = itemSprite;

        //指定されたイメージを可視化
        imgItemSlotList[itemNo - 1].DOFade(1.0f, 0.25f);
    }

    /// <summary>
    /// メッセージのテキストを設定し、表示する
    /// </summary>
    /// <param name="text">文字</param>
    /// <param name="color">色</param>
    public void SetMessageText(string text,Color color)
    {
        //引数を元に、メッセージのテキストを設定
        txtMessage.text=text;

        //引数を元に、テキストの色を設定
        txtMessage.color=color;
    }

   
    /// <summary>
    /// メッセージの表示、非表示を切り替える
    /// </summary>
    /// <param name="isSetting">表示するならtrue</param>
    public void SetMessageActive(bool isSetting)
    {
        //引数を元に、メッセージの透明度を取得
        float value = isSetting ? 1f : 0f;

        //取得した透明度をメッセージに反映する
        txtMessage.DOFade(value, 0f);
    }

   /// <summary>
   /// 指定されたアイテムスロットの背景色を設定する
   /// </summary>
   /// <param name="itemslotNo">アイテムスロットの番号</param>
   /// <param name="color">色</param>
    public void SetItemSlotBackgroundColor(int itemslotNo,　Color color)
    {
        //アイテムスロットの背景のイメージのリストの要素数だけ繰り返す
        for(int i = 0; i < imgItemSlotBackgroundList.Count; i++)
        {
            //指定されたアイテムスロットの背景色を設定し、それ以外は黒色に設定する
            imgItemSlotBackgroundList[i].color =i == (itemslotNo - 1) ? color:Color.black;

            //背景を半透明にする
            imgItemSlotBackgroundList[i].DOFade(0.3f, 0f);
        }
    }

    /// <summary>
    /// スコープを覗く
    /// </summary>
    public void PeekIntoTheScope()
    {
        //CanvasGroupを非表示にする
        SetCanvasGroup(false);

        //スコープを有効に
        scope.gameObject.SetActive(true);
    }

    /// <summary>
    /// スコープを覗くのをやめる
    /// </summary>
    public void NotPeekIntoTheScope()
    {
        //CanvasGroupを表示する
        SetCanvasGroup(true);

        //スコープを無効に
        scope.gameObject.SetActive(false);
    }
}
