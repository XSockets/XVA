using System.Collections.Generic;
using NgDataSync.Core.Model;

namespace NgDataSync.Core.ViewModel
{
    public class AnimalViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public int Age { get; set; }





        public AnimalViewModel() { }

        public AnimalViewModel(Animal entity, bool mapping = true)
            : base(entity)
        {
            this.Name = entity.Name;
            this.Age = entity.Age;


        }
    }
}

