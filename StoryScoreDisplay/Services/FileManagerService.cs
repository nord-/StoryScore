﻿using Newtonsoft.Json;
using StoryScore.Common;
using StoryScore.Display.Mqtt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryScore.Display.Services
{
    public class FileManagerService
    {
        private readonly Options _options;
        private readonly Client _mqttClient;
        private Thread _fileTransferThread;

        public FileManagerService(Options options, Mqtt.Client mqttClient)
        {
            _options = options;
            _mqttClient = mqttClient;
        }

        public string FileStorePath => _options.FileStorePath;

        public FileStream CreateFile(string filename)
        {
            var file = new FileStream(Path.Combine(FileStorePath, filename), FileMode.Create);
            return file;
        }

        public IEnumerable<RemoteFileInfo> GetRemoteFiles()
        {
            var fileStore = new DirectoryInfo(FileStorePath);
            foreach (var f in fileStore.GetFiles())
                yield return new RemoteFileInfo
                {
                    Name = f.Name,
                    AbsolutePath = f.FullName,
                    FileSize = f.Length,
                    ModifiedDate = f.LastWriteTime,
                    Checksum = Common.FileIO.GetChecksum(f.FullName)
                };
        }

        public async Task<Thread> StartReceiveFileAsync(string messageAsJson)
        {
            var fileInfo                   = JsonConvert.DeserializeObject<FileTransferStatus>(messageAsJson);
            var fileService                = new TcpServer.ReceiveFile(fileInfo, this);
            fileService.FileReceived       += FileService_FileReceived;
            fileService.FileTransferStatus += FileService_FileTransferStatus;

            var message = new Common.TcpServer
            {
                IPAddress = fileService.PublicIPAddress.ToString(),
                Port = TcpServer.ReceiveFile.Port
            };

            _fileTransferThread = new Thread(new ThreadStart(fileService.Listen));
            _fileTransferThread.Start();

            message.Timestamp = DateTime.Now;

            await _mqttClient.SendMessageAsync(_mqttClient.GetTopic(Common.Constants.Mqtt.ReceiveFile), message);

            return _fileTransferThread;
        }

        private async void FileService_FileTransferStatus(FileTransferStatus txStatus)
        {
            await _mqttClient.SendMessageAsync(_mqttClient.GetTopic(Common.Constants.Mqtt.TransferStatus), txStatus);
        }

        private async void FileService_FileReceived(FileTransferStatus txStatus)
        {
            Debug.Print($"File received: {txStatus.Name} took {txStatus.ElapsedMilliseconds} ms.");
            await _mqttClient.SendMessageAsync(_mqttClient.GetTopic(Common.Constants.Mqtt.TransferStatus), txStatus);

            if (_fileTransferThread.IsAlive)
            {
                _fileTransferThread.Abort();
                _fileTransferThread = null;
            }
        }
    }
}