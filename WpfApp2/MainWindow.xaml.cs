using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Input;
namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private System.Windows.Media.Color currentFontColor;
        private System.Windows.Media.Brush currentFontBrush;

        string doucmentpath = "未存檔"; //狀態列檔名
        public MainWindow()
        {
            InitializeComponent();
            EnumerateSystemFonts();
        }
        private void EnumerateSystemFonts()
        {
            foreach (System.Windows.Media.FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                fontComboBox.Items.Add(fontFamily.Source);
            }
            //加入sizeComboBox元素
            sizeComboBox.Items.Add("8");
            sizeComboBox.Items.Add("9");
            sizeComboBox.Items.Add("10");
            sizeComboBox.Items.Add("12");
            sizeComboBox.Items.Add("14");
            sizeComboBox.Items.Add("18");
            sizeComboBox.Items.Add("20");
            sizeComboBox.Items.Add("22");
            sizeComboBox.Items.Add("24");
            sizeComboBox.Items.Add("26");
            sizeComboBox.Items.Add("28");
            sizeComboBox.Items.Add("36");
            sizeComboBox.Items.Add("48");
            sizeComboBox.SelectedIndex = 3;
            fontComboBox.SelectedIndex = 0;
        }
        //檔案功能
        //開新檔案
        private void New_Executed(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
        }
        //開啟舊檔
        private void Open_Executed(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new
           Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "讀取文字檔案";
            openFileDialog.DefaultExt = ".rtf";
            openFileDialog.Filter = "文字檔案(*.rtf)|*.rtf|全部檔案(*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                doucmentpath = Path.GetFileName(openFileDialog.FileName); //讀取檔名
                //讀取rtf內容
                FileStream fs = new FileStream(path, FileMode.Open);
                TextRange range = new TextRange(myrichtextbox.Document.ContentStart,
               myrichtextbox.Document.ContentEnd);
                range.Load(fs, System.Windows.DataFormats.Rtf);
                
            }
        }
        //儲存檔案
        private void Save_Executed(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new
           Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Title = "儲存文字檔案";
            saveFileDialog.DefaultExt = ".rtf";
            saveFileDialog.Filter = "文字檔案(*.rtf)|*.rtf";
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                doucmentpath = Path.GetFileName(saveFileDialog.FileName); //讀取檔名
                FileStream fs = new FileStream(path, FileMode.Create);
                //讀取出richtextbox 文字
                TextRange text = new TextRange(myrichtextbox.Document.ContentStart,
               myrichtextbox.Document.ContentEnd);
                text.Save(fs, System.Windows.DataFormats.Rtf);
                System.Windows.MessageBox.Show("儲存成功!");
            }
        }
        //離開程式
        private void Close_Executed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //編輯功能
        //清除文字
        private void ClearText(object sender, RoutedEventArgs e)
        {
            myrichtextbox.Document.Blocks.Clear();
        }
        //字體大小功能
        private void sizeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextRange textRange = myrichtextbox.Selection;
            if (!textRange.IsEmpty)
            {
                textRange.ApplyPropertyValue(FontSizeProperty, sizeComboBox.SelectedItem);
            }
        }
        //字體字型功能
        private void fontComboBox_SelectionChanged(object sender,
       System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TextSelection selectedtext = myrichtextbox.Selection;
            if (!selectedtext.IsEmpty)
            {
                selectedtext.ApplyPropertyValue(FontFamilyProperty,
               fontComboBox.SelectedItem);
            }
        }
        //選取顏色
        private System.Windows.Media.Color GetDialogColor() //選顏色
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            System.Drawing.Color color = colorDialog.Color;
            return currentFontColor = System.Windows.Media.Color.FromArgb(color.A, color.R,
           color.G, color.B);
        }
        //字體顏色功能
        private void FontColor_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = myrichtextbox.Selection;
            if (!textRange.IsEmpty)
            {
                currentFontColor = GetDialogColor();
                currentFontBrush = new SolidColorBrush(currentFontColor);
                textRange.ApplyPropertyValue(TextElement.ForegroundProperty,
               currentFontBrush);
            }
        }
        //字體背景顏色功能
        private void FontBackground_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = myrichtextbox.Selection;
            if (!textRange.IsEmpty)
            {
                currentFontColor = GetDialogColor();
                currentFontBrush = new SolidColorBrush(currentFontColor);
                textRange.ApplyPropertyValue(TextElement.BackgroundProperty,
               currentFontBrush);
            }
        }
        //statusbar功能 顯示檔名、在第幾個字及選取幾個字
        private void richtextbox_selctionhanged(object sender, RoutedEventArgs e)
        {
            TextRange index = new TextRange(myrichtextbox.Document.ContentStart,
           myrichtextbox.Selection.Start);
            status_label.Content = "檔名：" +doucmentpath +"。 第" + index.Text.Length + "字，選取字數" +
           myrichtextbox.Selection.Text.Length + "字。";
        }

        private void closewindow(object sender, System.ComponentModel.CancelEventArgs e) //按下關閉時詢問是否存檔
        {
            var result = System.Windows.MessageBox.Show("即將關閉，是否存檔?", "關閉檔案", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                Save_Executed(null, null);
                e.Cancel = false;
            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = false;
            }
            else e.Cancel = true;
        }

    }
}