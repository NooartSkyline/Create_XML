using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Create_XML
{
    public partial class Form_Create_xml : Form
    {
        POS_HDDTDataContext pos = new POS_HDDTDataContext();
        DT_RTI001_POSDW_TRANSACTION_INT model_DT_INT;
        DT_RTI001_POSDW_RETAILLINEITEM model_RETAILLINEITEM;
        DT_RTI001_POSDW_TAX model_TAX;
        DT_RTI001_POSDW_TENDER model_TENDER;
        public Form_Create_xml()
        {
            InitializeComponent();
        }

        private void Btn_create_Click(object sender, EventArgs e)
        {
            var doc_sale = new DT_RTI001_SALESTRANSACTION();

            Dictionary<string, object> get_dic = new Dictionary<string, object>();
            string getdoc = txt_docno.Text;
            if (getdoc.Count() > 0)
            {
                get_dic = Pos_HDDT(getdoc);
            }
            else
            {
                MessageBox.Show("กรุณาใส่เลขที่เอกสาร");
            }
            doc_sale.POSDW_TRANSACTION_INT = new DT_RTI001_POSDW_TRANSACTION_INT[1];
            model_DT_INT = new DT_RTI001_POSDW_TRANSACTION_INT();
            TBTrans_POS_HD mhd = (TBTrans_POS_HD)get_dic["hd"];

            model_DT_INT.RETAILSTOREID = mhd.SITECODE;
            model_DT_INT.BUSINESSDAYDATE = mhd.DOCDATE.ToString();
            model_DT_INT.TRANSACTIONTYPECODE = "Z001";
            model_DT_INT.WORKSTATIONID = mhd.POSCODE;//mhd.REF_MACHINENO;
            model_DT_INT.TRANSACTIONSEQUENCENUMBER = mhd.DOCNO;
            model_DT_INT.BEGINDATETIMESTAMP = mhd.CREATEDATE.ToString("yyyyMMddHHmmss");
            model_DT_INT.ENDDATETIMESTAMP = mhd.CREATEDATE.ToString("yyyyMMddHHmmss");
            model_DT_INT.OPERATORID = mhd.CASHIER;//mhd.createuser;
            model_DT_INT.TRANSACTIONCURRENCY = "THB";
            model_DT_INT.PARTNERID = mhd.ARCODE;


            List<TBTrans_POS_DT> list_dt = (List<TBTrans_POS_DT>)get_dic["dt"];
            DT_RTI001_POSDW_RETAILLINEITEM[] arraydt = new DT_RTI001_POSDW_RETAILLINEITEM[list_dt.Count];

            int i = 0;
            foreach (TBTrans_POS_DT item in list_dt)
            {
                model_RETAILLINEITEM = new DT_RTI001_POSDW_RETAILLINEITEM();
                model_RETAILLINEITEM.RETAILSEQUENCENUMBER = (i + 1).ToString();
                model_RETAILLINEITEM.RETAILTYPECODE = "ZWSL";
                model_RETAILLINEITEM.ITEMIDQUALIFIER = "1";
                model_RETAILLINEITEM.ITEMID = item.BARCODE;
                model_RETAILLINEITEM.RETAILQUANTITY = Math.Round(item.QUANTITY,2).ToString(); ;
                model_RETAILLINEITEM.SALESUNITOFMEASURE = item.UNITCODE;
                model_RETAILLINEITEM.SALESAMOUNT = Math.Round(item.NETAMOUNT,2).ToString();
                model_RETAILLINEITEM.NORMALSALESAMOUNT = Math.Round(item.NETAMOUNT,2).ToString();
                model_RETAILLINEITEM.COST = "0";
                model_RETAILLINEITEM.ACTUALUNITPRICE = Math.Round(item.UNITPRICE,2).ToString();
                model_RETAILLINEITEM.UNITS = "0";

                DT_RTI001_POSDW_TAX[] array_tax = new DT_RTI001_POSDW_TAX[1];
                model_TAX = new DT_RTI001_POSDW_TAX();
                model_TAX.TAXSEQUENCENUMBER = "0001";
                model_TAX.TAXTYPECODE = "1001";
                model_TAX.TAXAMOUNT = Math.Round(item.TAXVALUE, 2).ToString(); ;// TAXVALUE จาก DT ทศนิยม 2 ตำแหน่ง
                array_tax[0] = model_TAX;
                model_RETAILLINEITEM.TAX = array_tax;
                arraydt[i] = model_RETAILLINEITEM;
                i++;
            }
            model_DT_INT.RETAILLINEITEM = arraydt;

            //TENDER
            DT_RTI001_POSDW_TENDER[] array_tender = new DT_RTI001_POSDW_TENDER[1];
            model_TENDER = new DT_RTI001_POSDW_TENDER();
            model_TENDER.TENDERSEQUENCENUMBER = "1";
            model_TENDER.TENDERTYPECODE = "CASH";
            decimal amount = mhd.CASHAMOUNT - mhd.CHANGEAMOUNT;
            model_TENDER.TENDERAMOUNT = Math.Round(amount,2).ToString(); ;//TBTrans_POS_HD-CASHAMOUNT ลบ TBTrans_POS_HD-CHANGEAMOUNT
            model_TENDER.TENDERCURRENCY = "THB";
            model_TENDER.TENDERID = "0";

            array_tender[0] = model_TENDER;
            model_DT_INT.TENDER = array_tender;
            doc_sale.POSDW_TRANSACTION_INT[0] = model_DT_INT;

            //Check folder temp ว่ามีหรือไม่ถ้าไม่มีให้สร้างก่อน
            string sTempFolder = @"C:\Temp\" + DateTime.Today.ToString("yyyyMMdd", new CultureInfo("en-US"));
            if (!Directory.Exists(sTempFolder))
                Directory.CreateDirectory(sTempFolder);
            string sPathFileName = sTempFolder + @"\Sales_" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";

            // Create File
            var serializer = new XmlSerializer(typeof(DT_RTI001_SALESTRANSACTION));
            //var xmlFilename = doCreateFileName(sSiteCode);
            using (var stream = new StreamWriter(sPathFileName)) // -- doc_sale.Save(sPathFileName);
                serializer.Serialize(stream, doc_sale);
        }
        private Dictionary<string,object> Pos_HDDT(string docno){

            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                pos.Connection.Open();
                TBTrans_POS_HD linq_hd = (from hd in pos.TBTrans_POS_HDs
                                          where hd.DOCNO == docno
                                          select hd).SingleOrDefault();

                List<TBTrans_POS_DT> linq_dt = (from dt in pos.TBTrans_POS_DTs
                               where dt.DOCNO == docno
                               select dt).ToList();

                pos.Connection.Close();
                dic.Add("hd", linq_hd);
                dic.Add("dt", linq_dt);
                dic.Add("msg", "");
            }
            catch (Exception ex)
            {
                dic.Add("hd", "");
                dic.Add("dt", "");
                dic.Add("msg", ex);
            }     
            return dic;
        }     

    }

}
