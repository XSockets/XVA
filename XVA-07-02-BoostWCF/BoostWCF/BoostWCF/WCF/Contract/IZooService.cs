using System.ServiceModel;
using System.ServiceModel.Web;

namespace BoostWCF.WCF.Contract
{
    [ServiceContract]
    public interface IZooService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Say/{message}")]
        string Say(string message);
    }
}
