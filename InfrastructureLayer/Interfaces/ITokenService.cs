using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace InfrastructureLayer.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser? user);
    }
}