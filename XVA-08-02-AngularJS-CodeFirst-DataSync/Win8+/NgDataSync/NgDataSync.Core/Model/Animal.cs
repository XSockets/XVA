using System;

namespace NgDataSync.Core.Model
{
    public class Animal : PersistentEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}