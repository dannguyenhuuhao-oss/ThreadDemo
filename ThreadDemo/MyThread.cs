using System;
using System.Drawing;
using System.Threading;

namespace ThreadDemo
{
    class MyThread
    {
        // ==========================================
        // HẰNG SỐ
        // ==========================================

        const double PI = 3.1416;


        // ==========================================
        // CÁC THUỘC TÍNH
        // ==========================================

        // Thread hiện hành
        public Thread t;


        // Thread đang chạy
        public bool start;


        // Thread đã dừng
        public bool stop;


        // Thread đang tạm dừng
        public bool suspend;


        // Hình ảnh
        public Bitmap Pic;


        // Kích thước vùng chạy
        internal int Xmax;
        internal int Ymax;


        // Vị trí
        public Point Pos;


        // Bước di chuyển
        internal double tx;
        internal double ty;


        // Góc chạy
        double dblGocChay;


        // ==========================================
        // ĐỐI TƯỢNG ĐIỀU KHIỂN PAUSE / RESUME
        // ==========================================

        private ManualResetEventSlim pauseEvent =
            new ManualResetEventSlim(true);


        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public MyThread(
            Random rnd,
            int xMax,
            int yMax)
        {
            Xmax = xMax;
            Ymax = yMax;


            // Vị trí ngẫu nhiên
            Pos.X =
                rnd.Next(
                    0,
                    Xmax
                );


            Pos.Y =
                rnd.Next(
                    0,
                    Ymax
                );


            // Góc chạy ngẫu nhiên
            dblGocChay =
                ChinhGocChay(
                    rnd.Next(
                        0,
                        360
                    )
                );
        }


        // ==========================================
        // TẠM DỪNG THREAD
        // ==========================================

        public void PauseThread()
        {
            pauseEvent.Reset();
        }


        // ==========================================
        // CHO THREAD CHẠY LẠI
        // ==========================================

        public void ResumeThread()
        {
            pauseEvent.Set();
        }


        // ==========================================
        // CHỜ KHI THREAD ĐANG PAUSE
        // ==========================================

        public void WaitIfPaused()
        {
            pauseEvent.Wait();
        }


        // ==========================================
        // ĐIỀU CHỈNH GÓC CHẠY
        // ==========================================

        double ChinhGocChay(
            double goc)
        {
            if (0 <= goc && goc < 90)
                return 45;


            if (90 <= goc && goc < 180)
                return 135;


            if (180 <= goc && goc < 270)
                return 225;


            if (270 <= goc && goc < 360)
                return 315;


            return 45;
        }


        // ==========================================
        // PHẢN XẠ KHI ĐỤNG THÀNH DỌC
        // ==========================================

        double DoiGocChayX(
            double goc)
        {
            double gocMoi;


            if (goc > 0 && goc < 180)
            {
                gocMoi =
                    180 - goc;
            }
            else
            {
                gocMoi =
                    180 + 360 - goc;
            }


            return ChinhGocChay(gocMoi);
        }


        // ==========================================
        // PHẢN XẠ KHI ĐỤNG THÀNH NGANG
        // ==========================================

        double DoiGocChayY(
            double goc)
        {
            return ChinhGocChay(
                360 - goc
            );
        }


        // ==========================================
        // ĐIỀU CHỈNH VỊ TRÍ
        // ==========================================

        public void HieuchinhVitri()
        {
            int x;
            int y;


            x = Pos.X;
            y = Pos.Y;


            // ==================================
            // KIỂM TRA ĐỤNG THÀNH
            // ==================================

            if (x == 0 ||
                x == Xmax - 1 ||
                y == 0 ||
                y == Ymax - 1)
            {
                // Đụng thành trái / phải
                if (x == 0 ||
                    x == Xmax - 1)
                {
                    dblGocChay =
                        DoiGocChayX(
                            dblGocChay
                        );
                }


                // Đụng thành trên / dưới
                else if (y == 0 ||
                         y == Ymax - 1)
                {
                    dblGocChay =
                        DoiGocChayY(
                            dblGocChay
                        );
                }
            }


            // ==================================
            // TÍNH X
            // ==================================

            tx =
                2 *
                Math.Cos(
                    dblGocChay *
                    PI /
                    180
                );


            x =
                x + (int)tx;


            if (x < 0)
            {
                x = 0;
            }
            else if (x >= Xmax)
            {
                x = Xmax - 1;
            }


            // ==================================
            // TÍNH Y
            // ==================================

            ty =
                2 *
                Math.Sin(
                    dblGocChay *
                    PI /
                    180
                );


            y =
                y + (int)ty;


            if (y < 0)
            {
                y = 0;
            }
            else if (y >= Ymax)
            {
                y = Ymax - 1;
            }


            // ==================================
            // XỬ LÝ 4 GÓC
            // ==================================

            if (x == 0 && y == 0)
            {
                // Góc trên trái
                dblGocChay =
                    ChinhGocChay(
                        dblGocChay + 45
                    );
            }
            else if (x == 0 &&
                     y == Ymax - 1)
            {
                // Góc dưới trái
                dblGocChay =
                    ChinhGocChay(
                        dblGocChay + 45
                    );
            }
            else if (x == Xmax - 1 &&
                     y == 0)
            {
                // Góc trên phải
                dblGocChay =
                    ChinhGocChay(
                        dblGocChay + 45
                    );
            }
            else if (x == Xmax - 1 &&
                     y == Ymax - 1)
            {
                // Góc dưới phải
                dblGocChay =
                    ChinhGocChay(
                        dblGocChay + 45
                    );
            }


            // ==================================
            // LƯU VỊ TRÍ MỚI
            // ==================================

            Pos.X = x;
            Pos.Y = y;
        }
    }
}