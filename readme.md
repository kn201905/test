
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

   * 新規作成命令が `new`
      * `new Form1()` で、Form1 を新規作成する。Form1 の後ろの () は、作成するときに追加の指示を出したい時に利用する。今は追加の指示は出さないので () の中身は空。
   * フォーム ≒ アプリケーションウィンドウ
   * Form1 という名前を変えたければ、(イ)、(ウ) の Form1 と書いてあるところを書き換えればＯＫ。

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
　
---
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

			TextBox text_box;
			text_box = new TextBox();
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
			//「text_box が見つからない」というエラーになる
		}
	}
}
```

* エラーとなる理由
　C と同じで、変数などに付けた名前は { } の内側からしか見ることができないため。

　{ } の外側に書かれた名前は見ることができるため、以下のようにすればよい。
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


# namespace（名前空間）、class（型）
* C# は、とにかく名前をつけて、名前のあるものに対して命令を下していく感じ。プログラム自体にも名前を付けなければならい。(1)-(3) に共通してある namespace のところにプログラムの名前が書いてある。

* (1)-(3) をまとめて、以下のように書いても同じ。下の例では TestProgram という名前を付けているけど、名前を変えたければいつでも変更していい。ただし、名前は統一しておくこと。（昨日のコードでは、ファイルによってプログラムの名前が異なっていたからビルドに失敗した感じ。）
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

	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
	}

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

* C# のプログラムは、全て部品ごとに分割されて設計される。C のように独立した関数はない。関数は、何かの部品の中に存在するという形になる。

　以下の例では、p, q, r という３つの整数を持つ class（＝型）をつくる。
```
class Test
{
	int p;
	int q;
	int r;
}
```

　で、Test の型を持つ変数 test を作って、p に 1 を代入してみる。
```
class Test
{
	int p;
	int q;
	int r;
}

Test test;
test.p = 1;
```

　考え方は上の例で正しいけれど、エラーが２つあってコンパイルできない。（C と感覚が異なるところ、、、）

* エラーの理由その１
```
　C# では、「Test test;」と書くと、「test」は「Test」を指す名前として使うよ、という意味にしかならない。
```
　「名前を決めただけ」で、数字を３つ記憶するメモリ空間は確保されない（なんでやねん、って感じだけど、後々この考え方も便利となる）。test という名前を決めただけで、p を保存するメモリがないため、「test.p = 1」が実行できない。

　で、test という名前で、実際に３つの数字を記憶させたりしたい場合は、new を使ってメモリを確保する。以下のどちらの書き方もＯＫ。
```
Test test;
test = new Test();  // () の意味は後ほど。
```
または、
```
Test test = new Test();
```
（参考）昨日のコードは以下のような感じになってたから、エラーが出てた。
```
TextBox text_box;
text_box.Text = "テスト";  // ここでエラーが出る。名前だけ決めて、"テスト"という文字を保存するメモリが確保されていないため。
```
以下のようにすれば、エラーにならない。
```
TextBox text_box = new TextBox();
text_box.Text = "テスト";
```


* エラーの理由その２

　class は、自分に所属している変数を扱う関数（＝メソッド）を持つことができる。
```
class Test
{
	int p;
	int q;
	int r;

	void Add()
	{
		r = p + q;
	}
}
```

　上記の例で、 
```
Test test = new Test();
test.Add();
```
 とすると、test.r = test.p + test.q が実行されるはず。

　しかし、class は **「自分の中に含まれているものを、外に見せない」** という性質を持っているため、p とか q とか rとか Add() というのがあるのは見えているけど、test に Add() をさせるために test.Add() としてもエラーになる。

　上のままでは、test に含まれている Add() を見せないことにしているので、`test.Add()` がエラーになる感じ。

　で、外から見てもいいよ、と許可を与える場合、public というキーワードを利用する。

```
class Test
{
	int p;
	int q;
	int r;

