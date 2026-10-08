using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ThreadDemo
{
    public partial class Form1 : Form
    {
        // ==========================================
        // KHAI BÁO BIẾN
        // ==========================================

        MyThread[] threadLst = new MyThread[26];

        const int xCell = 30;
        const int yCell = 30;

        const int xMax = 25;
        const int yMax = 20;

        public Random rnd = new Random();


        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public Form1()
        {
           

            // Cho Form nhận phím
            this.KeyPreview = true;

            // Đăng ký sự kiện
            this.Load += Form1_Load;
            this.KeyDown += Form1_KeyDown;
            this.FormClosed += Form1_FormClosed;
        }


        // ==========================================
        // GIẢ LẬP THREAD ĐANG LÀM VIỆC
        // ==========================================

        void MySleep(long count)
        {
            long i;
            long j;
            long k = 0;

            for (i = 0; i < count; i++)
            {
                for (j = 0; j < 64000; j++)
                {
                    k++;
                }
            }
        }


        // ==========================================
        // HÀM CHẠY CỦA THREAD
        // ==========================================

        void Running(object obj)
        {
            MyThread p = (MyThread)obj;

            Graphics g = null;
            Brush brush = null;

            try
            {
                g = this.CreateGraphics();
                brush = new SolidBrush(Color.Black);

                int x1;
                int y1;

                int x;
                int y;

                while (p.start)
                {
                    // ==================================
                    // NẾU THREAD ĐANG TẠM DỪNG
                    // ==================================

                    p.WaitIfPaused();

                    // Sau khi Resume, kiểm tra lại
                    if (!p.start)
                        break;


                    // ==================================
                    // LẤY VỊ TRÍ HIỆN TẠI
                    // ==================================

                    x1 = p.Pos.X;
                    y1 = p.Pos.Y;


                    // ==================================
                    // VẼ ICON
                    // ==================================

                    if (p.Pic != null)
                    {
                        g.DrawImage(
                            p.Pic,
                            xCell * x1,
                            yCell * y1
                        );
                    }


                    // ==================================
                    // LẤY MÀU CỦA ICON
                    // ==================================

                    Color c = Color.White;

                    if (p.Pic != null &&
                        p.Pic.Width > 1 &&
                        p.Pic.Height > 1)
                    {
                        c = p.Pic.GetPixel(1, 1);
                    }


                    int yR;
                    int yG;
                    int yB;


                    if (c.R > 128)
                        yR = 0;
                    else
                        yR = 255;


                    if (c.G > 128)
                        yG = 0;
                    else
                        yG = 255;


                    if (c.B > 128)
                        yB = 0;
                    else
                        yB = 255;


                    Pen pen = new Pen(
                        Color.FromArgb(yR, yG, yB),
                        2
                    );


                    // ==================================
                    // VẼ MŨI TÊN
                    // ==================================

                    if (p.tx >= 0 && p.ty >= 0)
                    {
                        // Góc dưới phải

                        x = xCell * x1 + xCell - 2;
                        y = yCell * y1 + yCell - 2;

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x - 10,
                            y
                        );

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x,
                            y - 10
                        );
                    }
                    else if (p.tx >= 0 && p.ty < 0)
                    {
                        // Góc trên phải

                        x = xCell * x1 + xCell - 2;
                        y = yCell * y1 + 2;

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x - 10,
                            y
                        );

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x,
                            y + 10
                        );
                    }
                    else if (p.tx < 0 && p.ty >= 0)
                    {
                        // Góc dưới trái

                        x = xCell * x1 + 2;
                        y = yCell * y1 + yCell - 2;

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x + 10,
                            y
                        );

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x,
                            y - 10
                        );
                    }
                    else
                    {
                        // Góc trên trái

                        x = xCell * x1 + 2;
                        y = yCell * y1 + 2;

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x + 10,
                            y
                        );

                        g.DrawLine(
                            pen,
                            x,
                            y,
                            x,
                            y + 10
                        );
                    }


                    pen.Dispose();


                    // ==================================
                    // GIẢ LẬP THREAD LÀM VIỆC
                    // ==================================

                    MySleep(500);


                    // ==================================
                    // KIỂM TRA LẠI TRẠNG THÁI
                    // ==================================

                    if (!p.start)
                        break;


                    // ==================================
                    // CẬP NHẬT VỊ TRÍ
                    // ==================================

                    p.HieuchinhVitri();


                    // ==================================
                    // XÓA VỊ TRÍ CŨ
                    // ==================================

                    g.FillRectangle(
                        brush,
                        xCell * x1,
                        yCell * y1,
                        xCell,
                        yCell
                    );
                }
            }
            catch
            {
                // Thread kết thúc
            }
            finally
            {
                // ==================================
                // XÓA ICON CUỐI CÙNG
                // ==================================

                try
                {
                    if (g != null && p != null)
                    {
                        int x1 = p.Pos.X;
                        int y1 = p.Pos.Y;

                        g.FillRectangle(
                            brush,
                            xCell * x1,
                            yCell * y1,
                            xCell,
                            yCell
                        );
                    }
                }
                catch
                {
                }


                // Thread đã dừng
                p.stop = true;


                if (brush != null)
                    brush.Dispose();

                if (g != null)
                    g.Dispose();
            }
        }


        // ==========================================
        // FORM LOAD
        // ==========================================

        private void Form1_Load(
            object sender,
            EventArgs e)
        {
            for (int i = 0; i < 26; i++)
            {
                // ==================================
                // TẠO MyThread
                // ==================================

                threadLst[i] =
                    new MyThread(
                        rnd,
                        xMax,
                        yMax
                    );


                // ==================================
                // TRẠNG THÁI BAN ĐẦU
                // ==================================

                threadLst[i].start = false;
                threadLst[i].stop = false;
                threadLst[i].suspend = false;


                // ==================================
                // ĐƯỜNG DẪN ẢNH
                // ==================================

                string fileName =
                    Path.Combine(
                        Application.StartupPath,
                        "Resources",
                        (i + 1) + ".bmp"
                    );


                // ==================================
                // KIỂM TRA FILE
                // ==================================

                if (!File.Exists(fileName))
                {
                    MessageBox.Show(
                        "Không tìm thấy file:\n\n"
                        + fileName,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }


                // ==================================
                // ĐỌC ẢNH
                // ==================================

                try
                {
                    using (FileStream fs =
                        new FileStream(
                            fileName,
                            FileMode.Open,
                            FileAccess.Read))
                    {
                        threadLst[i].Pic =
                            new Bitmap(fs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Không thể đọc file:\n"
                        + fileName
                        + "\n\n"
                        + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }


                // ==================================
                // KÍCH THƯỚC VÙNG CHẠY
                // ==================================

                threadLst[i].Xmax = xMax;
                threadLst[i].Ymax = yMax;
            }


            // ==================================
            // KÍCH THƯỚC FORM
            // ==================================

            this.ClientSize =
                new Size(
                    xMax * xCell,
                    yMax * yCell
                );


            // Đặt Form ở góc trái màn hình
            this.Location =
                new Point(0, 0);


            // Màu nền
            this.BackColor =
                Color.Black;


            // Cho Form nhận bàn phím
            this.Focus();
        }


        // ==========================================
        // XỬ LÝ PHÍM
        // ==========================================

        private void Form1_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            // ==================================
            // CHỈ NHẬN A-Z
            // ==================================

            if (e.KeyCode < Keys.A ||
                e.KeyCode > Keys.Z)
            {
                return;
            }


            // A = 0
            // B = 1
            // ...
            // Z = 25

            int index =
                e.KeyCode - Keys.A;


            MyThread p =
                threadLst[index];


            // ==================================
            // CTRL + SHIFT + X
            // DỪNG THREAD
            // ==================================

            if (e.Control && e.Shift)
            {
                if (p.start)
                {
                    p.start = false;

                    // Nếu đang pause thì đánh thức thread
                    p.ResumeThread();

                    p.suspend = false;
                }
            }


            // ==================================
            // CTRL + ALT + X
            // TẠM DỪNG THREAD
            // ==================================

            else if (e.Control && e.Alt)
            {
                if (p.start && !p.suspend)
                {
                    p.PauseThread();

                    p.suspend = true;
                }
            }


            // ==================================
            // ALT + X
            // CHẠY TIẾP THREAD
            // ==================================

            else if (e.Alt)
            {
                if (p.start && p.suspend)
                {
                    p.ResumeThread();

                    p.suspend = false;
                }
            }


            // ==================================
            // CTRL + X
            // GIẢM PRIORITY
            // ==================================

            else if (e.Control)
            {
                if (p.t != null &&
                    p.t.IsAlive)
                {
                    p.t.Priority =
                        ThreadPriority.Lowest;

                    MessageBox.Show(
                        "Thread "
                        + e.KeyCode
                        + "\nPriority = "
                        + p.t.Priority,
                        "Thread Priority"
                    );
                }
            }


            // ==================================
            // SHIFT + X
            // TĂNG PRIORITY
            // ==================================

            else if (e.Shift)
            {
                if (p.t != null &&
                    p.t.IsAlive)
                {
                    p.t.Priority =
                        ThreadPriority.Highest;

                    MessageBox.Show(
                        "Thread "
                        + e.KeyCode
                        + "\nPriority = "
                        + p.t.Priority,
                        "Thread Priority"
                    );
                }
            }


            // ==================================
            // CHỈ A-Z
            // TẠO THREAD
            // ==================================

            else
            {
                if (!p.start)
                {
                    p.start = true;
                    p.stop = false;
                    p.suspend = false;


                    // Tạo Thread mới
                    p.t =
                        new Thread(
                            new ParameterizedThreadStart(
                                Running
                            )
                        );


                    // ==================================
                    // THREAD A = HIGHEST
                    // THREAD CÒN LẠI = LOWEST
                    // ==================================

                    if (e.KeyCode == Keys.A)
                    {
                        p.t.Priority =
                            ThreadPriority.Highest;
                    }
                    else
                    {
                        p.t.Priority =
                            ThreadPriority.Lowest;
                    }


                    // Bắt đầu Thread
                    p.t.Start(p);
                }
            }


            // ==================================
            // NGĂN WINDOWS XỬ LÝ TIẾP PHÍM
            // ==================================

            e.Handled = true;
            e.SuppressKeyPress = true;
        }


        // ==========================================
        // FORM CLOSED
        // ==========================================

        private void Form1_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            // ==================================
            // YÊU CẦU TẤT CẢ THREAD DỪNG
            // ==================================

            for (int i = 0; i < 26; i++)
            {
                if (threadLst[i] != null)
                {
                    threadLst[i].start = false;

                    // Nếu đang pause thì Resume
                    // để thread có thể thoát
                    threadLst[i].ResumeThread();
                }
            }


            // ==================================
            // CHỜ THREAD KẾT THÚC
            // ==================================

            for (int i = 0; i < 26; i++)
            {
                if (threadLst[i] != null &&
                    threadLst[i].t != null &&
                    threadLst[i].t.IsAlive)
                {
                    try
                    {
                        threadLst[i].t.Join(1000);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}