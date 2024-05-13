using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourse.Models.InputModels
{
    public class CourseCreateInputModel
    {
        public string Title {get;set;} //nella view ho indicato per un campo input text la proprieta asp-for="Title" questo significa che il valore immesso all interno di quella input text verra automaticamente iniettato in questa proprieta
    }
}