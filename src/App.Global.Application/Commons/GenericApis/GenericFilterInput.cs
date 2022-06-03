using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace App.Global.Commons.GenericApis
{
    public class GenericFilterInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int? Status { get; set; }
    }
}
