using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour 
{
	public GameManager manager;		// ゲーム全体を管理する GameManager

	// 他のオブジェクトがこのオブジェクトの BoxCollider2D（Trigger）に入ったときに呼ばれる
	// 引数 col は、このオブジェクトと衝突した Collider2D オブジェクト
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.tag == "Ball"){												// 衝突したオブジェクトのタグが「Ball」かどうか
			manager.score++;															// GameManager クラスのスコアを 1 増やす
			col.gameObject.GetComponent<Ball>().SetDirection(transform.position);		// Ball コンポーネントを取得し、ブロックの位置を渡して SetDirection() を呼び出す
			manager.bricks.Remove(gameObject);											// GameManager の bricks リストからこのブロックを削除する

			if(manager.bricks.Count == 0)												// bricks リストにブロックが残っていないか？
				manager.WinGame();														// GameManager の WinGame() 関数を呼び出す

			Destroy(gameObject);														// このブロックを破壊する
		}
	}
}
