using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserAuthenticationSystem.Models;

namespace UserAuthenticationSystem.Repositories
{
    public interface IHashAlgoRepo
    {
        public int AddHashAlgo(string algorithmname);
        public HashingAlgorithm GetAlgorithmByName(string algoname);
    }
}
