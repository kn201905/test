# js の非同期機構

## よくあるパターン

### (A) Promise による実装（Chrome では 2014年に実装。V8 3.2）

```
new Promise(resolve => {
	console.log('--- A ---');
	WaitClient(resolve);
	console.log('--- B ---');
})
.then((msg) => {
	console.log('--- msg ---');
	console.log(msg);
});

console.log('--- C ---');
```

### (B) async による実装（Chrome では 2016年に実装。V8 5.5）
```
const WaitClientAsync = async () => {
	await new Promise(resolve => {
		setTimeout(() => resolve(null), 2000);
	});
	return 'connected';
}

(async () => {
	console.log('--- A ---');
	const msg = await WaitClientAsync();
	console.log('--- B ---');
	console.log('--- msg ---');
	console.log(msg);
})();

console.log('--- C ---');
```

### (C) 昔ながらの実装（いつの時代でも使えた方法）
```
console.log('--- X ---');
WaitClient(OnConnect);
console.log('--- Y ---');

function OnConnect(msg) {
	console.log('--- msg ---');
	console.log(msg);
};
```
