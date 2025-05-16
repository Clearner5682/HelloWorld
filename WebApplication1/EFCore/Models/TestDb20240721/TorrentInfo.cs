using System;

namespace WebApplication1.EFCore.Models.TestDb20240721
{
    public class TorrentInfo
    {
        public Guid Id { get; set; }
        public string TorrentNo { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
