using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SQLite;

namespace Persian.Calender
{
    public partial class Form1 : Form
    {
        Graphics g = null;
        public Form1()
        {
            InitializeComponent();

            timer1.Enabled = true;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("آیا برای خروج مطمئن هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == DialogResult.Yes)
                Close();
        }
        string PersianDigits(string s)
        {
            return s.Replace("0", "۰")
                    .Replace("1", "۱")
                    .Replace("2", "۲")
                    .Replace("3", "۳")
                    .Replace("4", "۴")
                    .Replace("5", "۵")
                    .Replace("6", "۶")
                    .Replace("7", "۷")
                    .Replace("8", "۸")
                    .Replace("9", "۹");
        }
        string ConvertToShamsiDayOfWeek(DayOfWeek dw)
        {
            switch (dw)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
            }
            return "";
        }

        private void btnShow_Click(object sender, EventArgs e)
        {

            richTextBox1.Clear();
            richTextBox2.Clear();
            //Add month to combo box
            string connectionString = @"Data Source=data\Vacation.db";
            SQLiteConnection con = new SQLiteConnection(connectionString);

            SQLiteCommand cmd = new SQLiteCommand("Select month,day,text from Vacation where month like'%" + textMonth.Text + "%'and day like '%" + textDay.Text + "%'", con);


            try
            {
                con.Open();

                SQLiteDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    richTextBox1.Text = "";
                    while (dr.Read())
                    {
                        richTextBox2.Text += dr[1].ToString() + " / " + dr[0].ToString() + " - " + dr[2].ToString() + "\n";
                    }
                }
                else
                    MessageBox.Show("not found");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
            PersianCalendar pc = new PersianCalendar();
            DateTime curDate = pc.ToDateTime(1400, 1, 1, 0, 0, 0, 0);



            this.Cursor = Cursors.WaitCursor;
            for (int counter = 0; counter < 53; counter++)
            {
                int i = 0;

                while (i < 7)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(curDate);
                    string str = ConvertToShamsiComplete(curDate, pc);
                    richTextBox1.Text += ConvertToShamsiDayOfWeek(dw) + "-" + PersianDigits(str);
                    curDate = curDate.AddDays(1);
                    i++;
                }

                richTextBox1.Text += ("***************************\n");
            }

            this.Cursor = Cursors.Default;
        }

        private string ConvertToShamsiComplete(DateTime curDate, PersianCalendar pc)
        {
            return string.Format("{0}/{1}/{2}\n", pc.GetYear(curDate), pc.GetMonth(curDate), pc.GetDayOfMonth(curDate));
        }
        private string ConvertTOShamsiComplete2(DateTime curDate, PersianCalendar pc)
        {
            return string.Format("{0}/{1}", pc.GetMonth(curDate), pc.GetDayOfMonth(curDate));
        }

        private string ConvertTOShamsiComplete3(DateTime curDate, PersianCalendar pc)
        {
            return string.Format("{0}", pc.GetDayOfMonth(curDate));
        }

