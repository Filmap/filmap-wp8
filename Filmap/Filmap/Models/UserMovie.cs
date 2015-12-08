using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmap
{
    public class UserMovie
    {
        public string id { get; set; }
        public string omdb { get; set; }
        public string user_id { get; set; }
        public string watched { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public bool? Response { get; set; }
    }
}
