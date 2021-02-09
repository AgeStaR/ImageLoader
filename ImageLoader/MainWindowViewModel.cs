﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageLoader
{
    internal class MainWindowViewModel : BindableBase
    {
        private string _downloadUrl;
        private byte[] _imageStream;
        private readonly DelegateCommand _downloadCommand;

        public MainWindowViewModel()
        {
            _downloadCommand = new DelegateCommand(async () => await Download(), CanExecuteMethod);
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

        public byte[] ImageStream
        {
            get => _imageStream;
            set
            {
                _imageStream = value;
                RaisePropertyChanged();
            }
        }

        private bool CanExecuteMethod()
        {
            return !string.IsNullOrEmpty(_downloadUrl);
        }

        private async Task Download()
        {
            using var client = new WebClient();
            ImageStream = await client.DownloadDataTaskAsync(new Uri(DownloadURL));
        }
    }
}