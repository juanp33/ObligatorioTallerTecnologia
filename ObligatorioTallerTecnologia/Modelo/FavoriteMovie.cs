using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTallerTecnologia.Modelo
{
    [Table("favorite_movie")]
    public class FavoriteMovie
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string MovieId { get; set; }
    }
}
