using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using StockPortfolio.Models;
using Xamarin.Forms;

namespace StockPortfolio
{
    public partial class NotesPage : ContentPage
    {
        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");

        public NotesPage()
        {
            InitializeComponent();

            if (File.Exists(_fileName))
            {
                _editor.Text = File.ReadAllText(_fileName);
            }
        }

        void OnSaveButtonClicked(object sender, EventArgs e)
        {
            File.WriteAllText(_fileName, _editor.Text);
        }

        void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
            _editor.Text = string.Empty;
        }


    }
}
