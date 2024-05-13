using System;
using System.Collections.Generic;
using System.Linq;
using MyCourse.Models.Services.Infrastructure;
using System.Data;
using System.Threading.Tasks;
using MyCourse.Models.ViewModels;
using MyCourse.Models.ValueTypes;
using MyCourse.Models.InputModels;


namespace MyCourse.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService //servizio applicativo che ha il compito di recuperare dati dal database e lo fa tramite l utilizzo del servizio infrastrutturale IDatabaseAccessor
    {
        private readonly IDatabaseAccessor db; // proprieta che deve essere iniettata nel servizio applicativo

        public AdoNetCourseService(IDatabaseAccessor db) // dipendenza debole
        {
            this.db=db;
        }
        //public async Task<List<CourseViewModel>> GetCoursesAsync(string search, int page, string orderby, bool ascending)
        public async Task<List<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
         {
            
            string orderby = model.OrderBy;
            string direction = "";
            
                    if(model.Ascending == true){
                        direction ="ASC";
                    }
                    else{direction = "DESC";}
            
            

            if(orderby=="CurrentPrice")
            {
                    orderby = "CurrentPrice_Amount";
            }
            
            FormattableString query= $"SELECT Id, Title,ImagePath, Author,Rating,FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Title LIKE {"%"+model.Search+"%"} ORDER BY {(Sql) orderby} {(Sql) direction} LIMIT {model.Limit} OFFSET {model.Offset}"; //con LIKE e {"%"+search+"%"}"  gli diciamo che nella condizione search deve essere contenuto nei titoli dei corsi
            DataSet dataSet=await db.QueryAsync(query);
            var dataTable= dataSet.Tables[0]; //recupera la prima tabella del dataset
            var courseList = new List<CourseViewModel>(); //crea la lista di corsi che deve eseere passata all view
            // per ogni riga presente nalla datatable deve creare un oggetto di tipo CourseViewModel 
            foreach(DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow);
                courseList.Add(course);
            }
            return courseList;
         }
       public async Task<CourseDetailViewModel> GetCourseAsync(int id)
       {
           FormattableString query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id}
            ; SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";
            DataSet dataSet=await db.QueryAsync(query);
            var courseTable = dataSet.Tables[0];
            
            //dato che la prima query mi ritorna esattamente una riga, controllo se effettivamente 
            //la tabella contiene una sola riga tramite Rows.Count
            if (courseTable.Rows.Count != 1) {
                throw new InvalidOperationException($"Did not return exactly 1 row for Course {id}");
            }
            //recupero i dati della prima riga della tabella -> quindi i dati del corso con id recuperato
            var courseRow = courseTable.Rows[0];
            //creo l'oggetto ViewModel popolandolo con i dati recuperati da db
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow);

            //recupero i dati della tabella risultato della seconda query
            var lessonTable = dataSet.Tables[1];

            //per ogni riga (lezione) presente nella tabella crea una lezione (oggetto LessonViewModel)
            //popolandolo con i dati letti da database
            foreach(DataRow lessonRow in lessonTable.Rows){
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow);
                //aggiungi la lezione alla lista di lezioni del corso
                courseDetailViewModel.Lezioni.Add(lessonViewModel);
            }

            return courseDetailViewModel;


        }

       public async Task<CourseDetailViewModel>CreateCourseAsync(CourseCreateInputModel inputModel)
       {
            string title = inputModel.Title;
            string author = "paolo";
            FormattableString query = $@"INSERT INTO Courses (Title,Author,ImagePath,CurrentPrice_Currency,CurrentPrice_Amount,FullPrice_Currency,FullPrice_Amount) VALUES ({title},{author},'/Courses/default.png','EUR',0,'EUR',0);
            SELECT last_insert_rowid();";//questa query recupera l'ultimo id inserito nel db (utile quando l'id è autoincrement)
            

            DataSet dataSet=await db.QueryAsync(query);
            int courseId = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);//il dataset contiene il risultato della SELECT (quindi l'id ultimo inserito)
            CourseDetailViewModel course = await GetCourseAsync(courseId);
            return course;
       }
    }
}