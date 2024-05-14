using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyCourse.Customizations.TagHelpers
{
    //classe che rappresenta la creazione del nuovo tag helper <rating>
    //Questa classe deve chiamarsi esattamente come il nome del tag che voglio utilizzare seguita da TagHelper
    public class RatingTagHelper : TagHelper
    {
        //proprietà in cui verrà iniettato il valore dell'attributo del tag rating
        public double Value {get;set;}

        //Metodo che viene invocato, nel momento in cui Razor nella view trova il tag <rating>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //implemento tutta la logica che prima era presente nel file .cshtml
            for(int i = 1; i <= 5; i++)
            {
                if (Value >= i)
                {
                    //tramite l'oggetto di tipo TagHelperOutput genero codice HTML
                    //usando la proprietà Content e il metodo AppendHtml
                    output.Content.AppendHtml("<i class=\"fas fa-star\"></i>");
                }
                else if (Value > i - 1)
                {
                    output.Content.AppendHtml("<i class=\"fas fa-star-half-alt\"></i>");
                }
                else
                {
                    output.Content.AppendHtml("<i class=\"far fa-star\"></i>");
                }
            }
        }
    }
}