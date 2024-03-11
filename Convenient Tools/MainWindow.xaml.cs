using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using ToolModel;
using ToolViewModel;

namespace Convenient_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel MainVM ;
        
        public MainWindow()
        {
            InitializeComponent();
            InitData();
        }

        public void InitData()
        {
            MainVM = Factory.CreateVM(EnumVM.MainViewModel) as MainViewModel;
            this.DataContext = MainVM;
            log_TxtData.SelectedDate = DateTime.Today;
        }

        #region 剪切板
        /// <summary>
        /// 说明：richtextbox的行，允许双击复制单行
        /// 作者：张鑫煜
        /// 日期：2024-01-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyLine(object sender, MouseButtonEventArgs e)
        {   
            Paragraph para = CutText.CaretPosition.Paragraph;
            if(para!=null)
            {
                TextRange textRange = new TextRange(para.ContentStart, para.ContentEnd);
                string txt = textRange.Text;
                Clipboard.SetText(txt);
            }
            else
            {
                CutText.Copy();
            }
        }

        #endregion 剪切板


        #region 查看天气
        /// <summary>
        /// 按钮 查看天气事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_GetWeather(object sender, RoutedEventArgs e)
        {
            if(btnWeather.Content.ToString() == "不看了")
            {
                btnWeather.Background = Brushes.Blue;
                Weather.Visibility = Visibility.Collapsed;
                btnWeather.Content = "看看天气";
                Tab.SelectedIndex = 0;
                return;
            }
            //没有天气数据就获取
            if (wjson?.data == null)
            {
                string api_url = "http://t.weather.itboy.net/api/weather/city/101120901";
                await GetApiData<WeatherJson>(api_url);
            }
            //从天气数据中抽取所需的信息 设置信息
            List<WeatherData> zhanshi = new List<WeatherData>();
            zhanshi.Add(wjson.data.yesterday);
            zhanshi.AddRange(wjson.data.forecast);
            tree.ItemsSource = zhanshi;

            //选中页面改到天气界面
            btnWeather.Content = "不看了";
            btnWeather.Background = Brushes.Red;
            Weather.Visibility = Visibility.Visible;
            Tab.SelectedItem = Weather;
        }

        /// <summary>
        /// 获取接口数据
        /// </summary>
        /// <param name="api_url"></param>
        /// <returns></returns>
        public async Task GetApiData<T>(string api_url)
        {         
            //异步获取接口数据
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(api_url);
            //获取成功后实施反序列化
            if (response.IsSuccessStatusCode)
            {
                if (typeof(T).Equals(typeof(WeatherJson)))
                {
                    string json_string = await response.Content.ReadAsStringAsync();
                    wjson = System.Text.Json.JsonSerializer.Deserialize<WeatherJson>(json_string);
                }
            }
            else
            {
                Console.WriteLine("接口请求失败");
            }
            int a = 0;
            int b = 1;
            (a, b) = (b, a);
        }

        public WeatherJson wjson { set; get; }
        public DataTable DataTree { set; get; }

        #endregion 查看天气

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = log_TxtData + log_TxtType.SelectedItem.ToString() + txt.Text + "\n";

            log_RichText.AppendText(str);
        }

        #region 随机像素点
        private void ColorButton_Loaded(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            for (int i=0; i<50;i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Button btn = new Button();
                    btn.Height = 5;
                    btn.Width = 5;       
                    byte a = (byte)random.Next(256); // 随机Alpha值  
                    byte r = (byte)random.Next(256); // 随机红色值  
                    byte g = (byte)random.Next(256); // 随机绿色值  
                    byte b = (byte)random.Next(256); // 随机蓝色值  
                    Color randomColor = Color.FromArgb(a, r, g, b);
                    btn.Background = new SolidColorBrush(randomColor);
                    btn.Margin = new Thickness(i * 10, j * 10,0,0);
                    btn.MouseEnter += SetColor;
                    btnGrid.Children.Add(btn);
                }
            }
        }
        private void SetColor(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            foreach (var btn in btnGrid.Children)
            {
                byte a = (byte)random.Next(256); // 随机Alpha值  
                byte r = (byte)random.Next(256); // 随机红色值  
                byte g = (byte)random.Next(256); // 随机绿色值  
                byte b = (byte)random.Next(256); // 随机蓝色值  
                Color randomColor = Color.FromArgb(a, r, g, b);
                (btn as Button).Background = new SolidColorBrush(randomColor);
            }
            (sender as Button).Background = Brushes.Black;
        }
        private void Image_MouseMove(object sender, RoutedEventArgs e)
        { }

        #endregion 随机像素点

        #region 小游戏

        private TextBlock txt1 = new TextBlock();
        private int Speed = 5;

        private Point currentPosition = new Point(400, 200); // 初始位置  
        private void GameCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            //change();
        }
        private void change()
        {
            if (string.IsNullOrEmpty(txt1.Text))
            {
                txt1.Text = "美味的小孩在哪里？";
                GameCanvas.Children.Add(txt1);
            }
            //根据得分控制移速，并且上限设置为80
            Speed = 5 + score / 10;
            Speed= Speed > 80 ? 80 : Speed;
            List<Key> lstKey = new List<Key> { Key.A, Key.S, Key.W, Key.D };
            foreach(Key key in lstKey)
            {
                if (Keyboard.IsKeyDown(key))
                {
                    switch (key)
                    {
                        case Key.W:
                            currentPosition.Y -= Speed;
                            break;
                        case Key.S:
                            currentPosition.Y += Speed;
                            break;
                        case Key.A:
                            currentPosition.X -= Speed;
                            break;
                        case Key.D:
                            currentPosition.X += Speed;
                            break;
                        default:
                            return;
                    }
                }
            }

            // 确保人物不走出游戏界面  
            currentPosition.X = Math.Max(0, Math.Min(GameCanvas.ActualWidth - sensei.ActualWidth, currentPosition.X));
            currentPosition.Y = Math.Max(0, Math.Min(GameCanvas.ActualHeight - sensei.ActualHeight, currentPosition.Y));


            // 更新人物位置  
            Canvas.SetLeft(sensei, currentPosition.X);
            Canvas.SetTop(sensei, currentPosition.Y);

            Canvas.SetLeft(txt1, currentPosition.X);
            Canvas.SetTop(txt1, currentPosition.Y);
        }
        #endregion 小游戏

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as TabControl ).SelectedIndex )
            {
                case 4:
                    UpdateGameLogic();
                    break;
                default:
                    break;
            }

        }

        private DispatcherTimer gameLoopTimer;
        private double updateInterval = 16.667; // 约等于60Hz的帧率  

        // 定义一个用于更新UI的方法  
        private async void UpdateGameLogic()
        {
            Canvas.SetLeft(sensei, currentPosition.X);
            Canvas.SetTop(sensei, currentPosition.Y);
            gameLoopTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(updateInterval)
            };
            gameLoopTimer.Tick += CreateImg;
            gameLoopTimer.Start();
        }

        public int score = 0;
        private async void CreateImg(object sender, EventArgs e)
        {
            // 更新界面元素  
            await Application.Current.Dispatcher.Invoke(async () =>
            {
                //异步生成随机图片
                Image ImgStu = new Image();
                ImgStu.Source = new BitmapImage(new Uri("D:/My Project/Convenient Tool/Convenient Tools/Convenient Tools/Resources/粤商通.png"));
                Random rand = new Random();
                ImgStu.Width = rand.Next(100);
                ImgStu.Height = rand.Next(100);
                int x = rand.Next(400);
                int y = rand.Next(400);
                Canvas.SetLeft(ImgStu, x);
                Canvas.SetTop(ImgStu, y);
                GameCanvas.Children.Add(ImgStu);
                await Task.Run(() => ToSmall(ImgStu));
                change();
                //生成之后 根据我们主控图片所在位置判断是否有覆盖 被主控图片覆盖的图片直接删除
                //采用rect控件交互判断
                Rect mainRect = new Rect(new Point(Canvas.GetLeft(sensei), Canvas.GetTop(sensei)),
                        new Size(sensei.ActualWidth, sensei.ActualHeight));
                List<Image> lstImage = new List<Image>();
                foreach (var chd in GameCanvas.Children)
                {
                    Image otherImage = null;
                    if (chd is Image)
                    {
                        otherImage = chd as Image;
                    }
                    else
                        continue;
                    if (otherImage == sensei)
                        continue;
                    Rect otherRect = new Rect(new Point(Canvas.GetLeft(otherImage), Canvas.GetTop(otherImage)),
                      new Size(otherImage.ActualWidth, otherImage.ActualHeight));
                    if (mainRect.IntersectsWith(otherRect))
                    {
                        lstImage.Add(otherImage);
                    }
                }
                lstImage.ForEach(o => GameCanvas.Children.Remove(o));
                score += lstImage.Count;
                Score.Text = score.ToString();
                //Thread.Sleep(20);
            });

        }
        private async void ToSmall(Image ImgStu)
        {
            Application.Current.Dispatcher.Invoke(()=>
            {// 创建一个DoubleAnimation动画来改变Canvas.Top属性的值  
                DoubleAnimation moveUpAnimation = new DoubleAnimation
                { 
                    To = -100,                 // 目标位置，例如向上移动100单位  
                    Duration = new Duration(TimeSpan.FromSeconds(1)), // 动画持续时间  
                    //RepeatBehavior = RepeatBehavior.Forever // 无限重复  
                };

                // 设置动画的目标属性和目标元素  
                Storyboard.SetTarget(moveUpAnimation, ImgStu);
                Storyboard.SetTargetProperty(moveUpAnimation, new PropertyPath("(Canvas.Top)"));

                // 创建一个Storyboard来包含动画  
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(moveUpAnimation);

                moveUpAnimation.Completed += (s, e) =>
                {
                    // 动画结束时销毁Image控件  
                    GameCanvas.Children.Remove(ImgStu);
                };

                // 开始动画  
                storyboard.Begin();
                //GameGrid.Children.Remove(ImgStu);
            });
        }

    }
}
