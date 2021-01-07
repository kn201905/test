# js の非同期機構

## 参考１

### (ア) Promise による実装（Chrome では Promise を 2014年に実装。V8 3.2）

```
new Promise(f => {  // 敢えて f と書いている（普通は resolve と書くところ）
	console.log('--- A ---');
	WaitForClient(f);
	console.log('--- B ---');
})
.then((msg) => {
	console.log('--- クライアント接続 ---');
	console.log(msg);
});

console.log('--- C ---');

// --------------

function WaitForClient(f_cb) {
	setTimeout(() => {
		f_cb('connected');
	}, 2000);
}
```

### (イ) async による実装（Chrome では async を 2016年に実装。V8 5.5）
```
(async () => {
	console.log('--- A ---');
	const msg = await WaitForClientAsync();
	console.log('--- B ---');
	console.log('--- クライアント接続 ---');
	console.log(msg);
})();

console.log('--- C ---');

// --------------

async function WaitForClientAsync() {
	await new Promise(resolve => {
		setTimeout(() => resolve(null), 2000);
	});
	return 'connected';
}
```

### (ウ) 昔ながらの実装（いつの時代でも使えた方法）
```
console.log('--- X ---');
StartListening(OnConnect);
console.log('--- Y ---');

function OnConnect(msg) {
	console.log('--- クライアント接続 ---');
	console.log(msg);
};

// --------------

function StartListening(f_cb) {
	setTimeout(() => {
		f_cb('connected');
	}, 2000);
}
```

## 参考２
非同期機構を、ステートマシンによって実装するか、キューによって実装するかで挙動が変わる例。

しかし、最近はキューによる仕組みもステートマシンと呼ぶのかも。MS公式のページ  
https://docs.microsoft.com/ja-jp/dotnet/csharp/async  
では、現在の実装もステートマシンと呼んでいるような気がする。技術用語も、ときとしてフワフワしてるように感じられるときがあるような？？

実装法にどのような呼び名を付けるかはさておいて、実装の仕方によって挙動が異なるため、昔は名前で呼び分けていた。
今ではステートマシンによる実装は見かけなくなったから、ステートマシンによる挙動を実際に観測するのは難しいかも。
ステートマシンによる実装とはどのようなものか、というのが昔の記事にあるから参考になると思う。  
https://www.atmarkit.co.jp/ait/articles/1211/02/news066.html  

昔の話だけど、昔の C# のコンパイラで IL（中間コード）に変換すると、上記のページと同じ結果が得られた。
C# の実装がステートマシンからキューに変わったのが、C# 6.0 で 2015年のこと。
C# 5 までは問題なく動作していたコードが、C# 6 になってデッドロックを引き起こすコードになることもあって、この実装法の変更は割と耳にしたもの。

```
(() => {
	console.log('--- A ---');

	(async () => {
		console.log('--- B ---');
		await new Promise(f => f(null));
		console.log('--- C ---');
	})();

	console.log('--- D ---');
})();
```

## 参考３
js や C# の実装では、以下のようなコードも書ける。ちょっとアホみたいなコードだけど、挙動を観察するにはいいと思う（ネットで探しても、こんなこねくりまわしたコードは見かけないと思うw）。

以下のコードが分かれば、非同期機構とは単なる「一時中断、再実行」の仕組みであると分かるようになると思う。よく例え話でされるような「重い処理」とは別の話。
「重い処理」を、現在実行しているスレッドから分離したいならば、それはモダンな意味での非同期機構の話じゃなくて、スレッドの同期機構の話になる。
モダンな意味での非同期機構は、あくまでも「１つのスレッド」を使い回す技術についての話。

一時中断、再実行なんて簡単そうなのに、どうして最近やたらと非同期機構の話が持ち上がるのかと言うと、
最初に話をしたように、関数呼び出しにはスタックの問題があって「関数の一時中断、再実行」の仕組みに今まで目を背けていたのだけど、
最近になってその問題を解決する方向に進んだから。

参考１に挙げた (ア) と (イ) では、異なる方法でスタックの問題を解決してることに気が付ければ完璧。

```
console.log('--- A ---');

const val_async = (async () => {
	console.log('--- B ---');
	const val_promise = await new Promise(f => f('resolved'));
	console.log('--- C ---');
	return val_promise;
})();

console.log('--- D ---');
console.log(val_async);

val_async.then((val) => {
	console.log('--- E ---');
	console.log(val)
});

(async () => {
	console.log('--- F ---');
	console.log(await val_async);
	console.log('--- G ---');
})();

console.log('--- H ---');
```

## 参考４
参考３ のコードを走らせると、js の async は Promise を返すものだと気付く。そうすると、以下のようなコードを走らせることもできるようになる。
