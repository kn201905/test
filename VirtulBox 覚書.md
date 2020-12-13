# 光学ドライブに Ubuntu の cdrom イメージをセットして起動 -> システムインストール
・インストールの実行が終わったら、cdrom イメージはイジェクトしておくこと（マシン起動順序も確認）  
・ユーザーインターフェース設定 -> 入力 -> マウス統合 は、外しておくと良い

# ネットワーク設定
・ブリッジ接続 -> 必要があれば /etc/netplan の設定の変更

# SSH で接続
```
$ sudo su -
# apt update
# apt upgrade
```

# Guest Additions のインストール（共有フォルダを利用するため）
・VM ウィンドウのメニュー -> デバイス -> Guest Additions CD イメージの挿入
```
# mkdir /mnt/cdrom
# mount -t iso9660 /dev/cdrom /mnt/cdrom

Guest Additions のインストール
# apt install -y gcc make perl linux-headers-generic

Guest Additionsをビルドしてインストール
# cd /mnt/cdrom
# ./VBoxLinuxAdditions.run
# reboot
```

# 共有フォルダの設定
・VirtualBox の設定 -> 共有フォルダ にて、フォルダを指定して、自動マウント、永続化のスイッチを入れる  
・仮想マシンを立ち上げると、共有フォルダが利用できるようになっている

# iptables の設定

```
#!/bin/bash

zone_safe='192.168.0.0/24'

iptables -F  # テーブル初期化
iptables -X  # チェーンを削除
iptables -Z  # パケットカウンタ・バイトカウンタをクリア

# ポリシーの設定
iptables -P INPUT DROP
#iptables -P OUTPUT DROP
iptables -P OUTPUT ACCEPT
iptables -P FORWARD DROP


# 不審なパケットは破棄
iptables -A INPUT -p tcp ! --syn -m state --state NEW -j DROP

# loopback
iptables -A INPUT -i lo -j ACCEPT
iptables -A OUTPUT -o lo -j ACCEPT

# 確立している接続は許可
iptables -A INPUT -p tcp -m state --state ESTABLISHED,RELATED -j ACCEPT
iptables -A OUTPUT -p tcp -m state --state ESTABLISHED,RELATED -j ACCEPT

iptables -A INPUT -p udp -m state --state ESTABLISHED,RELATED -j ACCEPT
iptables -A OUTPUT -p udp -m state --state ESTABLISHED,RELATED -j ACCEPT
# <<<<<<<<<< ここまで定番の操作 <<<<<<<<<<


iptables -A INPUT -s $zone_safe -j ACCEPT
iptables -A OUTPUT -d $zone_safe -j ACCEPT


#********** LOG **********
#iptables -A INPUT -j LOG --log-level warning --log-prefix "Drp_IN: " -m limit --limit 2/m --limit-burst 5
iptables -A INPUT -j DROP

#iptables -A OUTPUT -j LOG --log-level warning --log-prefix "Drp_OUT: " -m limit --limit 2/m --limit-burst 5
#iptables -A OUTPUT -j DROP
iptables -A OUTPUT -j ACCEPT

echo 'iptables の設定が正しく処理されました'
```

* 念のために、v6 アドレスがローカルしか振られていないことを確認。必要があれば、ip6tables を設定

# node.js をインストール
```
まず、n package を動かすために、古いものでいいので nodejs をインストールする
# apt install nodejs npm
# npm install n -g

安定バージョンをインストール（新しい nodejs は、/usr/local/bin にインストールされる）
# n stable

新しい nodejs がインストールされたため、古いものは削除
# apt purge -y nodejs npm

環境変数 node を更新するため、シェルを再起動
# exec $SHELL -l

バージョン確認
# node -v
```

# node お試し
* httpserver.js
```
'use strict';

const http = require('http');
const fs = require('fs');

const server = http.createServer();
const index_html = fs.readFileSync('index.html', 'utf-8');

server.on('request', (req, res) => {
	res.writeHead(200, {'Content-Type' : 'text/html'});
	res.write(index_html);
	res.end();
});

server.listen(80);
```

* index.html
```
<!DOCTYPE html>
<html lang="ja">
<head>
	<meta charset="utf-8">
	<title>テスト</title>
</head>

<body>
<H1>Hello!</H1>
</body>
</html>
```
