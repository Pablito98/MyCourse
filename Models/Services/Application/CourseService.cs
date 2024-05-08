using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.ViewModels;
using MyCourse.Models.ValueTypes;
using MyCourse.Models.Enums;

namespace MyCourse.Models.Services.Application
{
    public class CourseService : ICourseService
    {
        //metodo che recupera la lista di tutti i corsi
        public List<CourseViewModel> GetCourses()
        {
            var courseList = new List<CourseViewModel>();
            var rand = new Random();
            for(int i = 1; i <= 20; i++){
                var price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
                var course = new CourseViewModel{
                    Id = i,
                    Titolo = $"Corso {i}",
                    Autore = $"Giorgia {i}",
                    ImgPath = "/logo.svg",
                    Rating = rand.Next(10,50)/10.0, //num decimale compreso fra 1 e 5
                    PrezzoFull = new Money(Currency.EUR, rand.NextDouble() > 0.5 ? price : price - 1),
                    PrezzoScontato = new Money(Currency.EUR, price)
                };
                courseList.Add(course);
            } 
            return courseList; 
        }

        //metodo che recupera i dettagli di un corso, dato l'id specifico
        public CourseDetailViewModel GetCourse(int id)
        {
            var rand = new Random();
            var price = Convert.ToDecimal(rand.NextDouble() * 10 + 10);
            var course = new CourseDetailViewModel{
                Id = id,
                Titolo = $"Corso {id}",
                Autore = $"Giorgia {id}",
                ImgPath = "/logo.svg",
                Rating = rand.Next(10,50)/10.0, //num decimale compreso fra 1 e 5
                PrezzoFull = new Money(Currency.EUR, rand.NextDouble() > 0.5 ? price : price - 1),
                PrezzoScontato = new Money(Currency.EUR, price),
                Descrizione = $"Descrizione {id}",
                Lezioni = new List<LessonViewModel>()
            };

            for(var i = 1; i <= 5; i++){
                var lezione = new LessonViewModel{
                    Titolo  = $"Lezione {id}",
                    Durata = TimeSpan.FromSeconds(rand.Next(40,90))
                };
                course.Lezioni.Add(lezione);
            }
            return course;
        }


     

    }
}