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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QLTV
{
    public partial class QLDOCGIA : Form
    {
        SqlConnection con;

        public QLDOCGIA()
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
            string s = "Select * from DOCGIA ";
            DataTable dt = new DataTable();
            dt = TruyVan(s);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListViewItem lvi = lsvDocGia.Items.Add(dt.Rows[i]["MaDocGia"].ToString());
                lvi.SubItems.Add(dt.Rows[i]["HoTenDocGia"].ToString());
                lvi.SubItems.Add(dt.Rows[i]["NgaySinh"].ToString());
                lvi.SubItems.Add(dt.Rows[i]["DiaChi"].ToString());
                lvi.SubItems.Add(dt.Rows[i]["Email"].ToString());
                lvi.SubItems.Add(dt.Rows[i]["NgayLapThe"].ToString());
                lvi.SubItems.Add(dt.Rows[i]["NgayHetHan"].ToString());
                lvi.SubItems.Add(dt.Rows[i]["TienNo"].ToString());

              // lvi.SubItems.Add(dt.Rows[i]["TenBangCap"].ToString());
            }
        }

       

        void Xoa()
        {
            txtMa.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            dtpLap.Value = DateTime.Now;
            dtpHet.Value = DateTime.Now;
            txtDiaChi.Clear();
            txtEmail.Clear();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {


        }

        private void QLDOCGIA_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
           btncancel.Enabled = false;
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

        private void lsvDocGia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDocGia.SelectedItems.Count > 0)
            {
                txtMa.Text = lsvDocGia.SelectedItems[0].SubItems[0].Text;
                txtHoTen.Text = lsvDocGia.SelectedItems[0].SubItems[1].Text;
                dtpNgaySinh.Text = lsvDocGia.SelectedItems[0].SubItems[2].Text;
                txtDiaChi.Text = lsvDocGia.SelectedItems[0].SubItems[3].Text;
                txtEmail.Text = lsvDocGia.SelectedItems[0].SubItems[4].Text;
                dtpLap.Text = lsvDocGia.SelectedItems[0].SubItems[5].Text;
                dtpHet.Text = lsvDocGia.SelectedItems[0].SubItems[6].Text;
                txtNo.Text = lsvDocGia.SelectedItems[0].SubItems[7].Text;
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
            btncancel.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát ", "Kết nối không thành công", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string ht = txtHoTen.Text;
            string ns = dtpNgaySinh.Value.ToShortDateString();
            string dc = txtDiaChi.Text;
            string dt = txtEmail.Text;
            string nl = dtpLap.Value.ToShortDateString();
            string nh = dtpHet.Value.ToShortDateString();
            int tn = int.Parse(txtNo.Text);
            string s = " Insert into DOCGIA values(N'" + ht + "','" + ns + "',N'" + dc + "',N'" + dt + "','" + nl + "','" + nh + "','" + tn + "')";
            if (ThemXoaSua(s) == true)
            {
                lsvDocGia.Items.Clear();
                LayDuLieu_Len_Listview();
            }
            Xoa();

        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            if (lsvDocGia.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng để xóa");
                return;
            }

            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát chương trình", "Không kết nối CSDL được!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            foreach (ListViewItem i in lsvDocGia.SelectedItems)
            {
                string s = "delete from DOCGIA where MaDocGia = N'" + i.SubItems[0].Text + "'";
                SqlCommand cmd = new SqlCommand(s, con);
                ThemXoaSua(s);
            }
            con.Close();
            lsvDocGia.Items.Clear();
            LayDuLieu_Len_Listview();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (KetNoi("LAPTOP-FAMD6FDU\\PHAMHAO", "QLThuVien") == false)
            {
                MessageBox.Show("Nhấn OK để thoát chương trình", "Không kết nối CSDL được!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (lsvDocGia.SelectedItems.Count == 0)
            {
                MessageBox.Show("Chưa chọn dòng dữ liệu cần nhập");
                return;
            }
            string ht = txtHoTen.Text;
            string ns = dtpNgaySinh.Value.ToShortDateString();
            string dc = txtDiaChi.Text;
            string dt = txtEmail.Text;
            string nl = dtpLap.Value.ToShortDateString();
            string nh = dtpHet.Value.ToShortDateString();
            int tn = int.Parse(txtNo.Text);
            string s = "update DOCGIA set HoTenDocGia = N'" + ht + "',NgaySinh='" + ns + "',DiaChi = N'" + dc + "',Email= '" + dt + "',NgayLapThe= '" + nl + "' ,NgayHetHan= '" + nh + "',TienNo= '" + tn + "' where MaDocGia ='" + txtMa.Text + "'";
            if (ThemXoaSua(s) == true)
            {
                
                lsvDocGia.Items.Clear();
                LayDuLieu_Len_Listview();
            }
            
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            txtMa.Enabled = false;
            btnAdd.Enabled = true;
            btnDel.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = true;
            btncancel.Enabled = true;
        }
    }
}
