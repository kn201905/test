using System;
using System.Windows.Forms;

namespace http_test
{
	public partial class MainForm : Form
	{
		static TextBox ms_tbox_sys_msg;

		public MainForm()
		{
			InitializeComponent();

			ms_tbox_sys_msg = m_tbox_sys_msg;
		}

		private void OnClick_CloseBtn(object sender, EventArgs e)
		{
			MainForm.Wrt_SysMsg("--- 接続実験開始");

			Http_Socket.test();
		}

		public static void Wrt_SysMsg(string msg)
		{
			ms_tbox_sys_msg.AppendText(msg);
		}
	}
}
