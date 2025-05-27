using System;

namespace WebApplication1.EFCore.Models.TestDb20240721
{
    public class DownloadedTorrentInfo
    {
        public Guid Id { get; set; }
        public string TorrentNo { get; set; }
        public string TorrentFileName { get; set; }
    }
}
