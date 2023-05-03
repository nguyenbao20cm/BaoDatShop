using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.NewDetail
{
    public class GetAllNewDetailResponse
    {
        public int NewDetailId { get; set; }
        public int NewId { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}
