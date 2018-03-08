﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DXGame.Api.Models;
using DXGame.Common.Communication;
using Microsoft.AspNetCore.Mvc;

namespace DXGame.Api.Controllers
{
    [Route("api/[controller]")]
    public class PlayroomsController : Controller
    {
        ICache _cache;
        IMessageBus _messageBus;

        public PlayroomsController(ICache cache, IMessageBus messageBus)
        {
            _cache = cache;
            _messageBus = messageBus;
        }
        
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
