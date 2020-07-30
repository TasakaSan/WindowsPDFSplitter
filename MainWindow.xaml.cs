using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WindowsPdfSplitter
{
    public partial class MainWindow : Window
    {
        string globalFile;
        string globalFolder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_OpenFile (object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = "c:\\";
            fileDialog.Title = "Windows Pdf Splitter";
            fileDialog.Filter = "PDF Files (*.pdf*)|*.pdf*";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = false;

            string filePath = Path.GetDirectoryName(fileDialog.FileName);       

            if (fileDialog.ShowDialog() == true)
            {  
                PDFSelectorPath.Text = filePath + fileDialog.FileName;

                String filename = fileDialog.FileName;
                globalFile = filename;

                if (File.Exists(filename))
                {
                    byte[] pdfFile = File.ReadAllBytes(filename);
                    iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdfFile);
                    int numberOfPages = reader.NumberOfPages;
                    pdfCount.Text = numberOfPages.ToString();
                }
            }
        }

        private void btn_SaveFile (object sender, RoutedEventArgs e)
        {
            OpenFileDialog folderDialog = new OpenFileDialog();

            folderDialog.ValidateNames = false;
            folderDialog.CheckFileExists = false;
            folderDialog.CheckPathExists = true;
            folderDialog.FileName = "Select folder";

            if (folderDialog.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(folderDialog.FileName);
                PDFSavePath.Text = folderPath;
                globalFolder = folderPath;
            }
        }

        private void btn_SplitPDF(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PDFSelectorPath.Text) && string.IsNullOrEmpty(PDFSavePath.Text))
            {
                MessageBox.Show("You must select a pdf file with valid directory");
            }else if (string.IsNullOrEmpty(PDFSelectorPath.Text))
            {
                MessageBox.Show("You must select a pdf file");
            }else if (string.IsNullOrEmpty(PDFSavePath.Text))
            {
                MessageBox.Show("You must select a valid directory");
            }
            else
            {
                try
                {
                    String source_file = globalFile;
                    String result = globalFolder + "\\";
                    PdfCopy copy;
                    PdfReader.unethicalreading = true;
                    PdfReader reader = new PdfReader(
                        System.IO.File.ReadAllBytes(source_file)       
                        );

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string filename = Path.GetFileNameWithoutExtension(globalFile) + "_" + i + ".pdf";
                        
                        Document document = new Document();

                        copy = new PdfCopy(document, new FileStream(result + filename, FileMode.Create));
                            document.Open();
                                copy.AddPage(copy.GetImportedPage(reader, i));
                            document.Close();
                        
                        progressbarsplit.Value = i * progressbarsplit.Maximum / reader.NumberOfPages;
                    }
                }catch(Exception err)
                {
                    MessageBox.Show(
                        err.ToString() + "\n" +
                        " " + "\n" +
                        "filename : " + globalFile + "\n" +
                        "foldername : " + globalFolder
                    );
                }   
            }
        }

        private void btn_Reset(object sender, RoutedEventArgs e)
        {
            progressbarsplit.Value = 0;
            string globalFile = string.Empty;
            string globalFolder = string.Empty;
            pdfCount.Text = "000";
            PDFSelectorPath.Text = string.Empty;
            PDFSavePath.Text = string.Empty;
        }
    }
}
