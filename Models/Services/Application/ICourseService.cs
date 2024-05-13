using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.InputModels;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    //questa interfaccia include i metodi che un servizio,
    //per poter essere compatibile con il controller;
    //deve implementare affinchè funzioni
    public interface ICourseService
    {
      //Task<List<CourseViewModel>> GetCoursesAsync(string search, int page, string orderby, bool ascending);
      
      Task<List<CourseViewModel>> GetCoursesAsync(CourseListInputModel model);
      Task<CourseDetailViewModel> GetCourseAsync(int id);

      Task<CourseDetailViewModel>CreateCourseAsync(CourseCreateInputModel inputModel);
        

    }
}