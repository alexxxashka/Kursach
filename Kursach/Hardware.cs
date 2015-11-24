using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    public class Hardware
    {
        public string Section { set; get; }
        public string Type { set; get; }
        public string Firm { set; get; }
        public string Model { set; get; }
        public int Price { set; get; }
        public string Comment { set; get; }

        public Hardware()
        {

        }
        public Hardware(string newSection = "Не задано", string newType = "Не задано", string newFirm = "Не задано", string newModel = "Не задано", int newPrice = 0, string newComment = "Не задано")
        {
            Section = newSection;
            Type = newType;
            Firm = newFirm;
            Model = newModel;
            Price = newPrice;
            Comment = newComment;
        }
    }

}
