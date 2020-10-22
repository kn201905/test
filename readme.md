
# C# のインストール

　以下の URL あたりを参考に。

https://qiita.com/grinpeaceman/items/b5a6082f94c9e4891613

# まずは動くものを作る

　ファイル -> 新規作成 -> プロジェクト で、C# の「Windows フォームアプリケーション」を作成。

　お試しなので、名前などは適当に。

# とりあえず実行

　ビルドして、とりあえず実行（F5 キーで実行できると思う）

　ウィンドウが１枚開けばＯＫ。「✕」ボタンでウィンドウを閉じて終了。

# 動作したファイルの確認

　できあがるファイルで重要なのは下の３つ。namespace の後ろの TestProgram という名前は、作成時に指定した名前になっていると思う。

(ア) Program.cs
```
namespace TestProgram
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
```

(イ) Form1.cs
```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
	}
}
```

(ウ) Form1.Designers.cs（ソリューションエクスプローラで、▶ マークをいろいろ押してみると出てくると思う）
```
namespace TestProgram
{
	partial class Form1
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Text = "Form1";
		}

		#endregion
	}
}

```

# namespace について
　C# では、プログラムは何らかの namespace に入っている必要がある。ファイルが分かれてても namespace が同じなら同じプログラムとみなされる。


# 実行時の動作の流れ

* (ア) の static void Main() からプログラムが開始される。

* 以下の２つは、描画関連の内部スイッチの変更。詳細は気にしなくて良いと思う。
```
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
```

* `Application.Run(new Form1());`  の `new Form1()` について

   * `new Form1()` により、`Form1` というオブジェクト（＝もの）が作成される。今の場合、`Form1` はウィンドウを表しているので、この命令によりウィンドウが表示される。

   * フォーム ≒ アプリケーションウィンドウ

   * `Form1()` の () は、new を実行するときに追加で指示を出したい場合に利用する。

* `new Form1()` について

   * `new A()` が実行されると、プログラムのどこかに書いてある `A()` が実行される。
   * `new Form1()` により、(イ) の `public Form1()` が実行される。（public の意味は後で）
   * `Form1()` の中に、`InitializeComponent()` と書いてあるので、`InitializeComponent()` の実行に移る。
   * `InitializeComponent()` の中身に関しては、後で分かるようになればよい。

> **(イ) の `public partial class Form1 : Form { ... }` について**
>
>　public、partial、: Form については後で分かる。ここでは「class」について理解しておく。
>
>　`class A { ... }` とすると、`...` の部分が「A を構成する部品」という意味になる。「class」は、１つのまとまったものを表す。学校での「クラス」と同じ意味。
>
>　`class Form1 { Form1() { ... } }` で、Form1 というものには「`Form1()` という部品がある」と分かる。
>
>　`new Form1()` を実行すると、コンピュータは Form1 の class の内部を調べて、その中にある `Form1()` を実行する、という流れになる。

* `Application.Run( ... )` とすると、`...` の部分のものが実行機能を持つ場合、実行が開始される。Form1 は実行機能を持つ class なので、`new Form1()` で作成されたものの実行が開始される。

# テキストボックスを付けてみる

　(イ) のファイルの、`Form1()` にコードを追加してみる。何らかの部品を作る場合は、次のようにする。

* `「作成したい部品の class名」 「部品に付ける名前」;` で、部品の名前を決定する。

   * (例) `Size box_size;`　box_size は、`Size` というクラスの部品を表す名前。

* `「部品の名前」＝ new「class名」` で、実際に部品を生成する。

   * (例) `box_size = new Size(100, 100);` で、横 100 pixel、縦 100 pixel の大きさの `Size` オブジェクト（＝もの）を作り、それを `box_size` に割り当てる。今回は、「横 100 pixel、縦 100 pixel」という情報を追加するために、() の中に情報を追加している。

```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			Size box_size;　　　　　　　　　　// 名前を付ける
			box_size = new Size(100, 100);　// new で生成する

			TextBox text_box;　　　　　　// 名前を付ける
			text_box = new TextBox();　　// new で生成する
			text_box.Size = box_size;
			text_box.Multiline = true;

			this.Controls.Add(text_box);
		}
	}
}
```

* `TextBox` は、`Size` や `Multiline` などのプロパティ（＝属性値）を持つので、生成した後にそれらのプロパティを設定している。

   * ｀text_box.Multiline = true;｀ としておかないと、１行しか表示できないテキストボックスとなってしまう。

* 「名前を付ける」のと、「new で生成する」のを２行に分けて書くのが面倒くさい場合は、まとめることができる。以下の (a) と (b) は同じ意味になる。

(a)
```
TextBox text_box;
text_box = new TextBox();
```
(b)
```
TextBox text_box = new TextBox();
```

