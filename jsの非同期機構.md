# js の非同期機構

## よくあるパターン

### (A) Promise（Chrome では 2014年に実装。V8 3.2）によるパターン

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

### (B) async（Chrome では 2016年に実装。V8 5.5）によるパターン
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

### (C) 昔ながらのパターン
```
console.log('--- X ---');
WaitClient(OnConnect);
console.log('--- Y ---');

function OnConnect(msg) {
	console.log('--- msg ---');
	console.log(msg);
};
```
