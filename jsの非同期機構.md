## よくあるパターン

### (A)
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

### (B)
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
