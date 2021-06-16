using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.Repositories
{
   public class BlogRepository : DatabaseConnector, IRepository<Blog>
    {
        public BlogRepository(string connectionString) : base(connectionString) { }

        public List<Blog> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, title, url FROM Blog";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Blog> blogs = new List<Blog>();

                while (reader.Read())
                {
                    Blog blog = new Blog()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Url = reader.GetString(reader.GetOrdinal("URL"))
                    };

                    blogs.Add(blog);

                }
                    return blogs;
                   


                }

            }
        }
            //using (SqlConnection conn = Connection)
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = @"SELECT "
            //    }
            //}
        public Blog Get(int id)
        {
            throw new NotImplementedException();
        }
        public void Insert(Blog entry)
        {
            throw new NotImplementedException();
        }
        public void Update(Blog entry)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }




    }
}
