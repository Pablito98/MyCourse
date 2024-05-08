using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MyCourse.Models.Enums;
using MyCourse.Models.ValueTypes;


namespace MyCourse.Models.ViewModels
{
    public class LessonViewModel   
    {
        public int Id{get; set;}
        public string Titolo {get; set;}
        public TimeSpan Durata {get; set;}   

        public static LessonViewModel FromDataRow(DataRow dataRow)
        {
            var lessonViewModel = new LessonViewModel {
                Id = Convert.ToInt32(dataRow["Id"]),
                Titolo = Convert.ToString(dataRow["Title"]),
                Durata = TimeSpan.Parse(Convert.ToString(dataRow["Duration"]))
            };

            return lessonViewModel;
        }


    }
}