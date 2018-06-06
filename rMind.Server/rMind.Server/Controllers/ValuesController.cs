﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rMind.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace rMind.Server.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        IMindCore m_board;

        public ValuesController(IMindCore board)
        {
            m_board = board;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            var result = m_board.Check();
            return new {
                success = result
            };
        }

        [Route("switch/{pin}")]
        public object Switch(int pin)
        {
            return m_board.Switch(pin);
        }
    }
}
