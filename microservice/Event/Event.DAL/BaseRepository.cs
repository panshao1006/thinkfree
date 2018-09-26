using Core.ORM.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Event.DAL
{
    public class BaseRepository
    {
        protected DapperExtension _dal = new DapperExtension();
    }
}
