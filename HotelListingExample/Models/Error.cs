﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HotelListingExample.Models
{
    public class Error
    {

        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

        //public override string ToString()
        //{
        //    return JsonConvert.SerializeObject(this);
        //}
    }
}
