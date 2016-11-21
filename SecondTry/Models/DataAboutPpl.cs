using System;
using System.Data.Entity;

namespace SecondTry.Models
{
    public class People
    {
       
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
    }

    public class PeopleDBContext : DbContext
    {
        public DbSet<People> People { get; set; }
    }
}