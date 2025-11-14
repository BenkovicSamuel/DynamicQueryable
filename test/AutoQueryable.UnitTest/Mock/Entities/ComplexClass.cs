using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoQueryable.UnitTest.Mock.Entities
{
    [ComplexType]
    public class ComplexClass
    {
        public int Key { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Value { get; set; }
        public string Value2 { get; set; }
    }
}
