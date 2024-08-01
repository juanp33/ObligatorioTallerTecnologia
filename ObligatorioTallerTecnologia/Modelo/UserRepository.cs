using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ObligatorioTallerTecnologia.Modelo;

namespace ObligatorioTallerTecnologia.Modelo
{
    public class UserRepository
    {
        string _dbPath;
        public string statusMessage { get; set; }

        private SQLiteConnection conn;
        private void Init()
        {
            if (conn is not null)
                return;
            conn = new(_dbPath);
            conn.CreateTable<Usuario>();

        }
        public UserRepository(string dbPath)
        {
            _dbPath = dbPath;
        }
        public void AddUser(string nombre, string email, string contraseñaa)
        {
            int result = 0;
            try
            {
                Init();

                Usuario usuario = new Usuario { nombreUsuario = nombre, email = email, contraseña = contraseñaa };
                result = conn.Insert(usuario);
            }
            catch (Exception ex)
            {
                statusMessage = "Error xD";
            }
        }

        public List<Usuario> GetAll()
        {
            try
            {
                Init();
                return conn.Table<Usuario>().ToList();
            }
            catch
            {
                statusMessage = "Error";
            }
            return new List<Usuario>();
        }
    }
}
