using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.Entities;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using MyCourse.Models.InputModels;

namespace MyCourse.Models.Services.Application
{
    public class EfCoreCourseService : ICourseService
    { 
        private readonly MyCourseDbContext dbContext;

        public EfCoreCourseService(MyCourseDbContext dbContext){
            this.dbContext = dbContext;
        }

        //recupera tutte le info dei corsi da database
        //SELECT * FROM Courses
        //public async Task<List<CourseViewModel>> GetCoursesAsync(string search, int page, string orderby, bool ascending)
        public async Task<List<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {  //search=search ?? ""; // operatore per verificare che search non sia nullo, se è nullo viene settato a stringa vuota
           //int limit = 10;
            //int offset=(page-1)*limit;

            //orderby=orderby ?? "Rating";
            
            IQueryable<Course> baseQuery =  dbContext.Courses;
            switch(model.OrderBy){
                case "Title":
                    if(model.Ascending){
                        baseQuery = baseQuery.OrderBy(course => course.Title);
                    }
                    else{
                        baseQuery = baseQuery.OrderByDescending(course => course.Title);
                    }break;
                case "Rating":
                    if(model.Ascending){
                        baseQuery = baseQuery.OrderBy(course => course.Rating);
                    }
                    else{
                        baseQuery = baseQuery.OrderByDescending(course => course.Rating);
                    }
                    break;
                case "CurrentPrice":
                    if(model.Ascending){
                        baseQuery = baseQuery.OrderBy(course => (double)course.CurrentPrice.Amount);
                    }
                    else{
                        baseQuery = baseQuery.OrderByDescending(course => (double)course.CurrentPrice.Amount);
                    }
                    break;
            }
        
            //crea un oggetto di tipo List<CourseViewModel> che verrà passato alla View
            //stavolta questo oggetto lo popola utilizzando il metodo Select del Framework Entity Core
            //il quale richiede una espressione lambda tramite cui popoliamo gli oggetti di tipo Entity Course
            //List<CourseViewModel> courses = await dbContext.Courses;
            IQueryable<CourseViewModel> queryLinq = baseQuery
            .Skip(model.Offset) //skip salta i primi risultati (offset valori) come OFFSET in ADO 
            .Take(model.Limit)  //prende i primi valori (limit) come LIMIT in ADO
            .AsNoTracking()
            .Where(course => course.Title.Contains(model.Search))
            .Select(course => CourseViewModel.FromEntity(course));
            /*.Select(course => new CourseViewModel 
            {
                Id = course.Id,
                Titolo = course.Title,
                ImgPath = course.ImagePath,
                Autore = course.Author,
                Rating = course.Rating,
                PrezzoFull = course.FullPrice,
                PrezzoScontato = course.CurrentPrice
            });//.ToListAsync();*/

            List<CourseViewModel> courses = await queryLinq.ToListAsync();//Viene aperta una connessione con il db e inviata la query al database 

            return courses; 
        }

        //metodo che recupera il corso che ha id come parametro
        //SELECT * FROM Courses Where Id=id
        public async Task<CourseDetailViewModel> GetCourseAsync(int id)
        {
                //CourseDetailViewModel viewModel = await dbContext.Courses
                IQueryable<CourseDetailViewModel> queryLinq = dbContext.Courses
                .AsNoTracking()
                .Include(course => course.Lessons)
                .Where(course => course.Id == id)
                .Select(course => CourseDetailViewModel.FromEntity(course));
                /*.Select(course => new CourseDetailViewModel 
                {
                    Id = course.Id,
                    Titolo = course.Title,
                    Descrizione = course.Description,
                    ImgPath = course.ImagePath,
                    Autore = course.Author,
                    Rating = course.Rating,
                    PrezzoFull = course.FullPrice,
                    PrezzoScontato = course.CurrentPrice,

                    //recupero tutte le lezioni del corso che ho già recuperato con la query precedente       
                    Lezioni = course.Lessons.Select(lesson => new LessonViewModel
                    {
                        Id= lesson.Id,
                        Titolo = lesson.Title,
                        Durata = lesson.Duration
                    }).ToList()//si connette al database e recupera la lista delle lezioni associate al corso
                });//.SingleAsync();//Restituisce il primo elemento dell'elenco, ma se l'elenco ne contiene 0 o più di 1, solleva un'eccezione*/

                CourseDetailViewModel viewModel = await queryLinq.SingleAsync();//qui avviene la connessione con in db e l'esecuzione della query

                return viewModel;
        }
    }
}