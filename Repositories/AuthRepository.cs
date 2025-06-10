using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FargoApi.Services.Whatsapp;
using Microsoft.EntityFrameworkCore;
using project_API.data;
using project_API.Model;
using project_API.Repositories.Interface;

namespace project_API.Repositories;

    public class AuthRepository : IAuthRepository
    {
        private readonly appdbcontext _context;
        private readonly WhatsAppManger _whatsAppManger;
        public AuthRepository(appdbcontext context, WhatsAppManger whatsAppManger)
        {
            _context = context;
            _whatsAppManger = whatsAppManger;
        }


        public async Task<bool> Register(User user)
        {
          
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);
            if (existingUser != null)
                return false;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Login(string phoneNumber, string code)
        {
            
            
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null)
                return false;

            var validCode = await _context.Codes
                .FirstOrDefaultAsync(c => c.User.PhoneNumber == phoneNumber && c.CodeValue == code && !c.IsUsed);

            if (validCode == null || validCode.IsUsed)
                return false;

            validCode.IsUsed = true;
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> SendVerificationCode(string phoneNumber)
        {
            // Example logic: generate a code, store it, and pretend to send (e.g., email/SMS service integration here)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null) return false;
            
            // add to codes table
            var code = new Code
            {
             
                User = user,
                CodeValue = new Random().Next(1000, 9999).ToString(),
                IsUsed = false
                
                
            };
            _context.Codes.Add(code);
            
            await _context.SaveChangesAsync();
            return true;
        }
        

        public async Task<bool> CheckUser(string phoneNumber)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            return user != null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }


    }