	public void Add()
	{
		r = p + q;
	}
}

Test test = new Test();
test.Add();　　// これはＯＫ
```
　上のようにすると、Add() は見てもいいことになるから、test.Add() がエラーなく実行できるようになる。ただし、test.p = 1 というのはやっぱりエラーになる。以下のようにすれば、test.p = 1 が実行できるようになる。
```
class Test
{
	public int p;
	int q;
	int r;

	public void Add()
	{
		r = p + q;
	}
}

Test test = new Test();
test.p = 1;　　// エラーなく実行可能
test.q = 2;　　// これはエラー
```

# コンストラクタ
　class から変数を生み出すとき、コンストラクタが実行される。コンストラクタは、 **「クラスの名前()」** という形で宣言される。
```
class Test
{
	Test()  // <- これがコンストラクタ
	{
	}

	public int p;
	int q;
	int r;

	public void Add()
	{
		r = p + q;
	}
}
```
　上の例では、コンストラクタの中身が空であるから、コンストラクタを書いても書かなくても同じ。
```
class Test
{
	Test()
	{
		p = 1;
		q = 2;
		r = 3;
	}

	public int p;
	int q;
	int r;

	public void Add()
	{
		r = p + q;
	}
}
```
　上の例では、`Test test = new Test();` とすると、p に１、q に２、r に３が代入された test が生成される。けれど、コンストラクタを public にしていないため、Test() というコンストラクタを外に見せない状態になっている。だから、実際に利用するときには、以下のようにする。
```
class Test
{
	public Test()
	{
		p = 1;
		q = 2;
		r = 3;
	}

	public int p;
	int q;
	int r;

	public void Add()
	{
		r = p + q;
	}
}

Test test = new Test();
```

　コンストラクタには、以下のように引数を与えることができる。
```
class Test
{
	public Test()
	{
		p = 1;
		q = 2;
		r = 3;
	}

	public Test(int x)
	{
		p = x;
		q = 2;
		r = 3;
	}

	public Test(int y, int z)
	{
		p = 1;
		q = y;
		r = z;
	}

	public int p;
	int q;
	int r;

	public void Add()
	{
		r = p + q;
	}
}
```

　この場合、`Test test = new Test(10, 11)` とすると、p = 1, q = 10, r = 11 と設定された test が出来上がる、という感じ。

# class の定義の書き方
　class の宣言でできるのは、「名前を決める」or「名前を決めて、メモリを確保する」だけ。以下のような書き方はエラー。
```
エラーとなる書き方

public partial class Form1 : Form
{
	TextBox text_box = new TextBox();
	text_box.Size = new Size(100, 100);
	text_box.Multiline = true;
	this.Controls.Add(text_box);

	public Form1()
	{
		InitializeComponent();
	}
}
```

　class Form1 { } というのは、Form1 が **「部品として何を持っているか」** を列挙する構文のため。
```
public partial class Form1 : Form
{
	TextBox text_box = new TextBox();
	Button btn = new Button();

	public Form1()
	{
		InitializeComponent();

		text_box.Size = new Size(100, 100);
		text_box.Multiline = true;
		this.Controls.Add(text_box);

		btn.Text = "ボタン";
		this.Controls.Add(btn);
	}
}
```
　上のようにすると、Form1 は、以下の **「３つの部品を持つ」** という意味になる。

* text_box という名前の TextBox
* btn という名前の Button 
* 引数なしのコンストラクタ（Form1() のこと）

<BR>

# partial の意味
　class を設計する際、多くの部品が必要になることがある。その場合、partial を付けて class の設計を分割することができる。

　以下の２つは同じ意味。
```
public partial class Form1 : Form
{
	TextBox text_box = new TextBox();
	Button btn = new Button();

