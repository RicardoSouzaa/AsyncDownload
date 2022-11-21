using System;
using System.Collections.Generic;
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
using System.Net.Http;
using System.Net;
using System.IO;
using System.Globalization;

namespace AsyncExampleWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            resultsTextBox.Clear();
            resultsTextBox.Text = "Buscando Links \naguarde...\n\n";
            //vai buscar e imprimir todos links 1 por 1
            await SumPageSizesAsync();

            startButton.IsEnabled = true;
            resultsTextBox.Text += "\r\nControl returned to startButton_Click.";
        }

        private async Task SumPageSizesAsync()
        {
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            List<string> urlList = SetUpURLLists();

            var total = 0;

            foreach (var url in urlList)
            {
                //busca link por link aguarda resposta e retorna junto com byte de cada link
                byte[] urlContents = await client.GetByteArrayAsync(url);
                //byte[] urlContents = await GetURLContentsAsync(url);

                // imprime cada link depois add os bytes no Length
                DisplayResults(url, urlContents);

                total += urlContents.Length;
            }

            resultsTextBox.Text += string.Format("\r\n\r\nTotal bytes returned: {0}\r\n", total);
        }

        private List<string> SetUpURLLists()
        {
            var urls = new List<string>
            {
                "https://www.softplan.com.br/tech-writers/o-que-e-programacao-assincrona-e-como-utiliza-la/",
                "https://www.delftstack.com/pt/howto/csharp/async-and-await-in-csharp/",
                "https://www.delftstack.com/pt/howto/csharp/async-and-await-in-csharp/",
                "https://www.delftstack.com/pt/howto/csharp/async-and-await-in-csharp/",
                "https://www.delftstack.com/pt/howto/csharp/async-and-await-in-csharp/",
                "https://www.delftstack.com/pt/howto/csharp/async-and-await-in-csharp/",
                "https://www.delftstack.com/pt/howto/csharp/async-and-await-in-csharp/",
                "https://www.softplan.com.br/tech-writers/o-que-e-programacao-assincrona-e-como-utiliza-la/",
                "https://www.softplan.com.br/tech-writers/o-que-e-programacao-assincrona-e-como-utiliza-la/",
                "https://www.softplan.com.br/tech-writers/o-que-e-programacao-assincrona-e-como-utiliza-la/",
                "https://www.google.com.br",
                "https://www.google.com.br",
                "https://www.delftstack.com/pt/howto/csharp/async-and-await-in-csharp/"
            };
            return urls;
        }

        //private async Task<byte[]> GetURLContentsAsync(string url)
        //{
        //    var content = new MemoryStream();

        //    var webReq = (HttpWebRequest)WebRequest.Create(url);

        //    //espera a resposta de cada link.
        //    using (WebResponse response = await webReq.GetResponseAsync())
        //    {
        //        using (Stream responseStream = response.GetResponseStream())
        //        {
        //            // copia o conteudo de cada link e cria uma array
        //            await responseStream.CopyToAsync(content);
        //        }
        //    }
        //    //retorna uma lista array de bytes
        //    return content.ToArray();
        //}

        private void DisplayResults(string url, byte[] content)
        {
            var bytes = content.Length;

            var displayURL = url.Replace("https://", "");
            resultsTextBox.Text += string.Format("\n{0,-58} {1,8}", displayURL, bytes);
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            resultsTextBox.Text += "\nTHREAD SEPARADA\n";
        }
    }

    //metodo para caso o texto do textbox for thread... mudar para vermelho, funciona teoricamente.
    public class ForegroundConverterTest : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "\nTHREAD SEPARADA" ? "Red" : "Black";
        }

        public object ConvertBack(object value, Type typeTarget, object param, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}