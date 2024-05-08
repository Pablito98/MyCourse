using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.Enums;

namespace MyCourse.Models.ValueTypes
{
    //questa classe serve unicamente per indicare al servizio infrastrutturale che un dato parametro non deve essere convertito in un Sqlite parameter
    public class Sql 
    {
        private Sql(string value)
        {
            Value = value;
        }
        //Proprietà per conservare il valore originale
        public string Value { get; }

        //Conversione da/per il tipo string
        public static explicit operator Sql(string value) => new Sql(value);
        public override string ToString() {
            return this.Value;
        }
    }
}