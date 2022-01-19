using MemberListGifts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemberListGifts
{
    public partial class Form1 : Form
    {
        private FEDSMBREntities _fdb = new FEDSMBREntities();
        private GiftsEntities _gdb = new GiftsEntities();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime _today = DateTime.Now;

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
                string folderPath = directoryInfo.FullName + "GiftsInfo.txt";

                using (StreamReader sr = new StreamReader(folderPath))
                {
                    string line = sr.ReadToEnd();
                    string[] _list = line.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    int _year = Convert.ToInt32(_list[0]);
                    int _month = Convert.ToInt32(_list[1]);
                    int _gid = Convert.ToInt32(_list[2]);

                    DateTime _firstdate = new DateTime(_year, _month, 1);
                    DateTime _enddate = _firstdate.AddMonths(1).AddSeconds(-1);

                    //已匯入名單
                    List<int> entityPresentMids = _gdb.EntityPresent.Where(o => o.GId.Value == _gid).Select(o => o.MemberId).ToList().ConvertAll(c => (int)Convert.ToDouble(c));
                    //當月壽星
                    var mbrs = _fdb.Member.Where(o => o.RealValidDate != null && o.Status == 1 && o.Birthday != null).ToList();
                    List<int> mbrMids = new List<int>();
                    foreach (var b in mbrs)
                    {
                        string _mbd = b.Birthday.ToString();
                        if (!string.IsNullOrEmpty(_mbd))
                        {
                            DateTime _bd = Convert.ToDateTime(_mbd);
                            if(_bd.Month == _month)
                                mbrMids.Add(b.Id);
                        }
                    }

                    //尚未匯入名單
                    List<int> _NewMB = mbrMids.Except(entityPresentMids).ToList();

                    using (var _GiftsDb = new GiftsEntities())
                    {
                        for (int i = 2; i < _list.Length; i++)
                        {
                            int _g = Convert.ToInt32(_list[i]);
                            string _gNo = _GiftsDb.Gifts.Where(o => o.Id == _g).Select(o => o.GiftsNo).FirstOrDefault();
                            UsedRule usedRule = _GiftsDb.UsedRule.Where(o => o.GId == _g).FirstOrDefault();
                            if (usedRule.StartNo == null)
                            {
                                usedRule.StartNo = 1;
                                _GiftsDb.Entry(usedRule).State= EntityState.Modified;
                                _GiftsDb.SaveChanges();
                            }

                            int _startNo = usedRule.StartNo.Value;

                            foreach (var item in _NewMB)
                            {                                
                                string _couponNo = "10" + _gNo + _startNo.ToString().PadLeft(7, '0');

                                _GiftsDb.EntityPresent.Add(new EntityPresent
                                {
                                    MemberId = item.ToString(),
                                    MallId = "",
                                    GId = _g,
                                    CouponNo = _couponNo,
                                    Status = "N",
                                    UsedStart = _firstdate,
                                    UsedEnd = _enddate,
                                    CreateOn = _today
                                });

                                //_GiftsDb.EntityPresent_Log.Add(new EntityPresent_Log
                                //{
                                //    MemberId = item.ToString(),
                                //    MallId = "",
                                //    GId = _g,
                                //    CouponNo = _couponNo,
                                //    Status = "N",
                                //    CreateOn = _today,
                                //    Remark = "發送生日禮"
                                //});

                                _startNo = _startNo + 1;
                            }
                            usedRule.StartNo = _startNo;
                            _GiftsDb.Entry(usedRule).State = EntityState.Modified;
                            _GiftsDb.SaveChanges();
                        }

                        _GiftsDb.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _gdb.ExchangeLog.Add(new ExchangeLog
                {
                    PageName = "MemberListGifts",
                    Event = ex.StackTrace,
                    ModifyTime = _today
                });
                //_gdb.SystemErrorLog.Add(new SystemErrorLog
                //{
                //    PageName = "MemberListGifts",
                //    Event = ex.StackTrace,
                //    ModifyTime = _today
                //});
                _gdb.SaveChanges();
            }

            Close_Winform();
        }

        //關閉Winform
        protected void Close_Winform()
        {
            Environment.Exit(Environment.ExitCode);
            this.Close();
        }
    }
}
