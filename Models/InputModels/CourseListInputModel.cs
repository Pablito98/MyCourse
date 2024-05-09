using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Customizations.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Options;
using Microsoft.Extensions.Options;


namespace MyCourse.Models.InputModels
{
    //tramite questa annotazione indichiamo ai valuesprovider che non devono piu creare singoli oggetti ma un unico oggetto di tipo courselistinputmodel
    [ModelBinder(BinderType = typeof(CourseListInputModelBinder))]  
    public class CourseListInputModel //classe che utilizziamo per mappare tutti i dati che arrivano dalla richiesta(form, query string ecc) in un oggetto 
    {
        public string Search {get; set;}
        public int Page {get;}

        public string OrderBy {get;}

        public bool Ascending {get;}

        public int Limit {get;}

        public int Offset {get;}


        public CourseListInputModel (string search, int page, string orderby, bool ascending)
        {
            
            Search = search ?? "";
            Page = Math.Max(1,page);
            OrderBy = orderby ?? "Rating";
            Ascending = ascending;
            Limit = 10;
            Offset = (Page - 1) * Limit;          
            

            if(orderby=="CurrentPrice")
            {
                    orderby = "CurrentPrice_Amount";
            }
            
            
        }
    }
    
}