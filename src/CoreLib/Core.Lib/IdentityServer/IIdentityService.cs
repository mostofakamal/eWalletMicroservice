using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.IdentityServer
{
    public interface IIdentityService
    {
        string GetUserIdentity();

        string GetUserName();
    }
}
