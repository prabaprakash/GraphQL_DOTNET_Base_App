using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using System.Data;
namespace GraphQL_Example_DOT_NET
{
    class Program
    {
        public static void Main(string[] args)
        {
            Run().Wait();
        }
        private static async Task Run()
        {
            Console.WriteLine("Hello GraphQL!");

            var schema = new AuthorsSchema();
            var authorsQuery = @"
                 query {
                 authors (name: ""Nicholas Cage"") {
                    id
                    name
                    country
                    books{
                       name
                       id
                       genres {
                               name
                               id
                  }}}}";
            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = authorsQuery;
            }).ConfigureAwait(false);

            var json = new DocumentWriter(indent: true).Write(result);

            Console.WriteLine(json);
            Console.Read();
        }
    }

    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public List<Book> books { get; set; }
    }
    public class Book
    {
        public String Name { get; set; }
        public int Id { get; set; }

        public List<Genre> Genres { get; set; }
    }
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GenreType : ObjectGraphType<Genre>
    {
        public GenreType()
        {
            Name = "Genre";
            Field(x => x.Id).Description("ID");
            Field(x => x.Name, nullable: true).Description("genre");
        }
    }

    public class AuthorsType : ObjectGraphType<Author>
    {
        public AuthorsType()
        {
            Name = "Author";
            Field(x => x.Id).Description("ID");
            Field(x => x.Name, nullable: true).Description("Name of author");
            Field(x => x.Country, nullable: true).Description("Country");
            Field<ListGraphType<BookType>>().Name("books")
                   .Description("list of books");
        }
    }

    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Name = "Book";
            Field(d => d.Id).Description("ID");
            Field(d => d.Name).Description("Name");
            Field<ListGraphType<GenreType>>(
                            "genres", "list of genre");
        }
    }
    public class AuthorsQuery : ObjectGraphType
    {
        public AuthorsQuery()
        {
            Field<AuthorsType>()
                .Name("authors")
                .Argument<NonNullGraphType<StringGraphType>>("name", "authors name")
                .Resolve(context =>
                  {
                      var name = context.GetArgument<string>("name");
                      List<Genre> genres = new List<Genre>();
                      genres.Add(new Genre() { Id = 1, Name = "Adventure" });
                      genres.Add(new Genre() { Id = 1, Name = "Romantic" });
                      List<Book> books = new List<Book>();
                      books.Add(new Book() { Id = 1, Name = "The Notebook", Genres = genres });
                      books.Add(new Book() { Id = 1, Name = "Game of Thrones", Genres = genres });
                      List<Author> authors = new List<Author>();
                      authors.Add(new Author() { Id = 1, Name = "Nicholas Cage", Country = "US", books = books });
                      authors.Add(new Author() { Id = 1, Name = "Ankit Fedia", Country = "US", books = books });
                      return (from x in authors where x.Name == name select x).FirstOrDefault();
                  }
                );
        }
    }
    public class AuthorsSchema : Schema
    {
        public AuthorsSchema()
        {
            Query = new AuthorsQuery();
            RegisterType<AuthorsType>();
        }
    }
}
