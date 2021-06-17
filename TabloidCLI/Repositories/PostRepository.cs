﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.Repositories
{
    public class PostRepository : DatabaseConnector, IRepository<Post>
    {
        public PostRepository(string connectionString) : base(connectionString) { }

        public List<Post> GetAll()
        {
           using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * From Post WHERE IsDeleted = 0";
                    List<Post> posts = new List<Post>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("URL")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                //FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                //LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                //Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                //Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                //Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }
                    reader.Close();

                    return posts;
                }
            }
        }

        public Post Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id AS PostId,
                                               p.Title,
                                               p.Url,
                                               p.PublishDateTime
                                          FROM Post p 
                                         WHERE p.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Post post = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (post == null)
                        {
                            post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PostId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("Url")),
                                PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            };
                        }

                        //if (!reader.IsDBNull(reader.GetOrdinal("TagId")))
                        //{
                        //    post.Tags.Add(new Tag()
                        //    {
                        //        Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                        //        Name = reader.GetString(reader.GetOrdinal("Name")),
                        //    });
                        //}
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        public List<Post> GetByAuthor(int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id,
                                               p.Title As PostTitle,
                                               p.URL AS PostUrl,
                                               p.PublishDateTime,
                                                 p.AuthorId,
                                               p.BlogId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Title AS BlogTitle,
                                               b.URL AS BlogUrl
                                          FROM Post p 
                                               LEFT JOIN Author a on p.AuthorId = a.Id
                                               LEFT JOIN Blog b on p.BlogId = b.Id 
                                         WHERE p.AuthorId = @authorId";
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public void Insert(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Post 
                                    (Title, Url, PublishDateTime, BlogId, AuthorId)
                                     OUTPUT INSERTED.id
                                     VALUES (@Title, @Url, @PublishDateTime,                       @BlogId, @AuthorId)";
                    cmd.Parameters.AddWithValue("@Title", post.Title);
                    cmd.Parameters.AddWithValue("@Url", post.Url);
                    cmd.Parameters.AddWithValue("@PublishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@BlogId", post.Blog.Id);
                    cmd.Parameters.AddWithValue("@AuthorId", post.Author.Id);

                    int success = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Post post)
        {
            using (SqlConnection conn = Connection)
            {

                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE post
                                        SET Title = @title,
                                        URL = @url,
                                        PUBLISHDATETIME = @PublishDateTime,
                                        AUTHORID = @AuthorId";
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.Url);
                    cmd.Parameters.AddWithValue("@PublishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@AuthorId", post.Author.Id);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Post WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
