using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageLoader
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly DelegateCommand _downloadCommand;
        private readonly DelegateCommand _selectCommand;
        
        private string _downloadUrl;
        private byte[] _imageStream;
        private bool _isDownloading;

        public MainWindowViewModel()
        {
            _downloadCommand = SafeCommand.Create(Download, CanExecuteMethod);
            _selectCommand = SafeCommand.Create(Select);
        }

        public string DownloadURL
        {
            get => _downloadUrl;
            set
            {
                _downloadUrl = value;
                RaisePropertyChanged();
                _downloadCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand DownloadCommand => _downloadCommand;
        public ICommand SelectCommand => _selectCommand;

        public byte[] ImageStream
        {
            get => _imageStream;
            set
            {
                _imageStream = value;
                RaisePropertyChanged();
            }
        }
        
        private async Task Select()
        {
            FileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() ?? true)
            {
                var path = await Task.FromResult(dialog.FileName);
                var bytes = await File.ReadAllBytesAsync(path);

                ImageStream = bytes;
            }
        }

        private bool CanExecuteMethod()
        {
            return !string.IsNullOrEmpty(_downloadUrl) && !_isDownloading;
        }

        private async Task Download()
        {
            _isDownloading = true;
            _downloadCommand.RaiseCanExecuteChanged();

            using var client = new WebClient();
            ImageStream = await client.DownloadDataTaskAsync(new Uri(DownloadURL));

            _isDownloading = false;
            _downloadCommand.RaiseCanExecuteChanged();
        }
    }
}
