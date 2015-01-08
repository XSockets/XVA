using NgDataSync.Core.Model;
using NgDataSync.Core.Interfaces.Service;
using NgDataSync.Core.Interfaces.Data;
using NgDataSync.Core.ViewModel;

namespace NgDataSync.Service
{ 
	//NOTE:
	//If you need to implement your own logic/code do it in a partial class,
	//modifications in this file may be overwritten.
    public partial class AnimalService : BaseService<Animal,AnimalViewModel>, IAnimalService
    {
		protected new IAnimalRepository Repository;				
		
		public AnimalService(IUnitOfWork unitOfWork, IAnimalRepository animalRepository):base(unitOfWork)
		{
		    base.Repository = Repository = animalRepository;
		}		
		//Implement custom code in a partial class
	}
}