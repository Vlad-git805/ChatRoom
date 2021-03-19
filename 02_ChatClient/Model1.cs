namespace _02_ChatClient
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class Model1 : DbContext
    {

        public Model1()
            : base("name=Model1")
        {
            Database.SetInitializer(new Initializer());
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Tag { get; set; }
        public string Port { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<string> messages { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}