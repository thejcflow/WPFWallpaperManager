using ExceptionCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WallpaperManager.Models;

namespace WallpaperManager.DataHandlers
{
    public class SQLWallpaperDAO : IWallpaperDAO
    {
        private static string connectionString = (string) Application.Current.Resources["ConnectionString"];

        private SqlConnection connection;

        public SQLWallpaperDAO()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public bool Create(Wallpaper wallpaper)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO dbo.Wallpaper (Name, Image) VALUES (@name, @image)";
                command.Parameters.AddWithValue("@name", wallpaper.Name);
                command.Parameters.AddWithValue("@image", wallpaper.Image);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                throw new DatabaseHandlerException(e);
            }
        }

        public List<Wallpaper> Read()
        {
            var items = new List<Wallpaper>();
            try
            {
                string query = "SELECT * FROM dbo.Wallpaper";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new Wallpaper()
                    {
                        Id = int.Parse(reader[0].ToString()),
                        Name = reader[1].ToString(),
                        Image = (byte[])reader[2]
                    });
                }
                reader.Close();
            }
            catch (Exception e)
            {
                throw new DatabaseHandlerException(e);
            }
            return items;
        }


        public bool Update(int wallpaperId, Wallpaper wallpaper)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "UPDATE dbo.Wallpaper SET Name = @name, Image = @image WHERE Id = @id";
                command.Parameters.AddWithValue("@id", wallpaperId);
                command.Parameters.AddWithValue("@name", wallpaper.Name);
                command.Parameters.AddWithValue("@image", wallpaper.Image);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                throw new DatabaseHandlerException(e);
            }
        }

        public bool Delete(int wallpaperId)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "DELETE from dbo.Wallpaper WHERE Id = @id";
                command.Parameters.AddWithValue("@id", wallpaperId);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                throw new DatabaseHandlerException(e);
            }
        }
    }
}
