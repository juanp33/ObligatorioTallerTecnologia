﻿using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTallerTecnologia.Modelo
{
    [Table("usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int idUsuario { get; set; }

        [MaxLength(20), Unique]
        public string nombreUsuario { get; set; }

        [MaxLength(100), Unique]
        public string email { get; set; }

        [MaxLength(20)]
        public string contraseña { get; set; }

        [MaxLength(20)]
        public string telefono { get; set; }
        public string imagenFoto { get; set; }

        
      
    }
}
