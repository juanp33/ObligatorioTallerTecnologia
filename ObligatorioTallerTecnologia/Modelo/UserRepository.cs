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
        public Usuario GetUserByEmail(string email)
        {
            try
            {
                Init();
                return conn.Table<Usuario>().FirstOrDefault(u => u.email == email);
            }
            catch (Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                return null;
            }
        }
        private void Init()
        {
            if (conn != null)
                return;
            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Usuario>();
        }

        public UserRepository(string dbPath)
        {
            _dbPath = dbPath;
            Init(); 
        }
        public Usuario GetUserById(int id)
        {
            try
            {
                Init();
                return conn.Table<Usuario>().FirstOrDefault(u => u.idUsuario == id);
            }
            catch (Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                return null;
            }
        }
        public void AddUser(Usuario user)
        {
            try
            {
                Init();
                int result = conn.Insert(user);
                statusMessage = $"Usuario agregado: {user.nombreUsuario} (ID: {user.idUsuario})";
            }
            catch (Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
            }
        }
        public Usuario GetUser(string email, string password)
        {
            try
            {
                Init();
                return conn.Table<Usuario>().FirstOrDefault(u => u.email == email && u.contraseña == password);
            }
            catch (Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                return null;
            }
        }
        public List<Usuario> GetAll()
        {
            try
            {
                return conn.Table<Usuario>().ToList();
            }
            catch (Exception ex)
            {
                statusMessage = $"Error: {ex.Message}";
                return new List<Usuario>();
            }
        }

        public void CloseConnection()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }
    }
}
