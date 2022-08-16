using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;
using UnityEngine.UI;//UIを使用
using DG.Tweening;//DOTweenを使用
using UnityEngine.SceneManagement;//シーンのロードを使用

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
    private Text txtBulletCount;//残弾数テキスト

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
    private Transform canvasTran;//Canvasのtransform

    [SerializeField]
    private GameObject itemSlotSetPrefab;//アイテムスロットセットのプレファブ

    private List<Image> imgItemSlotList = new List<Image>();//アイテムスロットのイメージのリスト

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //アイテムスロットを生成
        GenerateItemSlots(5);
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //フレームレートを計算し、表示を更新する
        UpdateFpsText();
    }

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    public IEnumerator PlayGameStart()
    {
        //視界を白色に設定
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f,0.0f);

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

        //GameManagerからゲーム開始状態に切り替える
    }

    /// <summary>
    /// ゲームクリア演出を行う
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayGameClear()
    {
        //視界を白色に設定
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);

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

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ゲームオーバー演出を行う
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayGameOver()
    {
        //視界を黒色に設定
        eventHorizon.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        //1.0秒かけて視界を完全に暗くする
        eventHorizon.DOFade(1.0f, 1.0f);

        //視界が完全に暗転するまで待つ
        yield return new WaitForSeconds(1.0f);

        //3.0秒かけて「GameOver」を表示
        txtGameOver.DOText("GameOver",3.0f);

        //「GameOverの表示が終ったあと、さらに1.0秒間待つ
        yield return new WaitForSeconds(4.0f);

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// 視界を指定された時間だけ暗くする
    /// </summary>
    public IEnumerator SetEventHorizonBlack(float time)
    {
        //視界を黒色に設定
        eventHorizon.color = new Color(0.0f, 0.0f, 0.0f,0.0f);

        //1.0秒かけて視界を完全に暗くする
        eventHorizon.DOFade(1.0f, 1.0f);

        //引数で指定された時間だけ視界を暗いまま保つ
        yield return new WaitForSeconds(time);

        //0.5秒かけて視界を元に戻す
        eventHorizon.DOFade(0.0f, 0.5f);
    }

    /// <summary>
    /// 被弾した際の視界の処理
    /// </summary>
    public IEnumerator AttackEventHorizon()
    {
        //視界を赤色に設定
        eventHorizon.color = new Color(255.0f, 0.0f, 0.0f, 0.0f);

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
    public void UpdateHpSliderValue(float currentValue,float updateValue)
    {
        //0.5秒かけて体力用スライダーを更新する
        hpSlider.DOValue((currentValue + updateValue)/100.0f, 0.5f);
    }

    /// <summary>
    /// CanvasGroupの表示、非表示を切り替える
    /// </summary>
    /// <param name="set"></param>
    public void SetCanvasGroup(bool isSetting)
    {
        //引数を元に、CanvasGroupの透明度を設定
        canvasGroup.alpha = isSetting ? 1.0f : 0.0f;
    }

    /// <summary>
    /// 残弾数の表示の更新を行う
    /// </summary>
    public void UpdateTxtBulletCount(int bulletCount)
    {
        //引数を元に、残弾数のテキストを設定
       txtBulletCount.text=bulletCount.ToString();
    }

    /// <summary>
    /// フロート表示の生成を行う
    /// </summary>
    public IEnumerator GenerateFloatingMessage(string messageText,Color color)
    {
        //フロート表示を生成
        Text txtFloatingMessage = Instantiate(floatingMessagePrefab);

        //生成したフロート表示の親をCanvasに設定
        txtFloatingMessage.gameObject.transform.SetParent(canvasTran);

        //引数を元に、フロート表示のテキストを設定
        txtFloatingMessage.text = messageText;

        //引数を元に、フロート表示の色を設定
        txtFloatingMessage.color = color;

        //フロート表示の場所を初期位置を設定
        txtFloatingMessage.gameObject.transform.position= new Vector3(900.0f,200.0f,0);

        //フロート表示の大きさを初期化
        txtFloatingMessage.gameObject.transform.localScale = Vector3.one;

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
        //フレームレートを計算し、取得する
        float fps = 1f / Time.deltaTime;

        //表示を更新
        txtFps.text=fps.ToString("F0")+"fps";
    }

    /// <summary>
    /// 指定された数だけアイテムスロットを生成する
    /// </summary>
    /// <param name="generateNumber">アイテムスロットの数</param>
    public void GenerateItemSlots(int generateNumber)
    {
        //引数で指定された回数だけ生成処理を繰り返す
        for (int i = 0; i < generateNumber; i++)
        {
            //アイテムスロットを生成
            GameObject itemSlot = Instantiate(itemSlotSetPrefab);

            //生成したアイテムスロットの親をcanvasGroupに設定
            itemSlot.transform.SetParent(canvasGroup.transform);

            //生成したアイテムスロットの大きさを設定
            itemSlot.transform.localScale = Vector3.one;

            //生成したアイテムスロットの位置を設定
            itemSlot.transform.localPosition = new Vector3((-1 * (200 + (50 * (generateNumber - 5)))) + (100 * i), 0, 0);

            //生成したアイテムスロットの子オブジェクトのImageを取得
            if (itemSlot.transform.GetChild(2).TryGetComponent<Image>(out Image imgItem))//nullエラー回避
            {
                //取得したイメージを透明に設定
                imgItem.DOFade(0.0f, 0.01f);
            }

            //生成したアイテムスロットのイメージをリストに追加
            imgItemSlotList.Add(imgItem);

            //Playerが所持しているアイテムのリストの要素を、アイテムスロットの数だけ作る
            GameData.instance.playerItemList.Add(null);
        }
    }

    /// <summary>
    /// アイテムのSpriteを設定する
    /// </summary>
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
    public void SetMessageText(string text)
    {
        //引数を元に。メッセージのテキストを設定
        txtMessage.text=text;
    }
}
