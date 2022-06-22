using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;

namespace GetWeeklyPlansV4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<WeeklyPlanInfo> weeklyPlanInfos = new List<WeeklyPlanInfo>();
        
        public MainWindow()
        {
            InitializeComponent();
            this.ListBoxWeeklyPlans.ItemsSource = weeklyPlanInfos;
            Refresh();
        }

        private async void Download(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);
            button.IsEnabled = false;
            var downloadLink = (button.Tag as WeeklyPlanInfo).DownloadLink as string;
            var originHttpRaw = await new HttpClient().GetStringAsync(downloadLink);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(originHttpRaw);
            var nodeDiv1 = htmlDoc.DocumentNode.SelectSingleNode("//body/div"); // bodybg
            var nodeDiv2 = nodeDiv1.SelectNodes("div")[1]; // container
            var nodeDiv3 = nodeDiv2.SelectNodes("div")[1]; // wenzhang
            var nodeDiv4 = nodeDiv3.SelectSingleNode("div"); // con_main
            var nodeDiv5 = nodeDiv4.SelectNodes("div")[1]; //newscontent
            var targetHttpRaw = nodeDiv5.InnerHtml;
            
            
            
            string fileName = $"result_{(button.Tag as WeeklyPlanInfo).Title}.html";
            try
            {
                await File.WriteAllTextAsync(fileName, targetHttpRaw);
                if (MessageBox.Show($"{fileName}\n成功保存到根目录,按确认打开") == MessageBoxResult.OK)
                {
                    new PlanViewer(targetHttpRaw).Show();
                }

                button.IsEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void UpdateSource()
        {
            this.ListBoxWeeklyPlans.ItemsSource = null;
            this.ListBoxWeeklyPlans.ItemsSource = weeklyPlanInfos;
        }

        private async void Refresh()
        {
            var html = @"http://www.aqyz.net/xwzx/mzrc/index.html";
            var htmlRaw = await new HttpClient().GetStringAsync(html);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlRaw);
            
            var nodeDiv1 = htmlDoc.DocumentNode.SelectSingleNode("//body/div");
            var nodeDiv2 = nodeDiv1.SelectNodes("div")[1];
            var nodeDiv3 = nodeDiv2.SelectSingleNode("div");
            var nodeDiv4 = nodeDiv3.SelectNodes("div")[1];
            var nodeDiv5 = nodeDiv4.SelectNodes("div")[1];
            var nodeUl = nodeDiv5.SelectSingleNode("ul");
            var nodeLis = nodeUl.SelectNodes("li");
            
            weeklyPlanInfos.Clear();
            
            foreach (var nodeLi in nodeLis)
            {
                var nodeA = nodeLi.SelectSingleNode("a");
                var title = nodeA.InnerText.Trim();
                var href = nodeA.Attributes["href"].Value;
                
                this.weeklyPlanInfos.Add(new WeeklyPlanInfo() {Title = title,DownloadLink = href});
            }
            
            UpdateSource();
        }
        private void Refresh(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}