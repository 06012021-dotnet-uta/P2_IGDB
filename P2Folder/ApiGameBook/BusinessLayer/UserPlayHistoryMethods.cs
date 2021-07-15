﻿using Microsoft.EntityFrameworkCore;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserPlayHistoryMethods
    {
        private gamebookdbContext _context;
      
        public UserPlayHistoryMethods(gamebookdbContext context)
        {
            _context = context;
        }
        // Create play history
        /// <summary>
        /// Construction of play history of specific games
        /// </summary>
        /// <param name="user">The user who is adding the game</param>
        /// <param name="game">The game to add</param>
        /// <returns>True if able to add game</returns>
        public async Task <bool> CreatePlayHistoryAsync(User user, Game game)
        {
            bool success = false;
            try
            {
                if (_context.Games.Where(x => x.GameId == game.GameId).FirstOrDefault() == null)
                {
                    Console.WriteLine("Game not found.");
                    return success;
                }
                PlayHistory history = new PlayHistory()
                {
                    UserId = user.UserId,
                    GameId = game.GameId,
                };
                await _context.PlayHistories.AddAsync(history);
                await _context.SaveChangesAsync();
                success = true;
                return success;
            }
            catch
            {
                Console.WriteLine("Error, play history not added");
            }


            return success;
        }
        /// <summary>
        /// Deletes a game from the history 
        /// </summary>
        /// <param name="history">Takes in the history to delete</param>
        /// <returns>True if able to be deleted</returns>
        public async Task<bool> DeletePlayHistoryAsync(PlayHistory history)
        {
            bool success = false;
            try
            {
                _context.Remove(history);
                await _context.SaveChangesAsync();
                success = true;
                return success;
            }
            catch
            {
                Console.WriteLine("Error, play hisotry not removed");
            }
            return success;
        }
        /// <summary>
        /// Allows search of play history 
        /// </summary>
        /// <param name="userid">User who's Id we are searching</param>
        /// <param name="gameid">Game id for the game searching</param>
        /// <returns></returns>
        public async Task< PlayHistory> SearchPlayHistoryAsync(int userid, int gameid)
        {
            PlayHistory temp = null;
            temp = await _context.PlayHistories.Where(x => (x.UserId == userid && x.GameId == gameid) ).FirstOrDefaultAsync();
            return temp;
        }
    }
}
