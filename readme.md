* ありは自分で考えるのが得意なように思えるから、大まかな説明のみを。細かいところは脳内補完で、、
* C# は簡単だけど、C の知識があることで逆に間違えやすいところがあると思う。
* 以下のことを理解すれば、昨日のコードが動くように訂正できるようになると思う。
* 長い文章に見えるけど、ほとんどがサンプルコード。中身はスカスカなので気楽にどうぞ。

# まずは動くものを作る
　今までのファイルは後で直すとして、新規プロジェクトを作成。

　ファイル -> 新規作成 -> プロジェクト で、C# の「Windows フォームアプリケーション」を作成

　できあがるファイルで、重要なのは下の３つ。

(1) Program.cs
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

(2) Form1.cs
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

(3) Form1.Designers.cs 
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

　上記の例で、test が Test の変数とすると、test.Add(); とすると、test.r = test.p + test.q が実行されるはず。

　しかし、class は **「自分の中に含まれているものを、外に見せない」** という性質を持っているため、p とか q とか rとか Add() というのがあるのは見えているけど、test に Add() をさせるために test.Add() としてもエラーになる。

　上のままでは、test に含まれている Add() を見せないことにしているので、test.Add() がエラーになる感じ。

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

test.p = 1;  // エラーなく実行できる
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
　上の例では、`Test test = new Test();` とすると、p に１、q に２、r に３が代入された test が生成される。けれど、コンストラクタを public にしていないため、Test() というコンストラクタが見えてない状態になっている。だから、実際に利用するときには、以下のようにする。
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

# テキストボックスを付けてみる

　Form1.cs を見てみる。（Form とは「ウィンドウ」のこと）
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
　「partial」と「: Form」は、ちょっと無視して。。。

　今までの知識で、以下のことが分かる。
```
1) TestProgram という名前のプログラムを作る。

2) Form1 というクラスが設計されている。
　class Form1 の先頭に public とついているから、Form1 という名前は TestProgram の外からも見える状態にある。
　他のプログラムを作るときに、TestProgram.Form1 として、TestProgram の中の Form1 を利用することができる。

3) Form1 を作成するとき、コンストラクタ public Form1() が実行される。
　コンストラクタ Form1() が実行されると、InitializeComponent() が実行される。
　（InitializeComponent() は、Form1.Designer.cs のにあるから、ちょっと見てみてほしい）
```

　Form1 のコンストラクタにコードを追加してみる。
```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			Size box_size;
			box_size = new Size(100, 100);

			TextBox text_box;
			text_box = new TextBox();
			text_box.Size = box_size;
			text_box.Multiline = true;

			this.Controls.Add(text_box);
		}
	}
}
```
* 上のようにすると、Form1（＝ウィンドウ）が作られるときに、text_box などが追加で作成される。

* C# は名前を決めて、その名前で実際に利用できるようにメモリを確保する。というのが基本。

* 昨日言ったことけど、「this.」は省略可能。自分自身（＝Form1）の Controls に text_box を Add する、って書いた方が分かりやすいから、「this.」を書いてるだけ。

* C# はプラモデルみたいな感じ。部品を作って、それに命令を出す、っていうのを繰り返す。


　上の例は、省略して以下のように書ける。
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

			this.Controls.Add(text_box);
		}
	}
}
```
　この状態でビルドして実行すると、テキストボックスが張り付いたウィンドウが表示される。
　
* 実験のために、text_box.Multiline = true; の１行を削除したものも実行してみて。Multiline を指定しないと、テキストボックスは１行しか入力できないものに変わる。

# テキストボックスの位置を変える
　TextBox の位置を変える場合、Location を変更する。以下のようなコードを追加して実験。
```
Point box_pos;
box_pos = new Point(50, 50);

text_box.Location = box_pos;
```
または、
```
text_box.Location = new Point(50, 50);
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
* Button クラスで作成されたオブジェクト（＝「もの」っていう意味）は、ボタンが押されると Click に指定されたメソッド（＝関数）を呼び出す。
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

# ボタンを押したら、テキストボックスにメッセージを表示する
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

* エラーとなる理由
　C と同じで、変数などに付けた名前は {} の内側からしか見ることができないため。

　{} の外側に書かれた名前は見ることができるため、以下のようにすればよい。
```
namespace TestProgram
{
	public partial class Form1 : Form
	{
		// ここで、text_box という名前を作っておけば、
		// この名前は Form1() からも OnClick_Button() からも見ることができる。
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

# class の定義の書き方
　メソッドの外側でできるのは、「名前を決める」or「名前を決めて、メモリを確保する」だけ。以下のような書き方はエラー。
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

　class Form1 {} というのは、Form1 が **「部品として何を持っているか」** を列挙する構文のため。
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

　もし、text_box にメッセージを表示したい場合、class Form1 {} の中に、メッセージを表示するメソッドを書き加えていくことになる。

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

* まだ `「: Form」` の意味を説明してないけど、partial で class を分割する場合、「: Form」はどこか１ヶ所に書いてあれば良い、ということになっている。また、複数に書いても良い。

　以下は、全部同じ意味となる。
```
class Form1 : Form
{
	TextBox text_box = new TextBox();
	Button btn = new Button();
}
```
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
　当然ながら上のように何度も細かい区切りを書くのが面倒なので、using という構文を用いる。
```
using System.Windows.Forms;

Button btn = new Button();
TextBox text_box = new TextBox();
```
　using を書いておくと、Button などの前に「System.Windows.Forms」という言葉が自動的に補完されるようになる、ということ。

　Form1.Designer.cs では using が使われていないため、Button などを作るときに長い名前になっている、ということを知っておけば良いと思う。


# 最後に１つ
　以上のことが分かれば、昨日のコードは動作するように書き直せると思う。ただ、イベントハンドラの書き方が２通りあるため、混乱するかも。以下の２つの書き方は同じもの、って知っておいてほしい。詳しい意味については、また後ほど、、、

* (1)
```
class Form1 : Form
{
	Button btn = new Button()

	public Form1()
	{
		btn.Click += OnClick_Button;
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
		btn.Click += new System.EventHandler(OnClick_Button);
	}

	void OnClick_Button(object sender, EventArgs e)
	{
	}
}
```

　(1) は、C# 2.0 以降の書き方。(2) は C# 1.1 までの古い書き方。今は新しい書き方である (1) で書けばよい。

* (2) の書き方を紹介した理由

　C# コンパイラに付属しているフォーム設計画面（ボタンをドラッグ＆ドロップで貼り付けたりするやつ）を利用してフォームを作成すると、イベントハンドラが (2) の書き方で生成される。

　new System.EventHandler() を見て、なんだこれは？？とならないでもらえればＯＫ。

　new System.EventHandler() は書かなくても正しく動作する、っていうことを知っておいてもらえればＯＫ。
