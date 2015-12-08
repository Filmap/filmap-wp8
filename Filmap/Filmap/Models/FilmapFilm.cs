using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmap.Models
{

    public class FilmapFilm
    {
        public int id { get; set; }
        public string omdb { get; set; }
        public int user_id { get; set; }
        public int watched { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

}
