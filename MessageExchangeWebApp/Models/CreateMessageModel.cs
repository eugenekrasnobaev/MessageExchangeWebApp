using System;

namespace MessageExchangeWebApp.Models
{
    public class CreateMessageModel
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string SrcUserLogin { get; set; }
        public string DstUserLogin { get; set; }

    }
}