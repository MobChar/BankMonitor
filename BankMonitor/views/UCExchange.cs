﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankMonitor.datasource;
using BankMonitor.model;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;

namespace BankMonitor.views
{
    public partial class UCExchange : UserControl
    {
        public int checkLoad = 0;
        User user;

        internal User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public UCExchange()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void LoadData()
        {

            try
            {
                if (string.IsNullOrEmpty(user.Username)) return;
                NGANHANG db = new NGANHANG();
              
                db.GD_GOIRUT.Load();
                this.bds.DataSource = db.GD_GOIRUT.Local.ToBindingList();             
            }
            catch (Exception ex)
            {
            }
        }

        bool isValid()
        {
            int flag = 1;
            var db = new NGANHANG();


            if (string.IsNullOrEmpty(tbIdStaffExchange.Text))
            {
                flag *= 0;
                errorProvider.SetError(tbIdStaffExchange, "Nhập mã!");
            }
            else if (db.NhanViens.Find(tbIdStaffExchange.Text) == null)
            {
                flag *= 0;
                errorProvider.SetError( tbIdStaffExchange,"Mã nhân viên không tồn tại!");
            } else
            {
                errorProvider.SetError(tbMoneyExchange, null);
            }
            
            if (string.IsNullOrEmpty(tbIdAccountExchange.Text))
            {
                flag *= 0;
                errorProvider.SetError(tbIdAccountExchange, "Nhập số tài khoản!");
            }
            else if (db.TaiKhoans.Find(tbIdAccountExchange.Text) == null)
            {
                flag *= 0;
                errorProvider.SetError(tbIdAccountExchange, "Số tài khoản không tồn tại!");
            } else
            {
                errorProvider.SetError(tbIdAccountExchange, null);
            }

            Regex regex = new Regex(@"^[0-9]*$");
            if (string.IsNullOrEmpty(tbMoneyExchange.Text))
            {
                errorProvider.SetError(tbMoneyExchange, "Nhập số số dư!");
                flag *= 0;
            }
            else if (Int32.Parse(tbMoneyExchange.Text) < 100000)
            {
                errorProvider.SetError(tbMoneyExchange, "Số tiền phải lớn hơn 100.000đ!");
                flag *= 0;
            }
            else if (!regex.IsMatch(tbMoneyExchange.Text))
            {
                errorProvider.SetError(tbMoneyExchange, "Sai cú pháp!");
                flag *= 0;
            }
            else
            {
                errorProvider.SetError(tbMoneyExchange, null);
            }
            //
            if (string.IsNullOrEmpty(cbTypeExchange.Text))
            {
                flag *= 0;
                errorProvider.SetError(cbTypeExchange, "*");
            } else
            {
                errorProvider.SetError(cbTypeExchange, null);
            }

            if (flag == 0) return false;
            return true;  
        }

        private void dgvExchange_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // check unclick and click in header
            try
            {
                if (dgvExchange.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvExchange.CurrentRow.Selected = true;
                    // tbIdAccount.Enabled = false;
                    tbDateExchange.Text = dgvExchange.Rows[e.RowIndex].Cells[0].FormattedValue.ToString().Trim(' ');
                    tbIdExchange.Text = dgvExchange.Rows[e.RowIndex].Cells[1].FormattedValue.ToString().Trim(' ');
                    tbIdAccountExchange.Text = dgvExchange.Rows[e.RowIndex].Cells[2].FormattedValue.ToString().Trim(' ');
                    tbMoneyExchange.Text = dgvExchange.Rows[e.RowIndex].Cells[4].FormattedValue.ToString().Trim(' ');
                    tbIdStaffExchange.Text = dgvExchange.Rows[e.RowIndex].Cells[5].FormattedValue.ToString().Trim(' ');

                    string type = "";
                    if (dgvExchange.Rows[e.RowIndex].Cells[3].FormattedValue.ToString().Trim(' ') == "GT")
                    {
                        type = "Gửi tiền";
                    }
                    else type = "Rút tiền";

                    cbTypeExchange.SelectedIndex = cbTypeExchange.FindString(type);
                }
            }
            catch
            {

            }
        }

        private void bt_SubmitExchange_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                var db = new NGANHANG();
                DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
                var time = now.ToLocalTime().ToString();
                try
                {
                    if (cbTypeExchange.Text == "Gửi tiền")
                    {
                        db.Database.ExecuteSqlCommand("guiTien @p0, @p1, @p2", parameters: new[] { tbIdAccountExchange.Text, tbMoneyExchange.Text, tbIdStaffExchange.Text });
                        LoadData();
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("rutTien @p0, @p1, @p2", parameters: new[] { tbIdAccountExchange.Text, tbMoneyExchange.Text, tbIdStaffExchange.Text });
                        LoadData();
                    }
                } catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                
                MessageBox.Show("Thêm thành công!");
            }
        }

        private void bt_CancelExchange_Click(object sender, EventArgs e)
        {
            if (dgvExchange.SelectedRows.Count > 0) dgvExchange.CurrentRow.Selected = false;
            tbIdAccountExchange.Clear();
            tbDateExchange.Clear();
            tbIdExchange.Clear();
            tbIdStaffExchange.Clear();
            tbMoneyExchange.Clear();
            cbTypeExchange.SelectedIndex = -1;
        }

        private void UCExchange_Load(object sender, EventArgs e)
        {
        }
    }

    
}
