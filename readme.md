* ありは自分で考えるのが得意なように思えるから、大雑把な説明だけ。
* C# は簡単だけど、C の知識があることで逆に間違えやすいところがあるから注意、、、

# まずは動くものを作る
　今までのファイルは後で直すとして、新規プロジェクトを作成。

　ファイル -> 新規作成 -> プロジェクト で、C# の「Windows フォームアプリケーション」を作成

　できあがるファイルで、重要なのは下の３つ。

(1) Program.cs
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

　(1)-(3) をまとめて、以下のように書いても同じ。下の例では TestProgram という名前を付けているけど、名前を変えたければいつでも変更していい。ただし、名前は統一しておくこと。（昨日のコードでは、ファイルによってプログラムの名前が異なっていたからビルドに失敗した感じ。）
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

　しかし、上の例は、２つのエラーがあってコンパイルできない。（C と感覚が異なるところ、、、）

* エラーの理由その１
```
　C# では、「Test test;」と書くと、「test」は「Test」を指す名前として使うよ、という意味にしかならない。
```
　名前を決めただけで、数字を３つ格納するメモリ空間は用意されない、ということ。test という名前を決めただけで、p を保存するメモリが用意されていないため、「test.p = 1」が実行できない。

　で、test という名前で、実際に３つの数字を記憶させたりしたい場合は、new を使ってメモリを確保する。以下のどちらの書き方もＯＫ。
```
Test test;
test = new Test();  // () の意味は後ほど。
```
```
Test test = new Test();
```
（参考）だから、昨日のコードではエラーが出ていた。昨日のコードは以下のようになってた感じ。
```
TextBox text_box;
text_box.Text = "テスト";  // ここでエラーが出る。名前だけを決めて、text_box のテキストを保存するメモリなどが確保していないため。
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

　しかし、class は「自分の中に含まれているものを、外に見せない」という性質を持っているため、p とか q とか rとか Add() というのがあるのは見えているけど、test に Add() をして、と頼むために test.Add() としてもエラーになる。

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
　上のようにすると、Add() は見てもいいことになるから、test.Add() がエラーなく実行できるようになる。ただし、test.p = 1 というのはやっぱりエラーになる。test.p = 1 ができるようにするためには、以下のようにする。
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
```

# コンストラクタ
　class から変数を生み出すとき、コンストラクタが実行される。コンストラクタは、「クラスの名前()」という形で宣言される。
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
　上の例では、`Test test = new Test();` とすると、p に１、q に２、r に３が代入された test が生成されるはず。けれど、コンストラクタを public にしていないため、Test() というコンストラクタが見えてない状態になっている。だから、実際に利用するときには、以下のようにする。
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
		q = x;
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

　Form1.cs を見てみる。
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

2) Form1 というクラスを作る。
　class Form1 の先頭に public とついているから、Form1 という名前は TestProgram の外からも見える状態にある。
　他のプログラムを作るときに、TestProgram.Form1 として、TestProgram の中の Form1 を利用することができる。

3) new Form1 として Form1 を作成すると、コンストラクタ public Form1() が実行される。
　コンストラクタ Form1() が実行されると、InitializeComponent() が実行される。
　（InitializeComponent() は、Form1.Designer.cs のにあるから、ちょっと見てみてほしい）
```

　さて、Form1（＝ウィンドウのこと）を生成すると、Form1() が実行されるから、その中にコードを追加してみたい。
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
　C# は名前を決めて、その名前で実際に利用できるようにメモリを確保する。というのが基本。

　昨日も言ったけど、「this.」は省略可能。自分自身（ウィンドウのこと（＝Form））に、Controls を Add するよ、って書いた方が分かりやすいから、「this.」を書いてるだけ。

　上の例は、普通は省略して以下のように書く。
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
