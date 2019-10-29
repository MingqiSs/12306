using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _12036ByTicket
{
    public partial class Form2 : Form
    {
        List<Test> list = new List<Test>();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            list.Add(new Test {test=$"10:29{System.Environment.NewLine}10:29",test1="2",test2="3",test3="4",ed="操作" });
            list.Add(new Test { test = "222", test1 = "2", test2 = "3", test3 = "4", ed = "操作" });
            //设置自动换行  
            this.skinDataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度  
            this.skinDataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            skinDataGridView1.ReadOnly = true;
            skinDataGridView1.DataSource = list;


            #region 初始化点击事件
            ToolStripMenuItem tsMenumItem = new ToolStripMenuItem("删除选中");
            tsMenumItem.Click += ToolStripMenuItem_Click;
            this.cms_train.Items.Add(tsMenumItem);
            tsMenumItem = new ToolStripMenuItem("清空列表");
            tsMenumItem.Click += ToolStripMenuItem_Click;
            this.cms_train.Items.Add(tsMenumItem);
            #endregion
        }

        private void skinDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            MessageBox.Show("选中行" + (e.RowIndex + 1));
            if (e.RowIndex>0)
            {
                Test a = list[e.RowIndex];
            }
        }
        /// <summary>
        /// 设定右键菜单勾选项,设置ListView列表显示样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsMenumItem = sender as ToolStripMenuItem;
            if (tsMenumItem.Text == "清空列表")
            {
               // select_train_lb.Items.Clear();
            }
            if (tsMenumItem.Text == "删除选中")
            {
                //for (int i = 0; i < select_train_lb.Items.Count; i++)
                //{
                //    if (select_train_lb.GetSelected(i))
                //    {
                //        select_train_lb.Items.RemoveAt(i);
                //    }
                //}

            }
        }
      
        private void skinDataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.cms_train.Show(Control.MousePosition.X, Control.MousePosition.Y);

            }
        }
    }
    public class Test {

        public string test { get; set; }
        public string test1 { get; set; }
        public string test2 { get; set; }
        public string test3 { get; set; }
        public string ed { get; set; }
    }
}
