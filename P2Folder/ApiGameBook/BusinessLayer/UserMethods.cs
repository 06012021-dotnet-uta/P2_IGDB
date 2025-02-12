﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserMethods : IUserMethods
    {
        private gamebookdbContext _context;
        private readonly ILogger<UserMethods> _logger;

        public UserMethods(gamebookdbContext context)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<UserMethods>();
            _context = context;
        }
        public UserMethods(ILogger<UserMethods> logger, gamebookdbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Searches for users based on user name then allows creation of one if there is no double
        /// </summary>
        /// <param name="user">It takes a user object</param>
        /// <returns>Returns true if no other user with the same username exist and the user was succesfully created</returns>
        public bool CreateUser(User user)
        {
            bool success = false;

            try
            {
                User searchUser = SearchUserByUsername(user.Username); //Find if a username is already taken
                if (searchUser == null)
                {
                    // add user to db, change success to true if successful
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    success = true;
                    return success;
                }
                else
                {
                    _logger.LogWarning("WARNING: That username already exists");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return success;

        }

        /// <summary>
        /// Allows the ability to delete user
        /// </summary>
        /// <param name="user">Takes the name of the user to delete</param>
        /// <returns>Returns true on success</returns>
        public bool DeleteUser(User user)
        {
            bool success = false;

            try
            {
                //check if user exists in database
                if (SearchUserByUsername(user.Username) == null)
                {
                    _logger.LogWarning("WARNING: User not found");
                    return success;
                }
                else
                {
                    // delete user from db, change success to true if successful
                    _context.Users.Remove(user);
                    _context.SaveChanges(); //works if used synchronously
                    success = true;
                    return success;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return success;
        }

        /// <summary>
        /// Searches type users by username
        /// </summary>
        /// <param name="username">Username property of user</param>
        /// <returns>Type user</returns>
        public User SearchUserByUsername(string username)
        {
            User temp = null;
            try
            {
                // Search users table for user with matching name, returns null if not found
                temp = _context.Users.Where(x => x.Username == username).FirstOrDefault();
                if (temp == null)
                    _logger.LogWarning("WARNING: User not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return temp;
        }

        /// <summary>
        /// Searches type Id by username
        /// </summary>
        /// <param name="userId">Id property of user</param>
        /// <returns>Type user</returns>
        public User SearchUserByID(int userId)
        {
            User temp = null;
            try
            {
                // Search users table for user with matching id, returns null if not found
                temp = _context.Users.Where(x => x.UserId == userId).FirstOrDefault();
                if (temp == null)
                    _logger.LogWarning("WARNING: User not found");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return temp;
        }

        /// <summary>
        /// Gets list of users.
        /// </summary>
        /// <returns>List of Users in database.</returns>
        public List<User> UsersList()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Updates previous user data with new user data
        /// </summary>
        /// <param name="oldUser">User to be changed</param>
        /// <param name="newUser">User model that replaces old user's data</param>
        /// <returns>Returns true if old user's data was updated</returns>
        public bool EditUser(User oldUser, User newUser)
        {
            bool success = false;

            try
            {
                if (newUser.Username == null || newUser.Password == null || newUser.FirstName == null || newUser.LastName == null || newUser.Email == null)
                {
                    _logger.LogWarning("WARNING: The user model entered has missing data");
                    return success;
                }
                else
                {
                    oldUser.Username = newUser.Username;
                    oldUser.Password = newUser.Password;
                    oldUser.FirstName = newUser.FirstName;
                    oldUser.LastName = newUser.LastName;
                    oldUser.Email = newUser.Email;

                    _context.Users.Update(oldUser);
                    _context.SaveChanges();
                    success = true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return success;
        }
    }
}
