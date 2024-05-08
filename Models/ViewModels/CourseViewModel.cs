using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MyCourse.Models.Enums;
using MyCourse.Models.ValueTypes;


namespace MyCourse.Models.ViewModels
{
    //classe che rappresenta l'entità CORSO
    public class CourseViewModel
    {

        public int Id {get; set;}
        public string Titolo {get; set;}
        public string ImgPath {get; set;}
        public string Autore {get; set;}
        public double Rating {get; set;}
        public Money PrezzoFull {get; set;}
        public Money PrezzoScontato {get; set;}

        public static CourseViewModel FromDataRow(DataRow courseRow)
        {
            var courseViewModel = new CourseViewModel
            {
                Titolo= Convert.ToString(courseRow["Title"]), //recupero il titolo dal db leggendo la colonna title
                ImgPath = Convert.ToString(courseRow["ImagePath"]),
                Autore = Convert.ToString(courseRow["Author"]),
                Rating = Convert.ToDouble(courseRow["Rating"]),
                PrezzoFull = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["FullPrice_Currency"])),
                    Convert.ToDecimal(courseRow["FullPrice_Amount"])
                ),
                PrezzoScontato = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["CurrentPrice_Currency"])),
                    Convert.ToDecimal(courseRow["CurrentPrice_Amount"])
                ),
                Id = Convert.ToInt32(courseRow["Id"])
            };
            return courseViewModel;

            
        }
    }
}