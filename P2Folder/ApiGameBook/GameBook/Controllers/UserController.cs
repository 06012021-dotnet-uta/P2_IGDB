﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserMethods _userMethods;

        public UserController(UserMethods userMethods)
        {
            _userMethods = userMethods;
        }

        // GET: api/<UserController>
        [HttpGet]
        public List<User> Get()
        {
            return _userMethods.UsersList();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _userMethods.SearchUserByID(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public bool Post(User user)
        {
            return _userMethods.CreateUser(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            User user = _userMethods.SearchUserByID(id); // doesnt work right now
            return _userMethods.DeleteUser(user);
        }
    }
}
