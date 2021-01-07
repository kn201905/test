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
### (カ)
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

<BR>

### (キ)
```
(async () => {
	console.log('--- A ---');

	await (async () => {
		console.log('--- B ---');
		await new Promise(f => f(null));
		console.log('--- C ---');
	})();

	console.log('--- D ---');
})();
```
