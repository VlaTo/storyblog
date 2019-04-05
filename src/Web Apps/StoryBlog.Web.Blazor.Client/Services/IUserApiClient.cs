﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task LoginAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IPrincipal> SigninCallbackAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetUserInfoAsync(string token);
    }
}