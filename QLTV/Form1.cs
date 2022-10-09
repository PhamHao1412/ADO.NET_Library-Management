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
    public partial class Form1 : Form
    {
        SqlConnection con;
        public Form1()
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

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void LayDuLieu_Len_Listview()
        {
            string s = "Select *from NHANVIEN a, BangCap b WHERE a.MaBangCap = b.MaBangCap";
            DataTable dt = new DataTable();
            dt = TruyVan(s);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListViewItem lvi = lsvNhanVien.Items.Add(dt.Rows[i]["MaNhanVien"].ToString());
                lvi.SubItems.Add(dt.Rows[i][1].ToString());
                lvi.SubItems.Add(dt.Rows[i][2].ToString());
                lvi.SubItems.Add(dt.Rows[i][3].ToString());
                lvi.SubItems.Add(dt.Rows[i][4].ToString());
                lvi.SubItems.Add(dt.Rows[i]["TenBangCap"].ToString());
            }
        }

        void NapBangCapComboBox()
        {
            String s = "Select TenBangCap from BangCap";
            DataTable dt = new DataTable();
            dt = TruyVan(s);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbbBangCap.Items.Add(dt.Rows[i][0].ToString());
            }
        }

        void Xoa()
        {
            txtMaNhanVien.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            cbbBangCap.SelectedIndex = 0;
        }


    

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            
            txtMaNhanVien.Enabled = false;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (lsvNhanVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng để xóa");
                return;
            }

            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát chương trình", "Không kết nối CSDL được!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            foreach (ListViewItem i in lsvNhanVien.SelectedItems)
            {
                string s = "delete from NHANVIEN where MaNhanVien = N'" + i.SubItems[0].Text + "'";
                SqlCommand cmd = new SqlCommand(s, con);
                ThemXoaSua(s);
            }
            con.Close();
            lsvNhanVien.Items.Clear();
            LayDuLieu_Len_Listview();

        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát chương trình", "Không kết nối CSDL được!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (lsvNhanVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("Chưa chọn dòng dữ liệu cần nhập");
                return;
            }
           
            string ht = txtHoTen.Text;
            string ns = dtpNgaySinh.Value.ToShortDateString();
            string dc = txtDiaChi.Text;
            string sdt = txtDienThoai.Text;
            int bc = cbbBangCap.SelectedIndex + 1;
            string s = "update NHANVIEN set HoTenNhanVien = N'" + ht + "',NgaySinh='" + ns + "',Diachi = N'" + dc + "',Dienthoai= '" + sdt + "',MaBangCap= '" + bc + "' where MaNhanVien = N'" + txtMaNhanVien.Text + "'";
            if (ThemXoaSua(s) == true)
            {
                con.Close();
                lsvNhanVien.Items.Clear();
                LayDuLieu_Len_Listview();
            }
            Xoa(); ;

        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
           
            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát ", "Kết nối không thành công", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string ht = txtHoTen.Text;
            string ns = dtpNgaySinh.Value.ToShortDateString();
            string dc = txtDiaChi.Text;
            string dt = txtDienThoai.Text;
            int bc = cbbBangCap.SelectedIndex + 1;
            string s = " Insert into NHANVIEN values(N'" + ht + "','" + ns + "',N'" + dc + "','" + dt + "','" + bc + "')";
            if (ThemXoaSua(s)==true)
            {
                lsvNhanVien.Items.Clear();
                LayDuLieu_Len_Listview();
            }
            Xoa();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaNhanVien.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
        }

        private void btnthoat_Click_1(object sender, EventArgs e)
        {
            this.Close();

        }

        private void lsvNhanVien_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lsvNhanVien.SelectedItems.Count > 0)
            {
                txtMaNhanVien.Text = lsvNhanVien.SelectedItems[0].SubItems[0].Text;
                txtHoTen.Text = lsvNhanVien.SelectedItems[0].SubItems[1].Text;
                dtpNgaySinh.Text = lsvNhanVien.SelectedItems[0].SubItems[2].Text;
                txtDiaChi.Text = lsvNhanVien.SelectedItems[0].SubItems[3].Text;
                txtDienThoai.Text = lsvNhanVien.SelectedItems[0].SubItems[4].Text;
                cbbBangCap.SelectedIndex = cbbBangCap.FindString(lsvNhanVien.SelectedItems[0].SubItems[5].Text);
                txtMaNhanVien.Enabled = false;
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            if (KetNoi("LAPTOP-FAMD6FDU\\SQLEXPRESS", "QLThuVien"))
            {
                LayDuLieu_Len_Listview();
                NapBangCapComboBox();
            }
            else
            {
                MessageBox.Show("Nhấn OK để thoát ", "Kết nối không thành công ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }
    }
}
