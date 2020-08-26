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

　(1)-(3) をまとめて、以下のように書いても同じ。下の例では TestProgram という名前を付けているけど、名前を変えたければいつでも変更していい。ただし、名前は統一しておくこと。（昨日の例では、ファイルによってプログラムの名前が異なっていたからビルドに失敗した感じ。）
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

　で、実際に test という名前で、３つの数字を記憶させたりしたい場合は、new を使ってメモリを確保する。以下のどちらの書き方もＯＫ。
```
Test test;
test = new Test();  // () の意味は後ほど。
```
```
Test test = new Test();
```

