using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTV
{
    public partial class QLSACH : Form
    {
        SqlConnection con;

        public QLSACH()
        {
            InitializeComponent();
        }
        public bool KetNoi(String server, String database)
        {
            try
            {
                String s = @"Data Source=LAPTOP-FAMD6FDU\PHAMHAO;Initial Catalog=QLThuVien;Integrated Security=True";
                con = new SqlConnection(s);
                con.Open();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        DataTable TruyVan(String s)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            try
            {
                da = new SqlDataAdapter(s, con);
                da.Fill(ds, "KQ");
                con.Close();
                return ds.Tables["KQ"];
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi truy vấn CSDL. ");
                return new DataTable();
            }
        }

        bool ThemXoaSua(String s)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(s, con);
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi cập nhật CSDL");
                return false;
            }
        }



        void LayDuLieu_Len_Listview()
        {
            string s = "Select * from SACH ";
            DataTable dt = new DataTable();
            dt = TruyVan(s);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListViewItem lvi = lsvSach.Items.Add(dt.Rows[i]["MaSach"].ToString());
                lvi.SubItems.Add(dt.Rows[i][1].ToString());
                lvi.SubItems.Add(dt.Rows[i][2].ToString());
                lvi.SubItems.Add(dt.Rows[i][3].ToString());
                lvi.SubItems.Add(dt.Rows[i][4].ToString());
                lvi.SubItems.Add(dt.Rows[i][5].ToString());
                lvi.SubItems.Add(dt.Rows[i][6].ToString());
                // lvi.SubItems.Add(dt.Rows[i]["TenBangCap"].ToString());
            }
        }



        void Xoa()
        {
            txtMa.Clear();
            txtHoTen.Clear();
            txtTG.Clear();
            txtNamXB.Clear();
            txtNXB.Clear();
            txtTriG.Clear();
            dtpNgaySinh.Value = DateTime.Now;
         
         
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void QLSACH_Load(object sender, EventArgs e)
        {
            //btnSave.Enabled = false;
            //btncancel.Enabled = false;
            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien"))
            {
                LayDuLieu_Len_Listview();

            }
            else
            {
                MessageBox.Show("Nhấn OK để thoát ", "Kết nối không thành công ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }

        private void lsvSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvSach.SelectedItems.Count > 0)
            {
                txtMa.Text = lsvSach.SelectedItems[0].SubItems[0].Text;
                txtHoTen.Text = lsvSach.SelectedItems[0].SubItems[1].Text;
                txtTG.Text = lsvSach.SelectedItems[0].SubItems[2].Text;
                txtNamXB.Text = lsvSach.SelectedItems[0].SubItems[3].Text;
                txtNXB.Text = lsvSach.SelectedItems[0].SubItems[4].Text;
                txtTriG.Text = lsvSach.SelectedItems[0].SubItems[5].Text;
                dtpNgaySinh.Text = lsvSach.SelectedItems[0].SubItems[6].Text;
                txtMa.Enabled = false;

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtMa.Enabled = false;
            btnAdd.Enabled = false;
            btnDel.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát ", "Kết nối không thành công", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string ht = txtHoTen.Text;
            string tg = txtTG.Text;
            string namxb = txtNamXB.Text;
            string nxb = txtNXB.Text;
            int trig = int.Parse(txtTriG.Text);
            string ns = dtpNgaySinh.Value.ToShortDateString();
            string s = " Insert into SACH values(N'" + ht + "',N'" + tg + "',N'" + namxb + "',N'" + nxb + "','" + trig + "','" + ns + "')";
            if (ThemXoaSua(s) == true)
            {
                lsvSach.Items.Clear();
                LayDuLieu_Len_Listview();
            }
            Xoa();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            if (lsvSach.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng để xóa");
                return;
            }

            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát chương trình", "Không kết nối CSDL được!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            foreach (ListViewItem i in lsvSach.SelectedItems)
            {
                string s = "delete from Sach where MaSach = N'" + i.SubItems[0].Text + "'";
                SqlCommand cmd = new SqlCommand(s, con);
                ThemXoaSua(s);
            }
            con.Close();
            lsvSach.Items.Clear();
            LayDuLieu_Len_Listview();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát chương trình", "Không kết nối CSDL được!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (lsvSach.SelectedItems.Count == 0)
            {
                MessageBox.Show("Chưa chọn dòng dữ liệu cần nhập");
                return;
            }
            string ht = txtHoTen.Text;
            string tg = txtTG.Text;
            string namxb = txtNamXB.Text;
            string nxb = txtNXB.Text;
            int trig = int.Parse(txtTriG.Text);
            string ns = dtpNgaySinh.Value.ToShortDateString();
            string s = "update SACH set TenSach = N'" + ht + "',TacGia=N'" + tg + "',NamXuatBan ='" + namxb+ "',NhaXuatBan= N'" + nxb + "',TriGia= '" + trig + "',NgayNhap= '" + ns + "' where MaSach ='" + txtMa.Text + "'";
            //    string s = "update DOCGIA set HoTenDocGia = N'" + ht + "',NgaySinh='" + ns + "',Diachi = N'" + dc + "',Email= '" + dt + "',NgayLapThe= '" + nl + "',NgayHetHan= '" + nh + "',TienNo= '" + tn + "' where MaDocGia = N'" + ma + "'";
            if (ThemXoaSua(s) == true)
            {
                con.Close();
                lsvSach.Items.Clear();
                LayDuLieu_Len_Listview();
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            txtMa.Enabled = false;
            btnAdd.Enabled = true;
            btnDel.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = true;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
