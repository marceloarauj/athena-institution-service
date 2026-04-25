using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<string> list =
            [
                "Item 1",
                "Item 2",
                "Item 3",
                "Sucumba Rebeca",
                "Sucumba imediatamente"
            ];

            return Ok(list);
        }
    }
}