using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module5HW1.Models
{
    public class Response<T>
    {
        public int Page { get; init; }
        public int Per_Page { get; init; }
        public int Total { get; init; }
        public int Total_Pages { get; init; }
        public List<T> Data { get; init; }
    }
}