* `this.Controls.Add(text_box);` について

   * this は、`class Form1 { ... }` の中にあるので、this というのは Form1 を指すことになる。
   * `this.Controls.Add(...)` は、Form1 の Controls（＝テキストボックスやボタンなどのコントロールの集合）に、`...` を Add せよ、という意味。
   * Form1（=this）の Controls に text_box を Add せよ、という意味になる。

* この状態でビルドして実行すると、テキストボックスが張り付いたウィンドウが表示される。
　

# テキストボックスの位置を変える
　TextBox の位置を変える場合、Location を変更する。以下のようなコードを追加して実験。

　C# では、名前を決めて、new で生成して、それを渡したり操作したりする、という感覚が必要。位置を指定するためだけに、 **「位置という情報を作成する」** 必要がある。
```
Point box_pos;　　　　　　　　　// 名前を決める
box_pos = new Point(50, 50);　// new で生成する

text_box.Location = box_pos;　// 作った位置（Point）を渡す
```
または、
```
text_box.Location = new Point(50, 50);  // new で生成したものを、直接渡すこともできる
```

# ボタンを追加する
```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			TextBox text_box = new TextBox();
			text_box.Size = new Size(100, 100);
			text_box.Multiline = true;
			text_box.Location = new Point(50, 50);

			this.Controls.Add(text_box);


			Button btn = new Button();
			btn.Text = "ボタン";

			this.Controls.Add(btn);
		}
	}
}
```

# ボタンを押したら、ウィンドウが閉じるようにする
* ウィンドウを閉じるときは、Form に Close() を命令すればよい。
* Button クラスで作成されたオブジェクト（＝部品）は、ボタンが押されると Click に指定したメソッド（＝関数）を呼び出す。
```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			TextBox text_box;
			text_box = new TextBox();
			text_box.Size = new Size(100, 100);
			text_box.Multiline = true;
			text_box.Location = new Point(50, 50);

			this.Controls.Add(text_box);


			Button btn = new Button();
			btn.Text = "ボタン";
			btn.Click += OnClick_Button;

			this.Controls.Add(btn);
		}

		// object sender, EventArgs e の意味は後で。
		// 理解できるようになるのは、かなり後になるかも、、
		void OnClick_Button(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
```

# ボタンを押したら、テキストボックスにメッセージが表示されるようにする
* テキストボックスに、メッセージを追加表示させたい場合は AppendText() という命令を使う。
* 考え方としては以下のようになるけれど、以下のままではエラーとなる。
```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			TextBox text_box;
			text_box = new TextBox();
			text_box.Size = new Size(100, 100);
			text_box.Multiline = true;
			text_box.Location = new Point(50, 50);

			this.Controls.Add(text_box);


			Button btn = new Button();
			btn.Text = "ボタン";
			btn.Click += OnClick_Button;

			this.Controls.Add(btn);
		}

		void OnClick_Button(object sender, EventArgs e)
		{
			text_box.AppendText("Hello.\r\n");  // \r\n は改行の意味
		}
	}
}
```

上記のコードでは、`text_box.AppendText("Hello.\r\n");` を実行しようとするときに、「`text_box` が指しているものを見つけられない」というエラーが発生する。


* エラーとなる理由

　変数などに付けた名前の有効範囲は { } の内側に制限されるため。

* エラーの解消法

　名前の有効範囲を広めるために、{ } の外側で名前を宣言する。

```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		// ここで、text_box という名前を作っておけば、
		// この名前は Form1() からも OnClick_Button() からも共通して見ることができる。
		TextBox text_box;

		public Form1()
		{
			InitializeComponent();

			text_box = new TextBox();
			text_box.Size = new Size(100, 100);
			text_box.Multiline = true;
			text_box.Location = new Point(50, 50);

			this.Controls.Add(text_box);


			Button btn = new Button();
			btn.Text = "ボタン";
			btn.Click += OnClick_Button;

			this.Controls.Add(btn);
		}

		void OnClick_Button(object sender, EventArgs e)
		{
			text_box.AppendText("Hello.\r\n");
		}
	}
}
```
　名前を作ると同時に new をすることもできるから、以下のように書いてもよい。（`new TextBox();` の場所が変わっただけ）

```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		TextBox text_box = new TextBox();

		public Form1()
		{
			InitializeComponent();

			text_box.Size = new Size(100, 100);
			text_box.Multiline = true;
			text_box.Location = new Point(50, 50);

			this.Controls.Add(text_box);


			Button btn = new Button();
			btn.Text = "ボタン";
			btn.Click += OnClick_Button;

			this.Controls.Add(btn);
		}

		void OnClick_Button(object sender, EventArgs e)
		{
			text_box.AppendText("Hello.\r\n");
		}
	}
}
```
