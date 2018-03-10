using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace WriteJsonTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<WebElement> elements = new List<WebElement>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddElement(object sender, RoutedEventArgs e)
        {
            foreach (var item in elements)
            {
                if (item.name == WebElementNameTextBox.Text)
                {
                    ClearControls();
                    MessageBox.Show("Error the name already exists");
                }
            }
            WebElement webElement = new WebElement();

            if (!string.IsNullOrEmpty(WebElementNameTextBox.Text))
                webElement.name = WebElementNameTextBox.Text;

            if (!string.IsNullOrEmpty(comboBox.Text))
                webElement.locator = comboBox.Text;

            if (!string.IsNullOrEmpty(ValueTextBox.Text))
                webElement.value = ValueTextBox.Text;

            if (!string.IsNullOrEmpty(webElement.name))
            {
                elements.Add(webElement);
                MessageBox.Show(webElement.name + " has been added");
            }

            else
            {
                MessageBox.Show("Name Can not be null");
                ClearControls();
            }
            ClearControls();
        }

        private void ClearControls()
        {
            WebElementNameTextBox.Text = string.Empty;
            comboBox.Text = string.Empty;
            ValueTextBox.Text = string.Empty;
        }

        private void ClearData(object sender, RoutedEventArgs e)
        {
            WebElementNameTextBox.Text = string.Empty;
            comboBox.Text = string.Empty;
            ValueTextBox.Text = string.Empty;
        }

        private void SaveWebElements(object sender, RoutedEventArgs e)
        {
            GenerateFiles.Visibility = Visibility.Visible;
            ChooseFileButton.Visibility = Visibility.Visible;
            TestFileTextBox.Visibility = Visibility.Visible;

        }
        private void GenerateFile(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TestFileTextBox.Text))
            {
                string json = JsonConvert.SerializeObject(elements, Formatting.Indented);
                File.WriteAllText(TestFileTextBox.Text, json);
                MessageBox.Show("File Created Successfully");
                ChooseFileTextBox.Text = TestFileTextBox.Text;
                TestFileTextBox.Text = string.Empty;
                GenerateFiles.Visibility = Visibility.Hidden;
                ChooseFileButton.Visibility = Visibility.Hidden;
                TestFileTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("You Must Choose A File");
            }
        }

        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json";


            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                TestFileTextBox.Text = filename;
            }
        }

        private void GenerateTemplate(object sender, RoutedEventArgs e)
        {
            ChooseOutputFile.Visibility = Visibility.Visible;
            OutPutFile.Visibility = Visibility.Visible;
        }

        private void ChoseFileTemplate(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json";
            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                ChooseFileTextBox.Text = filename;
            }
        }

        private void ShowSaveFile(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ChooseFileTextBox.Text))
            {
                ChooseOutputFile.Visibility = Visibility.Visible;
                OutPutFile.Visibility = Visibility.Visible;
                SaveTemplate.Visibility = Visibility.Visible;
                CopyTemplateToClipBoard.Visibility = Visibility.Visible;
                NewFile.Visibility = Visibility.Visible;
                string outputFile = ChooseFileTextBox.Text.Replace(".json", ".txt");
                OutPutFile.Text = outputFile;
            }
            else
            {
                MessageBox.Show("You Should Choose A File");
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = "txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            //dlg.AddExtension = true;

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                OutPutFile.Text = filename;
            }
        }

        private void SaveTheTemplate(object sender, RoutedEventArgs e)
        {
            var filePath = ChooseFileTextBox.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                var file = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<List<WebElement>>(file);
                var saveFilePath = OutPutFile.Text;
                string webElementTemplate = null;
                StringBuilder sb = new StringBuilder();
                foreach (var name in data)
                {
                    webElementTemplate = String.Format("\tprivate IWebElement {0}(){{{1}\t var {2}Locator = _parser.GetElementByName(\"{3}\");{4}\t var {5} = Browser.Driver.WebDriver.InspectElement({6}Locator, _test);{7}\t return {8}{9}\t}}{10}",
                    name.name.Replace(" ", ""), Environment.NewLine, name.name.ToLower(), name.name, Environment.NewLine, name.name.Replace(" ", ""), name.name.Replace(" ", ""), Environment.NewLine, name.name.Replace(" ", ""), Environment.NewLine, Environment.NewLine);
                    //Console.WriteLine(webElementTemplate);
                    // string path = @"F:\" + fileName + ".txt";
                    TextWriter tw = new StreamWriter(saveFilePath, true);
                    tw.WriteLine(webElementTemplate);
                    tw.Close();
                    sb.Append(webElementTemplate);
                }
                Clipboard.SetText(sb.ToString());
                MessageBox.Show("File is created successfully");
            }
            else
            {
                MessageBox.Show("Please choose the path of the file");
            }
        }

        private void CopyToClipBoard(object sender, RoutedEventArgs e)
        {
            var filePath = ChooseFileTextBox.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                var file = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<List<WebElement>>(file);
                string webElementTemplate = null;
                StringBuilder sb = new StringBuilder();
                foreach (var name in data)
                {
                    webElementTemplate = String.Format("\tprivate IWebElement {0}(){{{1}\t var {2}Locator = _parser.GetElementByName(\"{3}\");{4}\t var {5} = Browser.Driver.WebDriver.InspectElement({6}Locator, _test);{7}\t return {8}{9}\t}}{10}",
                    name.name.Replace(" ", ""), Environment.NewLine, name.name.ToLower(), name.name, Environment.NewLine, name.name.Replace(" ", ""), name.name.Replace(" ", ""), Environment.NewLine, name.name.Replace(" ", ""), Environment.NewLine, Environment.NewLine);
                    sb.Append(webElementTemplate);
                }
                Clipboard.SetText(sb.ToString());
                MessageBox.Show("Template is created successfully");
            }
        }

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            ChooseOutputFile.Visibility = Visibility.Hidden;
            OutPutFile.Visibility = Visibility.Hidden;
            SaveTemplate.Visibility = Visibility.Hidden;
            ChooseFileTextBox.Text = string.Empty;
            OutPutFile.Text = string.Empty;
            CopyTemplateToClipBoard.Visibility = Visibility.Hidden;
            NewFile.Visibility = Visibility.Hidden;
        }
    }
}

