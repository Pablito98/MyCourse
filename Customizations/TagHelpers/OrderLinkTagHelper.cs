using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MyCourse.Models.InputModels;

namespace MyCourse.Customizations.TagHelpers
{
    public class OrderLinkTagHelper : AnchorTagHelper  //dato che questo componente sono tutti link allora estendo direttamente la classa AnchorTagHelper (<a>)
    {

        public string OrderBy {get;set;}
        public CourseListInputModel Input {get;set;}

        public OrderLinkTagHelper(IHtmlGenerator generator) : base(generator){

        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            //Imposto i valori dei Link
            RouteValues["search"] = Input.Search;
            RouteValues["orderby"] = OrderBy;
            RouteValues["ascending"] = (Input.OrderBy == OrderBy ? !Input.Ascending : Input.Ascending).ToString().ToLowerInvariant();

            base.Process(context,output);

            //Aggiungo l'indicatore di direzione
            if(Input.OrderBy == OrderBy){
                var direction = Input.Ascending ? "up" : "down";
                output.PostContent.SetHtmlContent($" <i class=\"fas fa-caret-{direction}\"></i>");
            }




        }
    }
}