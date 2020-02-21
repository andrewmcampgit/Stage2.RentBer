using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBer.Models
{
    public class EditableProperty
    {
        public Guid? OwnerId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}