using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MessageExchangeWebApp.Models
{
    public class CreateMessageModel
    {
        public string Content { get; set; }
        public string Date { get; set; }
        public string SrcUserLogin { get; set; }
        public string DstUserLogin { get; set; }

    }
}