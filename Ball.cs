using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public float speed;				// ボールが1秒間に移動する距離（速度）
	public float maxSpeed;			// ボールが移動できる最大速度
	public Vector2 direction;		// ボールが移動する方向を表す Vector2（例：斜め = Vector2(1, 1)）
	public Rigidbody2D rig;			// ボールの Rigidbody2D コンポーネント
	public GameManager manager;		// GameManager
	public bool goingLeft;			// ボールが左に進んでいるとき true
	public bool goingDown;			// ボールが下に進んでいるとき true

	void Start ()
	{
		transform.position = Vector3.zero;		// ボールの位置を画面中央に設定
		direction = Vector2.down;				// ボールの進行方向を下向きに設定
		StartCoroutine("ResetBallWaiter");		// 1秒待ってから動かすために ResetBallWaiter コルーチンを開始
	}

	void Update ()
	{
		rig.linearVelocity = direction * speed * Time.deltaTime;	// 方向 × 速度 で Rigidbody の速度を設定

		if(transform.position.x > 5 && !goingLeft){				// 右端に到達し、右方向へ進んでいる場合
			direction = new Vector2(-direction.x, direction.y);	// x方向を反転させて跳ね返す
			goingLeft = true;									// 左方向へ進んでいる状態にする
		}
		if(transform.position.x < -5 && goingLeft){				// 左端に到達し、左方向へ進んでいる場合
			direction = new Vector2(-direction.x, direction.y);	// x方向を反転させて跳ね返す
			goingLeft = false;									// 右方向へ進んでいる状態にする
		}
		if(transform.position.y > 3 && !goingDown){				// 上端に到達し、上方向へ進んでいる場合
			direction = new Vector2(direction.x, -direction.y);	// y方向を反転させて跳ね返す
			goingDown = true;									// 下方向へ進んでいる状態にする
		}
		if(transform.position.y < -5){							// パドルより下に落ちた場合
			ResetBall();										// ボールをリセットする
		}
	}

	// ボールが方向を変えるときに呼ばれる（パドルやブロックに当たったとき）
	// target は衝突したオブジェクトの位置
	public void SetDirection (Vector3 target)
	{
		Vector2 dir = new Vector2();			// 新しい進行方向を格納する Vector2

		dir = transform.position - target;		// ボールの位置 − 衝突対象の位置 = 進行方向
		dir.Normalize();						// 大きさを1に正規化

		direction = dir;						// ボールの進行方向を設定

		speed += manager.ballSpeedIncrement;	// GameManager の値分、ボール速度を加算

		if(speed > maxSpeed)					// 最大速度を超えた場合
			speed = maxSpeed;					// 最大速度に制限する

		if(dir.x > 0)							// x方向が正（右）なら
			goingLeft = false;
		if(dir.x < 0)							// x方向が負（左）なら
			goingLeft = true;	
		if(dir.y > 0)							// y方向が正（上）なら
			goingDown = false;
		if(dir.y < 0)							// y方向が負（下）なら
			goingDown = true;
	}

	// ボールがパドルの下に落ちたときに呼ばれる
	public void ResetBall ()
	{
		transform.position = Vector3.zero;		// ボールを画面中央に戻す
		direction = Vector2.down;				// 進行方向を下向きに設定
		StartCoroutine("ResetBallWaiter");		// 1秒待ってから再スタート
		manager.LiveLost();						// GameManager のライフ減少処理を呼び出す
	}

	// ボールを1秒待機させてから動かすためのコルーチン
	IEnumerator ResetBallWaiter ()
	{
		speed = 0;								// 一旦速度を0にする
		yield return new WaitForSeconds(1.0f);	// 1秒待つ
		speed = 200;							// 速度を200に設定
	}
}
