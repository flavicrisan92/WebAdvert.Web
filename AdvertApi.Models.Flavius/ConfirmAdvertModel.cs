using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertApi.Models.Flavius
{
    public class ConfirmAdvertModel
    {
        public string Id { get; set; }
        public AdvertStatus Status { get; set; }
        public string FilePath { get; set; }
    }
}