	public Form1()
	{
		InitializeComponent();

		text_box.Size = new Size(100, 100);
		text_box.Multiline = true;
		this.Controls.Add(text_box);

		btn.Text = "ボタン";
		this.Controls.Add(btn);
	}
}
```
or
```
public partial class Form1 : Form
{
	TextBox text_box = new TextBox();

	public Form1()
	{
		InitializeComponent();

		text_box.Size = new Size(100, 100);
		text_box.Multiline = true;
		this.Controls.Add(text_box);

		btn.Text = "ボタン";
		this.Controls.Add(btn);
	}
}

public partial class Form1
{
	Button btn = new Button();  // btn の定義だけを、別に書くこともできる。それだけの話
}
```
<BR>

* まだ `「: Form」` の意味を説明してないけど、partial で class を分割する場合、「: Form」はどこか１ヶ所に書いてあれば良い、ということになっている。また、複数に書いても良い。

　以下は、全部同じ意味となる。
```
class Form1 : Form
{
	TextBox text_box = new TextBox();
	Button btn = new Button();
}
```
or
```
pattial class Form1 : Form
{
	TextBox text_box = new TextBox();
}

pattial class Form1
{
	Button btn = new Button();
}
```
or
```
pattial class Form1
{
	TextBox text_box = new TextBox();
}

pattial class Form1 : Form
{
	Button btn = new Button();
}
```
or
```
pattial class Form1 : Form
{
	TextBox text_box = new TextBox();
}

pattial class Form1 : Form
{
	Button btn = new Button();
}
```

# using
　C# には、いろいろな部品が利用できるように準備されている。ウィンドウとか、ボタンとか、テキストボックスとか。。

　それで、頭の中を整理しやすいように、名前空間（最初に紹介した namespace）で細かく区切りを入れている。

　実は、今まで使っていた Button や TextBox は、System の中の Windows の中の Forms という区切りの中にある。

　Button や TextBox を作りたい場合、その細かい区切りの中にある Button や TextBox を作るよ、と指示をするために、正確に書くと以下のようになる。
```
System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
System.Windows.Forms.TextBox text_box = new System.Windows.Forms.TextBox();
```
　当然ながら上のように何度も細かい区切りを書くのが面倒なので、using という命令を利用する。
```
using System.Windows.Forms;

Button btn = new Button();
TextBox text_box = new TextBox();
```
　using を書いておくと、Button などの前に「System.Windows.Forms」という言葉が自動的に補完されるようになる。

　Form1.Designer.cs では using が使われていないため、Button などを作るときに長い名前になっている、ということを知っておけば良いと思う。


# 最後に１つ
　以上のことが分かれば、昨日のコードは動作するように書き直せると思う。ただ、イベントハンドラの書き方が２通りあるため、混乱するかも。以下の２つの書き方は同じもの、って知っておけばＯＫ（★ の部分が異なるだけ）。詳しい意味については、また後ほど、、、

* (1)
```
class Form1 : Form
{
	Button btn = new Button()

	public Form1()
	{
		btn.Click += OnClick_Button;　　// (★)
	}

	void OnClick_Button(object sender, EventArgs e)
	{
	}
}
```
* (2)
```
class Form1 : Form
{
	Button btn = new Button()

	public Form1()
	{
		btn.Click += new System.EventHandler(OnClick_Button);　　// ((★)
	}

	void OnClick_Button(object sender, EventArgs e)
	{
	}
}
```

　(1) は、C# 2.0 以降の書き方。(2) は C# 1.1 までの古い書き方。今は新しい書き方である (1) で書けばよい。

<BR>

* (2) の書き方を紹介した理由

　C# コンパイラに付属しているフォームデザイナ（ボタンをドラッグ＆ドロップで貼り付けたりするやつ）を利用してフォームを作成すると、イベントハンドラが (2) の書き方で生成される。

　new System.EventHandler() を見て、なんだこれは？？とならないでもらえればＯＫ。

　new System.EventHandler() は書かなくても正しく動作する、っていうことを知っておいてもらえればＯＫ。
