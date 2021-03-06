# 前置き

　新しくプログラム言語を覚えるとき、まず文法書を読んで文法を１つ１つ覚えようとする人が多いと思う。そういう方法でもいいけれど、オブジェクト指向プログラミング（＝OOP）はGUI と相性がよく理解もしやすいため、細かい文法を覚えるよりまずは目に見えて動くものを作る方が良いと思う(^^)/

　以下に示した流れでコードを書いていくと、細かい文法が分からない状態でコードができあがっていくから少々気持ち悪く感じると思う。けれど、1000行にも満たない小さいコードをいくら書いても文法や命令の本質的な意味は実感できないと思う。文法書を読み終わった人が、この命令の意味は分かるけど、これ役に立つの？という状態になるのがよくあるオチ。。(´・ω・`)

　ちょっと大きめのコードをマネしながら書いてみて、で、その後に文法書を読むのが楽ちんな方法だと思う(#^^#)
 
 　＊なお、これは共著で書いているため、本来の文とは少し表現が異なっている部分もありますがあしからず(>_<)

# C# のインストール

　以下の URL あたりを参考にしてね

https://qiita.com/grinpeaceman/items/b5a6082f94c9e4891613

# まずは動くものを作る

　ファイル -> 新規作成 -> プロジェクト で、C# の「Windows フォームアプリケーション」を作成(^^)v

　お試しなので、名前などは適当に決めてね☆

# とりあえず実行

　ビルドして、とりあえず実行（F5 キーで実行できると思うよ）

　ウィンドウが１枚開けばＯＫ。「✕」ボタンでウィンドウを閉じて終了！

# 動作したファイルの確認

　できあがるファイルで重要なのは下の３つだよ！namespace の後ろの TestProgram という名前は、作成時に指定した名前になっているはず！
 　
  現時点ではファイルは全部表示されてないから、ポチポチ操作して、（ア）、（イ）、（ウ）の３つのファイルを表示させよう！

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

(イ) Form1.cs（ソリューションエクスプローラで Form1.cs を右クリックして、「コードの表示」を選択するとでてくるよ）
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
　C# では、プログラムは何らかの namespace に入っている必要があるんだってばよ。ファイルが分かれてても namespace が同じなら同じプログラムとみなされるんや！


# 実行時の動作の流れ

* C# は、プログラム中にある `static void Main()` から実行が開始される、という仕様になっている。だから、(ア) の `static void Main()` からプログラムが開始されているんだよ(^^)v

* 以下の２つは、描画関連の内部スイッチの変更。詳細は気にしなくて良いと思う。今はスルーしてね！
```
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
```

* `Application.Run(new Form1());` について

   * `Form1` を生成（＝new）して、生成したものを `Application.Run()` に渡している。`Form1` はウィンドウを表していて、`Application.Run` はアプリの実行を行う命令であるため、この命令によりウィンドウが表示され、アプリの実行が開始される。

   * `Form1` が生成されるとき、(イ) の `public Form1()` が実行される。（この仕組については後で解説する。現段階では「何かを生成するときには、それに付随して実行されるものがある」という理解でＯＫ。）
   
   * (イ) の `Form1() { ... }` の中に、`InitializeComponent()` と書いてあるので、`InitializeComponent()` の実行に移る。`InitializeComponent()` の中身に関しては、後で分かるようになればよい。気にしたら負け。
<BR>

> **(イ) の `public partial class Form1 : Form { ... }` について**
>
>　public、partial、: Form については後で分かる。ここでは「class」について理解しておく。
>
>　`class A { ... }` とすると、`...` の部分が「A を構成する部品」という意味になるよ。「class」は、１つのまとまったものを表す。学校での「クラス」と同じ意味！
>
>　以下のように書くと、`Form1` というクラスの中に、`Form1()` という関数があるという意味になる。
> ```
> class Form1
> {
>	Form1()
>	{
>	}
> }
> ```

<BR>

> 「関数」という言葉について  
> 　数学での関数（xとかyとか）と同じ感覚。何かを実行して（計算して）、できたものを返す、という感覚でＯＫ。  
> `Form1()` という関数は、ウィンドウを作成して、できたウィンドウを返す、という動作をするんだよ。

<BR>

# テキストボックスを付けてみよう

　まずは(イ) のファイルの、`Form1()` にコードを追加する。何らかの部品を作る場合は、次のようにする。

* `「作成したい部品の class名」 「部品に付ける名前」;` で、部品の名前を決定する。

   * (例) `Size box_size;`　box_size は、`Size` というクラスの部品を表す名前。

* `「部品の名前」＝ new「class名」;` で、実際に部品を生成する。

   * (例) `box_size = new Size(100, 100);` で、横 100 pixel、縦 100 pixel の大きさの `Size` オブジェクト（＝もの）を作り、それを `box_size` に割り当てる。「横 100 pixel、縦 100 pixel」という情報を追加するために、() の中に情報を追加している。

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

   * `text_box.Multiline = true;` としておかないと、１行しか表示できないテキストボックスとなってしまう。

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

   * `this` は、`class Form1 { ... }` の中にあるので、`this` は `Form1` を指すことになる。

   * `this.Controls.Add(...)` は、`Form1` の `Controls`（＝テキストボックスやボタンなどのコントロールの集合）に、`...` を `Add` せよ、という意味。

   * `this.Controls.Add(text_box);` ＝ 「`Form1` の `Controls` に、`text_box` を `Add` する」

* この状態でビルドして実行すると、テキストボックスが張り付いたウィンドウが表示される。
　

# テキストボックスの位置を変えよう
　TextBox の位置を変える場合、Location を変更する。以下のようなコードを追加して実験。

　C# では、名前を決めて、new で生成して、それを渡したり操作したりする、という感覚が必要。位置を指定するためだけに、 **「位置という情報を作成する」** 必要がある。
  さっきのサイズ設定みたいな感じ。
```
Point box_pos;　　　　　　　　　// 名前を決める
box_pos = new Point(50, 50);　// new で生成する

text_box.Location = box_pos;　// 作った位置（Point）を渡す
```
または、
```
text_box.Location = new Point(50, 50);  // new で生成したものを、直接渡すこともできる
```

# ボタンを追加しよう
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
* テキストボックスに、メッセージを追加表示させたい場合は `AppendText()` という命令を使う。
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

---
# 02.md に続く
# Link
- [hexadecimal.md](#hexadecimal.md)
## hexadecimal.md
hexadecimal.md

