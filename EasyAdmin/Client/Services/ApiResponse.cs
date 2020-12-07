using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyAdmin.Client.Services
{
    public class ApiResponse<T>
    {
        public bool isSuccess { get; set; }
        public T Result { get; set; }
        public string errorMessage { get; set; }
    }
}
