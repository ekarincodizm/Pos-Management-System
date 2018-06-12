using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Pos_Management_System
{
    public partial class FormReportMember : Form
    {
        public FormReportMember()
        {
            InitializeComponent();
        }
        private void CbRegistor_CheckedChanged(object sender, EventArgs e)
        {

            if (CbRegistor.Checked == true)
            {
                GbRegistor.Enabled = true;
                CbAdd.Checked = false;
                CbDelete.Checked = false;
            }
            else
            {
                GbRegistor.Enabled = false;
            }

        }

        private void CbAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (CbAdd.Checked == true)
            {
                GbAdd.Enabled = true;
                CbDelete.Checked = false;
                CbRegistor.Checked = false;
            }
            else
            {
                GbAdd.Enabled = false;
            }
        }

        private void CbDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (CbDelete.Checked == true)
            {
                GbDelete.Enabled = true;
                CbAdd.Checked = false;
                CbRegistor.Checked = false;
            }
            else
            {
                GbDelete.Enabled = false;
            }
        }
        private void FormReportMember_Load(object sender, EventArgs e)
        {
            CbRegistor_CheckedChanged(sender, e);
            CbDelete_CheckedChanged(sender, e);
            CbAdd_CheckedChanged(sender, e);
            CobRegistor.SelectedIndex = 0;
            CobSex.SelectedIndex = 0;
            CombBuyS.SelectedIndex = 0;
            CombBuySex.SelectedIndex = 0;
        }

        private void TbMemberS_TextChanged(object sender, EventArgs e)
        {
            TbMemberE.Text = TbMemberS.Text;
        }

        private void TbMemberSKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }
        }

        private void TbMemberEKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }
        }

        private void TbNoMemberKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }

        }

        private void TbNoMemberEKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }
        }

        private void BtReport1_Click(object sender, EventArgs e)
        {
            try
            {

                if (CbAdd.Checked == true)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DataTable dt = new DataTable();
                    using (SSLsEntities db = new SSLsEntities())
                    {
                        var ds = db.MemberShare.Where(w => w.Enable == true & w.Member.Enable == true & w.CreateDate >= DtAddS.Value & w.CreateDate <= DtAddE.Value & (CobRegistor.SelectedIndex == 1 ? w.Member.IsRemoveShared == false : (CobRegistor.SelectedIndex == 2 ? w.Member.IsRemoveShared == true : w.Enable == true)) & (CobSex.SelectedIndex == 1 ? w.Member.Sex.Id == 2 : (CobSex.SelectedIndex == 2 ? w.Member.Sex.Id == 3 : w.Enable == true)) &
                          (TbMemberE.Text == "" ? w.Enable == true : w.Member.Code.Contains(TbMemberS.Text.Trim()) & w.Member.Code.Contains(TbMemberE.Text.Trim())) &
                          (TbNoMemberS.Text == "" ? w.Enable == true : w.Member.Code != TbNoMemberS.Text.Trim())).Select(s => new
                          {
                              Id = s.Member.Id,
                              MemberId = s.Member.Code,
                              MemberName = s.Member.Name,
                              RegistorDate = s.Member.CreateDate,
                              s.Member.Address,
                              s.Member.TaxId,
                              Sex = s.Member.Sex.Name,
                              QtyOld = (s.Member.MemberShare.Where(w => w.Enable == true & w.CreateDate < s.CreateDate).Sum(ss => ss.Qty) == null ? 0 : s.Member.MemberShare.Where(w => w.Enable == true & w.CreateDate < s.CreateDate).Sum(ss => ss.Qty)),
                              Qty = s.Qty,
                              QtyTotal = s.Member.MemberShare.Where(w => w.Enable == true).Sum(ss => ss.Qty),
                              AddShareDate = s.CreateDate,
                          }).OrderBy(o => o.MemberId).ToList();
                        if (ds.Count > 0)
                        {
                            dt = Library.ConvertToDataTable(ds);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //dataGridView1.DataSource = dt;
                        FormReportViewer formReportViewer = new FormReportViewer(this);
                        formReportViewer.reportViewer1.Reset();
                        formReportViewer.reportViewer1.ProcessingMode = ProcessingMode.Local;
                        formReportViewer.reportViewer1.LocalReport.DataSources.Clear();
                        formReportViewer.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                        var Report = new ReportParameter("Report", "สมาชิกเพิ่มหุ้น");
                        var Where = new ReportParameter("Where", "สมาชิกเพิ่มหุ้นวันที่ " + DtAddS.Value.ToString("dd/MM/yyyy") + " ถึง " + DtAddE.Value.ToString("dd/MM/yyyy") + " " + (TbMemberS.Text == "" ? "" : "ตามสมาชิก " + TbMemberS.Text + " - " + TbMemberE.Text + " ") + (TbNoMemberS.Text == "" ? "" : "ไม่ตามสมาชิก " + TbNoMemberS.Text + " ") + (CobRegistor.SelectedIndex == 1 ? "สถานะ : ร่วมหุ้น " : (CobRegistor.SelectedIndex == 2 ? "สถานะ : ถอนหุ้น " : "สถานะ : ทั้งหมด ")) + (CobSex.SelectedIndex == 1 ? "เพศ : ชาย" : CobSex.SelectedIndex == 2 ? "เพศ : หญิง" : "เพศ : ทั้งหมด"));
                        var Sort = new ReportParameter("Sort", (CbDelete.Checked == true ? "D" : "R"));

                        ReportParameter[] HeaderParams = { Report, Where, Sort };
                        formReportViewer.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.ReportMember2.rdlc";
                        foreach (ReportParameter param in HeaderParams)
                        {
                            formReportViewer.reportViewer1.LocalReport.SetParameters(param);
                        }
                        formReportViewer.reportViewer1.RefreshReport();
                        formReportViewer.Show();
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("ไม่พบข้อมูล", "เเจ้ง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var DsGetMember = Singleton.SingletonMember.Instance().Members.Where(w => w.Enable == true & (CbRegistor.Checked == true ? w.CreateDate >= DtRegistorS.Value & w.CreateDate <= DtRegistorE.Value : w.Enable == true)
                & (CobRegistor.SelectedIndex == 1 ? w.IsRemoveShared == false : (CobRegistor.SelectedIndex == 2 ? w.IsRemoveShared == true : w.Enable == true)) &
                (CbDelete.Checked == true ? w.ResignDate >= DtDeleteS.Value & w.ResignDate <= DtDeleteE.Value : w.Enable == true) &
                (CobSex.SelectedIndex == 1 ? w.Sex.Id == 2 : (CobSex.SelectedIndex == 2 ? w.Sex.Id == 3 : w.Enable == true)) &
                (TbMemberE.Text == "" ? w.Enable == true : Convert.ToDouble(w.Code) >= Convert.ToDouble(TbMemberS.Text.Trim()) & Convert.ToDouble(w.Code) <= Convert.ToDouble(TbMemberE.Text.Trim())) &
                (TbNoMemberS.Text == "" ? w.Enable == true : w.Code != TbNoMemberS.Text.Trim())).Select(s => new
                {
                    Id = s.Id,
                    MemberId = s.Code,
                    MemberName = s.Name,
                    RegistorDate = s.CreateDate,
                    s.Address,
                    s.TaxId,
                    Sex = s.Sex.Name,
                    Qty = s.MemberShare.Where(w => w.Enable == true).Sum(ss => ss.Qty),
                    Bath = s.MemberShare.Where(w => w.Enable == true).Sum(ss => ss.Qty * ss.Share.Value),
                    s.IsRemoveShared,
                    s.ResignDate,
                    s.ResignRemark
                }).OrderBy(o => o.MemberId).ToList();
                    if (DsGetMember.Count() > 0)
                    {
                        DataTable dt = Library.ConvertToDataTable(DsGetMember);

                        FormReportViewer formReportViewer = new FormReportViewer(this);
                        formReportViewer.reportViewer1.Reset();
                        formReportViewer.reportViewer1.ProcessingMode = ProcessingMode.Local;
                        formReportViewer.reportViewer1.LocalReport.DataSources.Clear();
                        formReportViewer.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
                        var Report = new ReportParameter("Report", (CbRegistor.Checked == true ? "สมัครสมาชิก" : (CbDelete.Checked == true ? "สมาชิกลาออก" : "ทั้งหมด")));
                        var Where = new ReportParameter("Where", (CbRegistor.Checked == true ? "สมัครสมาชิกวันที่ " + DtRegistorS.Value.ToString("dd/MM/yyyy") + " ถึง " + DtRegistorE.Value.ToString("dd/MM/yyyy") + " " : (CbDelete.Checked == true ? "สมาชิกลาออกวันที่ " + DtDeleteS.Value.ToString("dd/MM/yyyy") + " ถึง " + DtDeleteE.Value.ToString("dd/MM/yyyy") + " " : "")) + (TbMemberS.Text == "" ? "" : "ตามสมาชิก " + TbMemberS.Text + " - " + TbMemberE.Text + " ") + (TbNoMemberS.Text == "" ? "" : "ไม่ตามสมาชิก " + TbNoMemberS.Text + " ") + (CobRegistor.SelectedIndex == 1 ? "สถานะ : ร่วมหุ้น " : (CobRegistor.SelectedIndex == 2 ? "สถานะ : ถอนหุ้น " : "สถานะ : ทั้งหมด ")) + (CobSex.SelectedIndex == 1 ? "เพศ : ชาย" : CobSex.SelectedIndex == 2 ? "เพศ : หญิง" : "เพศ : ทั้งหมด"));
                        var Sort = new ReportParameter("Sort", (CbDelete.Checked == true ? "D" : "R"));

                        ReportParameter[] HeaderParams = { Report, Where, Sort };
                        formReportViewer.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.ReportMember1.rdlc";
                        foreach (ReportParameter param in HeaderParams)
                        {
                            formReportViewer.reportViewer1.LocalReport.SetParameters(param);
                        }
                        formReportViewer.reportViewer1.RefreshReport();
                        formReportViewer.Show();
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("ไม่พบข้อมูล", "เเจ้ง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception cx)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(cx.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        class MemberBuy
        {
            public string MemberId { get; set; }
            public decimal BuyTotal { get; set; }
            public string TaxId { get; set; }
            public string Sex { get; set; }
            public string MemberName { get; set; }
            public bool IsRemoveShared { get; set; }
        }
        class Get99
        {
            public int Qty { get; set; }
        }
        class MemberBuyList
        {
            public string MemberId { get; set; }
            public decimal BuyTotal { get; set; }
            public string TaxId { get; set; }
            public string Sex { get; set; }
            public string MemberName { get; set; }
            public decimal Qty { get; set; }
            public bool IsRemoveShared { get; set; }
        }

        private void BtBuyS_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                DataTable dt = new DataTable();

                List<MemberBuyList> MemberBuyListsGet = new List<MemberBuyList>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    string queryString = "select t.MemberId, BuyTotal,m.Name,m.TaxId,isnull(s.Name,'ชาย') Sex,isnull(m.IsRemoveShared,1) IsRemoveShared from ( select MEMBER_ID MemberId, sum(PRICE) BuyTotal from WH_TRAT.dbo.PS_ORDER o inner join WH_TRAT.dbo.PS_HD h on o.invoice_no = h.invoice_no inner join WH_TRAT.dbo.PS_DTL d on o.invoice_no = d.invoice_no where CANCEL_DATE is null and convert(varchar, h.create_date,112) >= '" + DtBuyS.Value.ToString("yyyyMMdd") + "' and convert(varchar, h.create_date,112) <= '" + DtBuyE.Value.ToString("yyyyMMdd") + "' " + (TbBuyMS.Text.Trim() == "" ? "" : "and convert(varchar, MEMBER_ID) >= '" + TbBuyMS.Text.Trim() + "' and convert(varchar, MEMBER_ID) <= '" + TbBuyME.Text.Trim() + "' ") + (TbBuyNM.Text.Trim() == "" ? "" : "and MEMBER_ID <> '" + TbBuyNM.Text.Trim() + "' ") + " group by MEMBER_ID )t left join mem.Member m on t.MemberId = m.Code and m.Enable = 1 inner join mem.Sex s on m.FKSex = s.Id ";
                    var getdata = db.Database.ExecuteEntities<MemberBuy>(queryString);
                    if (getdata.Count() > 0)
                    {
                        foreach (var item in getdata.ToList())
                        {
                            MemberBuyListsGet.Add(new MemberBuyList()
                            {
                                MemberId = item.MemberId,
                                MemberName = item.MemberName,
                                TaxId = item.TaxId,
                                Sex = item.Sex,
                                BuyTotal = item.BuyTotal
                            });

                        }
                    }

                }
                if (MemberBuyListsGet.Count() > 0)
                {
                    List<MemberBuyList> MemberBuyLists = new List<MemberBuyList>();
                    var DsGetMember = MemberBuyListsGet.Where(w => w.MemberId != null & (CombBuyS.SelectedIndex == 1 ? w.IsRemoveShared == false : (CombBuyS.SelectedIndex == 2 ? w.IsRemoveShared == true : w.MemberId != null)) &
                      (CombBuySex.SelectedIndex == 1 ? w.Sex == "ชาย" : (CombBuySex.SelectedIndex == 2 ? w.Sex == "หญิง" : w.MemberId != null))).ToList();
                    foreach (var item in DsGetMember.ToList())
                    {
                        if (item.MemberId == "99")
                        {

                            using (SSLsEntities db = new SSLsEntities())
                            {
                                string queryString = "select COUNT(h.INVOICE_NO) Qty from WH_TRAT.dbo.PS_ORDER o inner join WH_TRAT.dbo.PS_HD h on o.invoice_no = h.invoice_no where CANCEL_DATE is null and convert(varchar, h.create_date,112) >= '" + DtBuyS.Value.ToString("yyyyMMdd") + "' and convert(varchar, h.create_date,112) <= '" + DtBuyE.Value.ToString("yyyyMMdd") + "' and MEMBER_ID = '99'";
                                var getdata99 = db.Database.ExecuteEntities<Get99>(queryString);
                                MemberBuyLists.Add(new MemberBuyList()
                                {
                                    MemberId = item.MemberId,
                                    MemberName = item.MemberName,
                                    TaxId = item.TaxId,
                                    Sex = item.Sex,
                                    BuyTotal = item.BuyTotal,
                                    Qty = (getdata99 == null ? 0 : getdata99.Select(s => s.Qty).FirstOrDefault())
                                });
                            }
                        }
                        else
                        {
                            MemberBuyLists.Add(new MemberBuyList()
                            {
                                MemberId = item.MemberId,
                                MemberName = item.MemberName,
                                TaxId = item.TaxId,
                                Sex = item.Sex,
                                BuyTotal = item.BuyTotal,
                                Qty = 1
                            });
                        }
                    }
                    FormReportViewer formReportViewer = new FormReportViewer(this);
                    formReportViewer.reportViewer1.Reset();
                    formReportViewer.reportViewer1.ProcessingMode = ProcessingMode.Local;
                    formReportViewer.reportViewer1.LocalReport.DataSources.Clear();
                    formReportViewer.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", MemberBuyLists));
                    var Report = new ReportParameter("Report", "สมาชิกใช้บริการ");
                    var Where = new ReportParameter("Where", "วันที่ซื้อสินค้า " + DtBuyS.Value.ToString("dd/MM/yyyy") + " ถึง " + DtBuyE.Value.ToString("dd/MM/yyyy") + " " + (TbBuyMS.Text == "" ? "" : "ตามสมาชิก " + TbBuyMS.Text + " - " + TbBuyME.Text + " ") + (TbBuyNM.Text == "" ? "" : "ไม่ตามสมาชิก " + TbBuyNM.Text + " ") + (CobRegistor.SelectedIndex == 1 ? "สถานะ : ร่วมหุ้น " : (CobRegistor.SelectedIndex == 2 ? "สถานะ : ถอนหุ้น " : "สถานะ : ทั้งหมด ")) + (CobSex.SelectedIndex == 1 ? "เพศ : ชาย" : CobSex.SelectedIndex == 2 ? "เพศ : หญิง" : "เพศ : ทั้งหมด"));
                    ReportParameter[] HeaderParams = { Report, Where };
                    formReportViewer.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.ReportMemberBuy.rdlc";
                    foreach (ReportParameter param in HeaderParams)
                    {
                        formReportViewer.reportViewer1.LocalReport.SetParameters(param);
                    }
                    formReportViewer.reportViewer1.RefreshReport();
                    formReportViewer.Show();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("ไม่พบข้อมูล", "เเจ้ง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception cx)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(cx.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TbBuyMEKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }

        }

        private void TbBuyNMKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }

        }

        private void TbBuyMS_TextChanged(object sender, EventArgs e)
        {
            TbBuyME.Text = TbBuyMS.Text;
        }

        private void TbBuyMSKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                DataTable dt = new DataTable();

                List<MemberBuyList> MemberBuyListsGet = new List<MemberBuyList>();
                using (SSLsEntities db = new SSLsEntities())
                {
                    string queryString = "select t.MemberId, BuyTotal,m.Name,m.TaxId,isnull(s.Name,'ชาย') Sex,isnull(m.IsRemoveShared,1) IsRemoveShared from ( select MEMBER_ID MemberId, sum(PRICE) BuyTotal from WH_TRAT.dbo.PS_ORDER o inner join WH_TRAT.dbo.PS_HD h on o.invoice_no = h.invoice_no inner join WH_TRAT.dbo.PS_DTL d on o.invoice_no = d.invoice_no where CANCEL_DATE is null and convert(varchar, h.create_date,112) >= '" + DtBuyS.Value.ToString("yyyyMMdd") + "' and convert(varchar, h.create_date,112) <= '" + DtBuyE.Value.ToString("yyyyMMdd") + "' " + (TbBuyMS.Text.Trim() == "" ? "" : "and convert(varchar, MEMBER_ID) >= '" + TbBuyMS.Text.Trim() + "' and convert(varchar, MEMBER_ID) <= '" + TbBuyME.Text.Trim() + "' ") + (TbBuyNM.Text.Trim() == "" ? "" : "and MEMBER_ID <> '" + TbBuyNM.Text.Trim() + "' ") + " group by MEMBER_ID )t left join mem.Member m on t.MemberId = m.Code and m.Enable = 1 inner join mem.Sex s on m.FKSex = s.Id ";
                    var getdata = db.Database.ExecuteEntities<MemberBuy>(queryString);
                    if (getdata.Count() > 0)
                    {
                        foreach (var item in getdata.ToList())
                        {
                            MemberBuyListsGet.Add(new MemberBuyList()
                            {
                                MemberId = item.MemberId,
                                MemberName = item.MemberName,
                                TaxId = item.TaxId,
                                Sex = item.Sex,
                                BuyTotal = item.BuyTotal
                            });

                        }
                    }

                }
                if (MemberBuyListsGet.Count() > 0)
                {
                    List<MemberBuyList> MemberBuyLists = new List<MemberBuyList>();
                    var DsGetMember = MemberBuyListsGet.Where(w => w.MemberId != null & (CombBuyS.SelectedIndex == 1 ? w.IsRemoveShared == false : (CombBuyS.SelectedIndex == 2 ? w.IsRemoveShared == true : w.MemberId != null)) &
                      (CombBuySex.SelectedIndex == 1 ? w.Sex == "ชาย" : (CombBuySex.SelectedIndex == 2 ? w.Sex == "หญิง" : w.MemberId != null))).ToList();
                    foreach (var item in DsGetMember.ToList())
                    {
                        if (item.MemberId == "99")
                        {

                            using (SSLsEntities db = new SSLsEntities())
                            {
                                string queryString = "select COUNT(h.INVOICE_NO) Qty from WH_TRAT.dbo.PS_ORDER o inner join WH_TRAT.dbo.PS_HD h on o.invoice_no = h.invoice_no where CANCEL_DATE is null and convert(varchar, h.create_date,112) >= '" + DtBuyS.Value.ToString("yyyyMMdd") + "' and convert(varchar, h.create_date,112) <= '" + DtBuyE.Value.ToString("yyyyMMdd") + "' and MEMBER_ID = '99'";
                                var getdata99 = db.Database.ExecuteEntities<Get99>(queryString);
                                MemberBuyLists.Add(new MemberBuyList()
                                {
                                    MemberName = "สมาชิก",
                                    BuyTotal = item.BuyTotal,
                                    Qty = (getdata99 == null ? 0 : getdata99.Select(s => s.Qty).FirstOrDefault())
                                });
                            }
                        }
                        else
                        {
                            MemberBuyLists.Add(new MemberBuyList()
                            {

                                MemberName = "ลูกค้าทั่วไป",
                                BuyTotal = item.BuyTotal,
                                Qty = 1
                            });
                        }
                    }
                    var abc = MemberBuyLists.GroupBy(b => b.MemberName).Select(s => new
                    {
                        MemberName = s.Key,
                        BuyTotal = s.Sum(ss => ss.BuyTotal),
                        Qty = s.Sum(ss => ss.Qty)
                    }).ToList();

                    FormReportViewer formReportViewer = new FormReportViewer(this);
                    formReportViewer.reportViewer1.Reset();
                    formReportViewer.reportViewer1.ProcessingMode = ProcessingMode.Local;
                    formReportViewer.reportViewer1.LocalReport.DataSources.Clear();
                    formReportViewer.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", abc));
                    var Report = new ReportParameter("Report", "สมาชิกใช้บริการเเบบสรุป");
                    var Where = new ReportParameter("Where", "วันที่ซื้อสินค้า " + DtBuyS.Value.ToString("dd/MM/yyyy") + " ถึง " + DtBuyE.Value.ToString("dd/MM/yyyy") + " " + (TbBuyMS.Text == "" ? "" : "ตามสมาชิก " + TbBuyMS.Text + " - " + TbBuyME.Text + " ") + (TbBuyNM.Text == "" ? "" : "ไม่ตามสมาชิก " + TbBuyNM.Text + " ") + (CobRegistor.SelectedIndex == 1 ? "สถานะ : ร่วมหุ้น " : (CobRegistor.SelectedIndex == 2 ? "สถานะ : ถอนหุ้น " : "สถานะ : ทั้งหมด ")) + (CobSex.SelectedIndex == 1 ? "เพศ : ชาย" : CobSex.SelectedIndex == 2 ? "เพศ : หญิง" : "เพศ : ทั้งหมด"));
                    ReportParameter[] HeaderParams = { Report, Where };
                    formReportViewer.reportViewer1.LocalReport.ReportEmbeddedResource = "Pos_Management_System.ReportMemberBuy2.rdlc";
                    foreach (ReportParameter param in HeaderParams)
                    {
                        formReportViewer.reportViewer1.LocalReport.SetParameters(param);
                    }
                    formReportViewer.reportViewer1.RefreshReport();
                    formReportViewer.Show();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("ไม่พบข้อมูล", "เเจ้ง", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception cx)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(cx.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
