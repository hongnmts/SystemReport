using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using SystemReport.WebAPI.Interfaces;

namespace SystemReport.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class TestController : ControllerBase
    {
        private IRowValueService _rowValueService;
        public TestController(IRowValueService rowValueService)
        {
            _rowValueService = rowValueService;
        }

        // [HttpGet]
        // [Route("enum")]
        // public async Task<IActionResult> TestEnumGetDisplay()
        // {
        //     var data = EnumExtensions.GetValue(EHinhThucGui, "Thap");
        //     return Ok(new {data,data1}); 
        // }

        //public IActionResult TestValid()
        //{

        //}

        [HttpGet]
        [Route("TestFormula")]
        public IActionResult TestFormula(string str)
        {
            // string formula = str; //or get it from DB
            // StringToFormula stf = new StringToFormula();
            // double result = stf.Eval(formula);
            // var between = "<81818> + <21231>".Replace("<81818>", "abc");
            // between = between.Replace("<21231>", "123");
            
            // string str1 = "<81818> + <21231>";
            // List<string> arrays = new List<string>();
            // foreach (Match match in Regex.Matches(str1,"<[^}]+>"))
            // {
            //     arrays.Add(match.Value);
            // }
            var data = _rowValueService.StringCongThuc("634412828a34374246ee9adc");
            return Ok(data);
        }
    }
}