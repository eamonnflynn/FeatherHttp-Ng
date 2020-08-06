using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NgFeatherHttp.Web
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
       
            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var app = builder.Build();

            app.MapGet("/api/person",GetPerson);
            app.MapPost("/api/person", CreatePerson);
            app.MapPut("/api/person", UpdatePerson);
            app.MapDelete("/api/person", DeletePerson);


            if (!app.Environment.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (app.Environment.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            await app.RunAsync();
        }

        private static async Task DeletePerson(HttpContext http)
        {
            if (!http.Request.RouteValues.TryGet("id", out int id))
            {
                http.Response.StatusCode = 400;
                return;
            }

            await using var db = new PersonContext();
            var person = await db.Persons.FindAsync(id);

            if (person == null)
            {
                http.Response.StatusCode = 404;
                return;
            }

            db.Persons.Remove(person);

            await db.SaveChangesAsync();

            http.Response.StatusCode = 204;
        }

        private static async Task UpdatePerson(HttpContext http)
        {
            if (!http.Request.RouteValues.TryGet("id", out int id))
            {
                http.Response.StatusCode = 400;
                return;
            }

            await using var db = new PersonContext();
            var person = await db.Persons.FindAsync(id);

            if (person == null)
            {
                http.Response.StatusCode = 404;
                return;
            }

            var inputPerson = await http.Request.ReadJsonAsync<PersonItem>();

            person.Name = inputPerson.Name;
            person.Email = inputPerson.Email;
           
            await db.SaveChangesAsync();

            http.Response.StatusCode = 204;
        }

        private static async Task CreatePerson(HttpContext http)
        {
            var todo = await http.Request.ReadJsonAsync<PersonItem>();

            await using var db = new PersonContext();
            await db.Persons.AddAsync(todo);
            await db.SaveChangesAsync();

            http.Response.StatusCode = 204;
        }

        private static async Task GetPerson(HttpContext http)
        {
            await using var db = new PersonContext();
            var todos = await db.Persons.ToListAsync();

            await http.Response.WriteJsonAsync(todos);
        }
    }
}
