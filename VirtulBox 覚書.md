# 光学ドライブに Ubuntu の cdrom イメージをセットして起動 -> システムインストール
・インストールの実行が終わったら、cdrom イメージはイジェクトしておくこと（マシン起動順序も確認）  
・ユーザーインターフェース設定 -> 入力 -> マウス統合 は、外しておくと良い

# ネットワーク設定
・ブリッジ接続 -> 必要があれば /etc/netplan の設定の変更

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


