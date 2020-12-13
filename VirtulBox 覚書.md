# 光学ドライブに Ubuntu の cdrom イメージをセットして起動 -> システムインストール
・インストールの実行が終わったら、cdrom イメージはイジェクトしておくこと（マシン起動順序も確認）
・ユーザーインターフェース設定 -> 入力 -> マウス統合 は、外しておくと良い

# ネットワーク設定
・ブリッジ接続 -> netplan の設定の変更

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
