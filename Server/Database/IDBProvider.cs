using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Database
{
    interface IDBProvider
    {
        bool CreateUser(User user);
        User GetUser(string username);
        bool UpdateUser(User user);

        bool CreateKey(Key key);
        Key GetKey(string id);
        bool DeleteKey(Key key);
    }
}
