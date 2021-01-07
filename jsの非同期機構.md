# js の非同期機構

## 参考１

### (ア) Promise による実装（Chrome では 2014年に実装。V8 3.2）

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

### (イ) async による実装（Chrome では 2016年に実装。V8 5.5）
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

js の実装法も、このあたりで変更があった可能性があるかも。

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
