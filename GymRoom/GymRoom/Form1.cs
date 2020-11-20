using GymRoom.E;
using GymRoom.EF;
using GymRoom.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
namespace GymRoom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            designGridView();
            dao = new GymerDao();

            
            checkGV = true;

            pbIcon.Image = Image.FromFile(Application.StartupPath + @"\icon.png");
            this.Icon = new Icon(Application.StartupPath+@"\icon.ico");
            MessageBox.Show("Chào boss ngày mới");
            loadGridView();
        }

        GymerDao dao;
        bool checkGV = false;

        
        bool checkMonth()
        {
            try
            {
                int.Parse(txtMonth.Text);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng nhập số tháng hợp lệ !");
                return false;
            }
        }
        DialogResult optionYes(string content,string tittle, MessageBoxIcon icon)
        {
           return MessageBox.Show(content, tittle, MessageBoxButtons.YesNo,icon);
        }
        void loadGridView()
        {
            this.gvInformation.DataSource = dao.GetListGymer();
            for (int i = 0; i < gvInformation.Rows.Count; i++)
            {
                this.gvInformation.Rows[i].Cells[8].Value = (i+1).ToString();
                this.gvInformation.Rows[i].Cells[8].Style.BackColor = Color.LightGray;
         
            }
            this.txtSumGymer.Text=this.gvInformation.Rows.Count.ToString();


        }
        void designGridView()
        {
            this.gvInformation.AutoGenerateColumns = false;
        }

        private void DangKy_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadGridView();
        }
        private void gvInformation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                txtMonth.Text = "";
                this.txtID.Text = gvInformation.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.txtName.Text = gvInformation.Rows[e.RowIndex].Cells[1].Value.ToString();
                this.txtAdress.Text = gvInformation.Rows[e.RowIndex].Cells[4].Value.ToString();
                this.txtNote.Text = gvInformation.Rows[e.RowIndex].Cells[5].Value.ToString();

                DateTime timeRe = Convert.ToDateTime(gvInformation.Rows[e.RowIndex].Cells[2].Value.ToString());
                this.dtpDateRegistration.Value = timeRe;

                DateTime timeExpire = Convert.ToDateTime(gvInformation.Rows[e.RowIndex].Cells[3].Value.ToString());
                this.txtDateExpired.Text = timeExpire.Date.ToShortDateString();


                TimeSpan value = timeExpire.Subtract(DateTime.Now);
                if (value.Days <= 5)
                {
                    txtSoNgayConLai.ForeColor = Color.Red;

                }
                else txtSoNgayConLai.ForeColor = Color.Green;


                txtSoNgayConLai.Text = value.Days.ToString();
            }
            catch (Exception) { Console.WriteLine("LOI CLICK CELL"); }
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
           // if (txtFind.Text.Trim().Equals("")) { this.gvInformation.DataSource = dao.GetListGymer(); return; }
            this.gvInformation.DataSource = dao.findGymer(txtFind.Text.Trim());
            for (int i = 0; i < gvInformation.Rows.Count; i++)
            {
                this.gvInformation.Rows[i].Cells[8].Value = (i + 1).ToString();
            }
            this.txtSumGymer.Text = this.gvInformation.Rows.Count.ToString();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(!txtID.Text.Equals("none"))
            {
                MessageBox.Show("Vui lòng clear đã !");
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Nhập tên gymer đi đã !");
                return;
            }
            if (!checkMonth()) return;
            if (!checkValidTextBox()) return;

            if (optionYes("Bạn có muốn thêm người dùng này không ?", "Confirm", MessageBoxIcon.Question) == DialogResult.No) return;

            GYMER gymer = new GYMER();
            gymer.name = txtName.Text;
            gymer.numMonth = int.Parse(txtMonth.Text);
            gymer.adress = txtAdress.Text;
            gymer.note = txtNote.Text;

            gymer.dateRegistraion = dtpDateRegistration.Value;
            gymer.dateModify = DateTime.Now;
            gymer.dateCreate = DateTime.Now;
            gymer.dateExpired = (dtpDateRegistration.Value).AddMonths(int.Parse(txtMonth.Text));
            gymer.status = true;
            dao.addGymer(gymer);
            MessageBox.Show("Thêm gymer thành công");
            clear();
            loadGridView();
        }

        
        private void txtMonth_TextChanged(object sender, EventArgs e)
        {

            changeSoNgayConLai();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Equals("none"))
            {
                MessageBox.Show("Phải chọn gymer trước đã");
                return;
            }

            if (optionYes("Bạn có muốn xóa người dùng này không ?", "Confirm", MessageBoxIcon.Warning) == DialogResult.No) return;

            if (dao.deleteGymer(long.Parse(txtID.Text)))
            {
                MessageBox.Show("Xóa thành công");
                clear();
            }else MessageBox.Show("Xóa thất bại");

            loadGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
        void clear()
        {
            txtID.Text = "none";
            txtName.Text = "";
            txtDateExpired.Text = "none";
            txtMonth.Text = "";
            txtAdress.Text = "";
            txtNote.Text = "";
            txtSoNgayConLai.Text = "0";
            dtpDateRegistration.Value = DateTime.Now;
        }
        void changeSoNgayConLai()
        {
            try
            {
                DateTime test = dtpDateRegistration.Value;
                DateTime expired = test.AddMonths(int.Parse(txtMonth.Text));
                txtDateExpired.Text = expired.Date.ToShortDateString();

                TimeSpan value = expired.Subtract(DateTime.Now);
                if (value.Days <= 5)
                {
                    txtSoNgayConLai.ForeColor = Color.Red;

                }
                else txtSoNgayConLai.ForeColor = Color.Green;


                txtSoNgayConLai.Text = value.Days.ToString();

            }
            catch (Exception)
            {
                txtDateExpired.Text = "none";
                txtSoNgayConLai.Text = "0";
            }
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Equals("none"))
            {
                MessageBox.Show("Phải chọn gymer trước đã");
                return;
            }
            if (!checkMonth()) return;

            if (!checkValidTextBox()) return;
            if (optionYes("Bạn có muốn gia hạn cho "+txtName.Text+" không ?", "Confirm", MessageBoxIcon.Warning) == DialogResult.No) return;

            bool check = dao.extendGymer(long.Parse(txtID.Text), dtpDateRegistration.Value, (dtpDateRegistration.Value).AddMonths(int.Parse(txtMonth.Text)), int.Parse(txtMonth.Text));
            if (check)
            {

                
                loadGridView();
                MessageBox.Show("Gia hạn thành công");
                clear();
            }else MessageBox.Show("Gia hạn không thành công");


        }

        private void dtpDateRegistration_ValueChanged(object sender, EventArgs e)
        {
            txtMonth.Text = "";
        }

        private void gvInformation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < gvInformation.Rows.Count; i++)
            {
                DateTime expired = Convert.ToDateTime(gvInformation.Rows[i].Cells[3].Value.ToString());
                Console.WriteLine(expired.Date.ToShortDateString());
                TimeSpan value = expired.Subtract(DateTime.Now);
                Console.WriteLine(value.Days);
                if (value.Days >= 5)
                {
                    this.gvInformation.Rows[i].Cells[6].Style.ForeColor = Color.Green;
                    this.gvInformation.Rows[i].Cells[6].Value = "OK";
                }
                else
                {
                    if(value.Days < 5)
                    {
                      
                        this.gvInformation.Rows[i].Cells[6].Style.ForeColor = Color.Red;

                        if(value.Days == 0) { this.gvInformation.Rows[i].Cells[6].Value = "Hết hạn"; }
                        else this.gvInformation.Rows[i].Cells[6].Value = "Gần hết hạn";
                    }
                   
                }
            }

        }

        private void gvInformation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void btnAdd_MouseHover(object sender, EventArgs e)
        {
            tssNoti.Text = "Phải clear trước, nhập tên gymer, nhập số tháng";
        }

        private void btnXoa_MouseHover(object sender, EventArgs e)
        {
            tssNoti.Text = "Phải click vào gymer ở bảng thống kê trước rồi mới xóa được";
        }

        private void btnClear_MouseHover(object sender, EventArgs e)
        {
            tssNoti.Text = "Làm trống các ô";
        }

        private void btnGiaHan_MouseHover(object sender, EventArgs e)
        {
            tssNoti.Text = "Phải chọn gymer ở bảng thống kê trước rồi mới nhập số tháng để gia hạn";
        }

        private void btnAdd_MouseLeave(object sender, EventArgs e)
        {
            tssNoti.Text = "";
        }

        private void btnXoa_MouseLeave(object sender, EventArgs e)
        {
            tssNoti.Text = "";
        }

        private void btnClear_MouseLeave(object sender, EventArgs e)
        {
            tssNoti.Text = "";
        }

        private void btnGiaHan_MouseLeave(object sender, EventArgs e)
        {
            tssNoti.Text = "";
        }

        private void txtFind_MouseHover(object sender, EventArgs e)
        {
            tssNoti.Text = "Ô tìm kiếm";
        }

        private void txtFind_MouseLeave(object sender, EventArgs e)
        {
            tssNoti.Text = "";
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            tssNoti.Text = "Chỉ sửa được tên gymer, nơi ở và note";
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            tssNoti.Text = "";
        }
        bool checkValidTextBox()
        {
            if (txtName.Text.Length > 50)
            {
                MessageBox.Show("Tên chỉ nhập được 50 kí tự");
                return false;
            }
            
            if (txtAdress.Text.Length > 250)
            {
                MessageBox.Show("Nơi ở chỉ nhập được 250 kí tự");
                return false;
            }
            if (txtNote.Text.Length > 250)
            {
                MessageBox.Show("Note chỉ nhập được 250 kí tự");
                return false;
            }
            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Equals("none"))
            {
                MessageBox.Show("Phải chọn gymer trước đã");
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Nhập tên gymer đi đã !");
                return;
            }
            if (!checkValidTextBox()) return;
            if (optionYes("Bạn có muốn sửa gymer " + txtName.Text + " không ?", "Confirm", MessageBoxIcon.Question) == DialogResult.No) return;
          
            bool check = dao.updateGymer(long.Parse(txtID.Text),txtName.Text,txtAdress.Text,txtNote.Text);
            if (check)
            {


                loadGridView();
                MessageBox.Show("Sửa thành công");
                clear();
            }
            else MessageBox.Show("Sửa không thành công");

        }

        private void exportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = "GymManager.xlsx";
            string customExcelSavingPath = Application.StartupPath + @"\Assets\" + fileName;
            ExcelUtil.GenerateExcel(ExcelUtil.ConvertToDataTable(dao.GetListGymer()),customExcelSavingPath);
            MessageBox.Show("OK ! Dã lưu file trong thư mục Assets \r\n path:"+ customExcelSavingPath);
        }

        private void facebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Copy link ở dưới và dán vào chrome ", "Liên hệ qua facebook", "https://www.facebook.com/profile.php?id=100009229815468");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadGridView();
        }
    }
}