        private void tabPage2_Paint(object sender, PaintEventArgs e)
        { 
            PersianCalendar pc = new PersianCalendar();

            DateTime dt = pc.ToDateTime(1399, 12, 30, 0, 0, 0, 0);

            Graphics g = e.Graphics;

            //Draw Winter table

            g.FillRectangle(Brushes.CornflowerBlue, 120, 10, 25, 20);
            g.DrawString("دی", DefaultFont, Brushes.Black, 120, 10);

            g.FillRectangle(Brushes.CornflowerBlue, 120, 260, 40, 20);
            g.DrawString("بهمن", DefaultFont, Brushes.Black, 120, 260);

            g.FillRectangle(Brushes.CornflowerBlue, 120, 510, 50, 20);
            g.DrawString("اسفند", DefaultFont, Brushes.Black, 120, 510);

            //Draw table of Winter
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    g.FillRectangle(Brushes.CornflowerBlue, j * 250 + 30, i * 250 + 30, 220, 180);
                }
            }

            int a = 30;

            //Marking holiday

            g.FillRectangle(Brushes.Orange, 90, 310, 15, 15);
            g.FillRectangle(Brushes.Orange, 90, 610, 15, 15);
            g.FillRectangle(Brushes.Orange, 90, 30, 15, 15);
            g.FillRectangle(Brushes.Orange, 120, 30, 15, 15);
            g.FillRectangle(Brushes.Orange, 120, 155, 15, 15);
            g.FillRectangle(Brushes.Orange, 120, 435, 15, 15);
            g.FillRectangle(Brushes.Orange, 120, 155, 15, 15);
            g.FillRectangle(Brushes.Orange, 120, 560, 15, 15);
            g.FillRectangle(Brushes.Orange, 154, 360, 15, 15);
            g.FillRectangle(Brushes.Orange, 154, 685, 15, 15);
            g.FillRectangle(Brushes.Orange, 189, 560, 15, 15);

            //Day of dey

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 30, a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Day of bahman

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 30, 80 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Day of esfand

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 30, 155 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Number of Days for Dey
            
            int h = 90;
            
            dt = pc.ToDateTime(1400, 10, 1, 0, 0, 0, 0);
            
            for (int i = 0; i < 3; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f1(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }
            
            h += 35;
            
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f1(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 35;
            }
            
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f1(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }

            //Number of days for bahman

            h = 90;
            
            dt = pc.ToDateTime(1400, 11, 2, 0, 0, 0, 0);
            
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f2(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }

            for (int i = 0; i < 1; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f2(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }

            //Number of days for Esfand

            h = 90;

            dt = pc.ToDateTime(1400, 12, 1, 0, 0, 0, 0);

            for (int i = 0; i < 6; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f3(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }

            h += 33;

            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f3(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }

            for (int i = 0; i < 2; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f3(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }

            //Draw FALL table

            g.FillRectangle(Brushes.Orange, 380, 10, 20, 20);
            g.DrawString("مهر", DefaultFont, Brushes.Black, 380, 10);

            g.FillRectangle(Brushes.Orange, 380, 260, 40, 20);
            g.DrawString("آبان", DefaultFont, Brushes.Black, 380, 260);

            g.FillRectangle(Brushes.Orange, 380, 510, 55, 20);
            g.DrawString("آذر", DefaultFont, Brushes.Black, 380, 510);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    g.FillRectangle(Brushes.Orange, j * 250 + 30, i * 250 + 30, 220, 180);
                }
            }

            dt = pc.ToDateTime(1399, 12, 30, 0, 0, 0, 0);

            a = 30;

            //Marking holiday

            g.FillRectangle(Brushes.OrangeRed, 623, 80, 15, 15);
            g.FillRectangle(Brushes.OrangeRed, 656, 105, 15, 15);
            g.FillRectangle(Brushes.OrangeRed, 656, 155, 15, 15);
            g.FillRectangle(Brushes.OrangeRed, 590, 310, 15, 15);
            g.FillRectangle(Brushes.OrangeRed, 722, 610, 15, 15);

            //days of mehr

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 280, a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Days of aban

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 280, 80 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Days of azar

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 280, 155 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Number of days for Mehr

            h = 340;

            dt = pc.ToDateTime(1400, 7, 1, 0, 0, 0, 0);

            for (int i = 0; i < 2; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f1(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }
            h += 33;
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f1(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }

            //Number of days for Abban

            h = 340;

            dt = pc.ToDateTime(1400, 8, 1, 0, 0, 0, 0);

            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f2(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }
            for (int i = 0; i < 2; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f2(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }


            //Number of days for Azar

            h = 340;

            dt = pc.ToDateTime(1400, 9, 1, 0, 0, 0, 0);
            for (int i = 0; i < 5; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f3(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }
            h += 33;
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f3(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }
            for (int i = 0; i < 4; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f3(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }
            //Draw summer table

            g.FillRectangle(Brushes.OrangeRed, 620, 10, 28, 20);
            g.DrawString("تیر", DefaultFont, Brushes.Black, 620, 10);

            g.FillRectangle(Brushes.OrangeRed, 620, 260, 40, 20);
            g.DrawString("مرداد", DefaultFont, Brushes.Black, 620, 260);

            g.FillRectangle(Brushes.OrangeRed, 620, 510, 30, 20);
            g.DrawString("شهریور", DefaultFont, Brushes.Black, 620, 510);

            //Draw table 

            for (int i = 0; i < 3; i++)
            {
                for (int j = 2; j < 3; j++)
                {
                    g.FillRectangle(Brushes.OrangeRed, j * 250 + 30, i * 250 + 30, 220, 180);
                }
            }

            dt = pc.ToDateTime(1399, 12, 30, 0, 0, 0, 0);

            a = 30;

            //Marking holiday

            g.FillRectangle(Brushes.GreenYellow, 472, 130, 15, 15);
            g.FillRectangle(Brushes.GreenYellow, 340, 410, 15, 15);
            g.FillRectangle(Brushes.GreenYellow, 439, 385, 15, 15);
            g.FillRectangle(Brushes.GreenYellow, 439, 410, 15, 15);

            //Days of Mehr

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 530, a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Days of Aban

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 530, 80 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Days of Azar

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 530, 155 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Number of days for Tir

            h = 590;

            dt = pc.ToDateTime(1400, 4, 1, 0, 0, 0, 0);

            for (int i = 0; i < 4; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f1(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }
            h += 33;
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f1(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }


            //number of days for Mordad

            h = 590;

            dt = pc.ToDateTime(1400, 5, 2, 0, 0, 0, 0);

            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f2(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }
            for (int i = 0; i < 2; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f2(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }

            //number of days for Shahrivar

            h = 590;

            dt = pc.ToDateTime(1400, 6, 1, 0, 0, 0, 0);

            for (int i = 0; i < 5; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f3(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }
            h += 33;
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f3(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }
            for (int i = 0; i < 5; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f3(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }



            //Draw spring table

            //Write month name on top of table

            g.FillRectangle(Brushes.LawnGreen, 850, 10, 55, 20);
            g.DrawString("فروردین", DefaultFont, Brushes.Black, 850, 10);

            g.FillRectangle(Brushes.LawnGreen, 850, 260, 68, 20);
            g.DrawString("اردیبهشت", DefaultFont, Brushes.Black, 850, 260);

            g.FillRectangle(Brushes.LawnGreen, 850, 510, 40, 20);
            g.DrawString("خرداد", DefaultFont, Brushes.Black, 850, 510);

            //Draw table for spring month

            for (int i = 0; i < 3; i++)
            {
                for (int j = 3; j < 4; j++)
                {
                    g.FillRectangle(Brushes.LawnGreen, j * 250 + 30, i * 250 + 30, 220, 180);
                }
            }

            dt = pc.ToDateTime(1399, 12, 30, 0, 0, 0, 0);

            a = 30;

            //Marking holiday

            g.FillRectangle(Brushes.Red, 835, 55, 15, 15);
            g.FillRectangle(Brushes.Red, 835, 80, 15, 15);
            g.FillRectangle(Brushes.Red, 835, 105, 15, 15);
            g.FillRectangle(Brushes.Red, 835, 130, 15, 15);
            g.FillRectangle(Brushes.Red, 835, 155, 15, 15);
            g.FillRectangle(Brushes.Red, 835, 180, 15, 15);
            g.FillRectangle(Brushes.Red, 865, 80, 15, 15);
            g.FillRectangle(Brushes.Red, 865, 155, 15, 15);
            g.FillRectangle(Brushes.Red, 835, 685, 15, 15);
            g.FillRectangle(Brushes.Red, 865, 180, 15, 15);
            g.FillRectangle(Brushes.Red, 898, 285, 15, 15);
            g.FillRectangle(Brushes.Red, 898, 310, 15, 15);
            g.FillRectangle(Brushes.Red, 898, 335, 15, 15);
            g.FillRectangle(Brushes.Red, 898, 360, 15, 15);
            g.FillRectangle(Brushes.Red, 898, 385, 15, 15);
            g.FillRectangle(Brushes.Red, 898, 535, 15, 15);
            g.FillRectangle(Brushes.Red, 931, 535, 15, 15);
            g.FillRectangle(Brushes.Red, 931, 410, 15, 15);
            
            //Days of farvardin

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 780, a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Days of ordibehesht

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 780, 80 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            //Days of khordad

            for (int j = 0; j < 7; j++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                g.DrawString(ConvertToShamsiDayOfWeek(dw), DefaultFont, Brushes.Black, 780, 155 + a);
                dt = dt.AddDays(1);
                a += 25;
            }

            

            h = 840;
            dt = pc.ToDateTime(1400, 1, 1, 0, 0, 0, 0);

            //Number of days for farvardin

            for (int i = 0; i < 6; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f1(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }

            h += 30;

            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f1(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 30;
            }

            for (int i = 0; i < 4; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f1(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);
            }
            h += 30;


            //Number of days for ordibehesht

            h = 840;
            
            dt = pc.ToDateTime(1400, 2, 1, 0, 0, 0, 0);

            for (int i = 0; i < 3; i++)
            {
                DayOfWeek dw = pc.GetDayOfWeek(dt);
                string str2 = ConvertTOShamsiComplete3(dt, pc);
                int test = f2(dw);
                g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                dt = dt.AddDays(1);

            }
           
            h +=30;
            
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f2(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                h += 33;
            }

            //Number of days for khordad

            h = 840;

            dt = pc.ToDateTime(1400, 3, 1, 0, 0, 0, 0);

            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    DayOfWeek dw = pc.GetDayOfWeek(dt);
                    string str2 = ConvertTOShamsiComplete3(dt, pc);
                    int test = f3(dw);
                    g.DrawString(PersianDigits(str2), DefaultFont, Brushes.Black, h, test);
                    dt = dt.AddDays(1);
                }
                g.DrawString(PersianDigits("29"), DefaultFont, Brushes.Black, 222, 535);
                g.DrawString(PersianDigits("30"), DefaultFont, Brushes.Black, 222, 560);
                g.DrawString(PersianDigits("31"), DefaultFont, Brushes.Black, 222, 585);
                h += 33;
            }
        }

        //Width positioning function

        //first line table
        private int f1(DayOfWeek dw)
        {
            if (ConvertToShamsiDayOfWeek(dw) == "شنبه")
            {
                return 30;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "یکشنبه")
            {
                return 55;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "دوشنبه")
            {
                return 80;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "سه شنبه")
            {
                return 105;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "چهارشنبه")
            {
                return 130;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "پنج شنبه")
            {
                return 155;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "جمعه")
            {
                return 180;
            }
            return 0;

        }

        //second line table

        private int f2(DayOfWeek dw)
        {
            if (ConvertToShamsiDayOfWeek(dw) == "شنبه")
            {
                return 285;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "یکشنبه")
            {
                return 310;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "دوشنبه")
            {
                return 335;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "سه شنبه")
            {
                return 360;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "چهارشنبه")
            {
                return 385;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "پنج شنبه")
            {
                return 410;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "جمعه")
            {
                return 435;
            }
            return 0;
        }

        //third line table

        private int f3(DayOfWeek dw)
        {
            if (ConvertToShamsiDayOfWeek(dw) == "شنبه")
            {
                return 535;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "یکشنبه")
            {
                return 560;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "دوشنبه")
            {
                return 585;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "سه شنبه")
            {
                return 610;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "چهارشنبه")
            {
                return 635;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "پنج شنبه")
            {
                return 660;
            }
            else if (ConvertToShamsiDayOfWeek(dw) == "جمعه")
            {
                return 685;
            }
            return 0;

        }

        //Put time and date in two text box for third page

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime curDate = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            int y = pc.GetYear(curDate);
            int m = pc.GetMonth(curDate);
            int d = pc.GetDayOfMonth(curDate);
            textBox1.Text = string.Format($"{y}/{m}/{d}");

            //How to give local time to computer and show in textBox2

            int hour = curDate.Hour;
            int minute = curDate.Minute;
            int second = curDate.Second;

            textBox2.Text = string.Format($"{hour}:{minute}:{second}");

            drawClock(hour, minute, second);
        }

        //Draw clock in picture box of third page

        private void drawClock(int h, int m, int s)
        {

            Graphics g = pictureBox1.CreateGraphics();

            //second

            int rs = (pictureBox1.Width - 60) / 2;

            int degSec = -s * 6 + 90;
            double radSec = degSec * Math.PI / 180.0;

            int xs = (int)(rs * Math.Cos(radSec));
            int ys = -(int)(rs * Math.Sin(radSec));

            //minute

            int rm = (pictureBox1.Width - 80) / 2;

            //MIN
            double degMin = -m * 6 + 94;

            double radMin = degMin * Math.PI / 180.0;

            int xm = (int)(rm * Math.Cos(radMin));
            int ym = -(int)(rm * Math.Sin(radMin));

            //MIN1
            double degMin1 = -m * 6 + 90;

            double radMin1 = degMin1 * Math.PI / 180.0;

            int xm1 = (int)((rm + 5) * Math.Cos(radMin1));
            int ym1 = -(int)((rm + 5) * Math.Sin(radMin1));
            //MIN2
            double degMin2 = -m * 6 + 86;

            double radMin2 = degMin2 * Math.PI / 180.0;

            int xm2 = (int)(rm * Math.Cos(radMin2));
            int ym2 = -(int)(rm * Math.Sin(radMin2));

            //hour
            int rh = (pictureBox1.Width - 130) / 2;
            //HOUR1
            double degHour1 = 0;
            if (h <= 12 && h >= 0)
            {
                degHour1 = -h * 30 + 95;
            }
            else if (h > 12 && h < 23)
            {
                int i = h - 12;
                degHour1 = -i * 30 + 95;
            }
            double radHour1 = degHour1 * Math.PI / 180.0;

            int xh1 = (int)(rh * Math.Cos(radHour1));
            int yh1 = -(int)(rh * Math.Sin(radHour1));

            //HOUR2
            double degHour2 = 0;
            if (h <= 12 && h >= 0)
            {
                degHour2 = -h * 30 + 85;
            }
            else if (h > 12 && h < 23)
            {
                int i = h - 12;
                degHour2 = -i * 30 + 85;
            }
            double radHour2 = degHour2 * Math.PI / 180.0;

            int xh2 = (int)(rh * Math.Cos(radHour2));
            int yh2 = -(int)(rh * Math.Sin(radHour2));
            //HOUR
            double degHour = 0;
            if (h <= 12 && h >= 0)
            {
                degHour = -h * 30 + 90;
            }
            else if (h > 12 && h < 23)
            {
                int i = h - 12;
                degHour = -i * 30 + 90;
            }
            double radHour = degHour * Math.PI / 180.0;

            int xh = (int)((rh + 10) * Math.Cos(radHour));
            int yh = -(int)((rh + 10) * Math.Sin(radHour));
            DrawCircle(g, rs);
            DrawScreen(g, xh, yh, xh1, yh1, xh2, yh2, xm, ym, xm1, ym1, xm2, ym2, xs, ys);
        }

        //Draw clockwise

        private void DrawScreen(Graphics g, int xh, int yh, int xh1, int yh1, int xh2, int yh2, int xm, int ym, int xm1, int ym1, int xm2, int ym2, int xs, int ys)
        {
            // g.Clear(Color.White);
            int offsetX = pictureBox1.Width / 2;
            int offsetY = pictureBox1.Height / 2;

            //second
            g.DrawLine(Pens.Red, offsetX, offsetY, xs + offsetX, ys + offsetY);

            //minute
            g.DrawLine(Pens.Black, offsetX, offsetY, xm + offsetX, ym + offsetY);
            g.DrawLine(Pens.Black, offsetX, offsetY, xm1 + offsetX, ym1 + offsetY);
            g.DrawLine(Pens.Black, offsetX, offsetY, xm2 + offsetX, ym2 + offsetY);
            g.DrawLine(Pens.Black, xm + offsetX, ym + offsetY, xm1 + offsetX, ym1 + offsetY);
            g.DrawLine(Pens.Black, xm2 + offsetX, ym2 + offsetY, xm1 + offsetX, ym1 + offsetY);

            //hour
            g.DrawLine(Pens.Black, offsetX, offsetY, xh + offsetX, yh + offsetY);
            g.DrawLine(Pens.Black, offsetX, offsetY, xh1 + offsetX, yh1 + offsetY);
            g.DrawLine(Pens.Black, offsetX, offsetY, xh2 + offsetX, yh2 + offsetY);
            g.DrawLine(Pens.Black, xh1 + offsetX, yh1 + offsetY, xh + offsetX, yh + offsetY);
            g.DrawLine(Pens.Black, xh2 + offsetX, yh2 + offsetY, xh + offsetX, yh + offsetY);

        }

        //Draw clock circle box and number of the clock

        private void DrawCircle(Graphics g, int rs)
        {
            //offSet
            int offsetX = pictureBox1.Width / 2;
            int offsetY = pictureBox1.Height / 2;

            //circle
            g.FillEllipse(Brushes.CornflowerBlue, 20, 20, 475, 475);
            g.DrawEllipse(Pens.Black, 20, 20, 475, 475);

            //g.Clear(Color.White);

            //number of clock 
            //number 1
            double I = -60 * Math.PI / 180.0;
            int Ix = (int)((rs + 10) * Math.Cos(I));
            int Iy = (int)((rs + 10) * Math.Sin(I));
            g.DrawString("1", DefaultFont, Brushes.Black, Ix + offsetX, Iy + offsetY);

            //number 2
            double II = -30 * Math.PI / 180.0;
            int IIx = (int)((rs + 10) * Math.Cos(II));
            int IIy = (int)((rs + 10) * Math.Sin(II));
            g.DrawString("2", DefaultFont, Brushes.Black, IIx + offsetX, IIy + offsetY);

            //number 3
            double III = 0 * Math.PI / 180.0;
            int IIIx = (int)((rs + 10) * Math.Cos(III));
            int IIIy = (int)((rs + 10) * Math.Sin(III));
            g.DrawString("3", DefaultFont, Brushes.Black, IIIx + offsetX, IIIy + offsetY);

            //number 4
            double IV = -330 * Math.PI / 180.0;
            int IVx = (int)((rs + 10) * Math.Cos(IV));
            int IVy = (int)((rs + 10) * Math.Sin(IV));
            g.DrawString("4", DefaultFont, Brushes.Black, IVx + offsetX, IVy + offsetY);

            //number 5
            double V = -300 * Math.PI / 180.0;
            int Vx = (int)((rs + 10) * Math.Cos(V));
            int Vy = (int)((rs + 10) * Math.Sin(V));
            g.DrawString("5", DefaultFont, Brushes.Black, Vx + offsetX, Vy + offsetY);

            //number 6
            double VI = -270 * Math.PI / 180.0;
            int VIx = (int)((rs + 10) * Math.Cos(VI));
            int VIy = (int)((rs + 10) * Math.Sin(VI));
            g.DrawString("6", DefaultFont, Brushes.Black, VIx + offsetX, VIy + offsetY);

            //number 7
            double VII = -240 * Math.PI / 180.0;
            int VIIx = (int)((rs + 10) * Math.Cos(VII));
            int VIIy = (int)((rs + 10) * Math.Sin(VII));
            g.DrawString("7", DefaultFont, Brushes.Black, VIIx + offsetX, VIIy + offsetY);

            //number 8
            double VIII = -210 * Math.PI / 180.0;
            int VIIIx = (int)((rs + 10) * Math.Cos(VIII));
            int VIIIy = (int)((rs + 10) * Math.Sin(VIII));
            g.DrawString("8", DefaultFont, Brushes.Black, VIIIx + offsetX, VIIIy + offsetY);

            //number 9
            double IX = -180 * Math.PI / 180.0;
            int IXx = (int)((rs + 10) * Math.Cos(IX));
            int IXy = (int)((rs + 10) * Math.Sin(IX));
            g.DrawString("9", DefaultFont, Brushes.Black, IXx + offsetX, IXy + offsetY);

            //number 10
            double X = -150 * Math.PI / 180.0;
            int Xx = (int)((rs + 10) * Math.Cos(X));
            int Xy = (int)((rs + 10) * Math.Sin(X));
            g.DrawString("10", DefaultFont, Brushes.Black, Xx + offsetX, Xy + offsetY);

            //number 11
            double XI = -120 * Math.PI / 180.0;
            int XIx = (int)((rs + 10) * Math.Cos(XI));
            int XIy = (int)((rs + 10) * Math.Sin(XI));
            g.DrawString("11", DefaultFont, Brushes.Black, XIx + offsetX, XIy + offsetY);

            //number 12
            double XII = -90 * Math.PI / 180.0;
            int XIIx = (int)((rs + 10) * Math.Cos(XII));
            int XIIY = (int)((rs + 10) * Math.Sin(XII));
            g.DrawString("12", DefaultFont, Brushes.Black, XIIx + offsetX, XIIY + offsetY);
        }

        //to add self holiday in the Data base

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textMonth.Text == string.Empty)
            {
                return;
            }

            string connectionString = @"Data Source=data\Vacation.db";

            SQLiteConnection con = new SQLiteConnection(connectionString);

            SQLiteCommand cmd = new SQLiteCommand("Insert into Vacation(month, day, text) VALUES($month, $day, $text)", con);

            try
            {
                cmd.Parameters.AddWithValue("$month", textMonth.Text);
                cmd.Parameters.AddWithValue("$day", textDay.Text);
                cmd.Parameters.AddWithValue("$text", text.Text);

                con.Open();

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("The new Vacation is added");
                }
                else
                {
                    MessageBox.Show("The Vacation isn't added");
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //To exit from application

        private void btnExit_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("آیا برای خروج مطمئن هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) == DialogResult.Yes)
                Close();

        }

        //to delete holiday from data base 

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (textMonth.Text == string.Empty)
            {
                return;
            }
            // MessageBox

            string ConnectionString = @"Data Source=data\Vacation.db";
            SQLiteConnection con = new SQLiteConnection(ConnectionString);

            SQLiteCommand cmd = new SQLiteCommand(@"Delete from Vacation where month=$month and day=$day", con);

            try
            {

                cmd.Parameters.AddWithValue("$month", textMonth.Text);
                cmd.Parameters.AddWithValue("$day", textDay.Text);

                con.Open();

                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show($"{result}This holiday has been deleted");
                }
                else
                {
                    MessageBox.Show("No holiday were found to delete");
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnnorooz_Click(object sender, EventArgs e)
        {
            Draw(350, 210, 140, Pens.Green);
            Draw(150, 100, 80, Pens.Red);
            string[] month=
            {
                "فروردین","اردیبهشت","خرداد",
                "تیر","مرداد","شهریور",
                "مهر","آبان","آذر",
                "دی","بهمن","اردیبهشت"
            };

            for (int i = 0; i < 12; i++)
            {
                
            }
        }

        void Draw(int R, int r, int d, Pen p)
        {
            Graphics g = tabPage3.CreateGraphics();

            g.DrawString("نوروز1401", DefaultFont, Brushes.Red, 800, 650);

            double px_old = R - r + d;
            double py_old = 0;

            int offsetx = this.Width / 2;
            int offsety = this.Height / 2;

            for (double t = 0; t < lcm(R, r) / R * 2 * Math.PI; t += 0.1)
            {
                double cx = (R - r) * Math.Cos(t);
                double cy = (R - r) * Math.Sin(t);

                double u = -R * t / r;

                double px = cx + d * Math.Cos(u);
                double py = cy + d * Math.Sin(u);

                g.DrawLine(p, offsetx + (float)px_old, offsety + (float)py_old, offsetx + (float)px, offsety + (float)py);
                px_old = px;
                py_old = py;

                System.Threading.Thread.Sleep(10);
            }
        }
        static int gcd(int a, int b)
        {
            if (a % b == 0)
                return b;
            return gcd(b, a % b);
        }

        static int lcm(int a, int b)
        {
            return a * b / gcd(a, b);
        }


    }
}



//programer:mohammad mahdy arab
//mail:ceon.m.m.arab@gmail.com
//from damghan university